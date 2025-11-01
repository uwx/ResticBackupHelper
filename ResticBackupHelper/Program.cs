// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using ResticBackupHelper;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

try
{
    Console.WriteLine("Hello, World!");

    YamlStructure structure;
    using (var input = new StreamReader(File.OpenRead("backups.yml")))
    {
        var deserializer = new StaticDeserializerBuilder(new YamlStaticContext())
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        structure = deserializer.Deserialize<YamlStructure>(input);
    }

    Console.WriteLine(structure);

    using var emptyDir = new TemporaryDirectory();

    foreach (var (mainTag, backup) in structure.Backups)
    {
        var psi = new ProcessStartInfo
        {
            FileName = structure.Meta.ResticExecutable,
            Verb = "runas",
            Environment =
            {
                ["RESTIC_PASSWORD"] = structure.Meta.Password,
                ["RESTIC_REPOSITORY"] = structure.Meta.Repository,
                ["RESTIC_PACK_SIZE"] = structure.Meta.PackSize.ToString(),
            },
            WorkingDirectory = backup.Path != null
                ? Environment.ExpandEnvironmentVariables(backup.Path)
                : Environment.CurrentDirectory,
        };

        if (backup.Path != null && !Directory.Exists(backup.Path))
        {
            throw new InvalidOperationException("The specified backup path does not exist: " + backup.Path);
        }

        psi.ArgumentList.Add("backup");

        using var filesFrom = new TemporaryFile(".txt");
        using var excludeFrom = new TemporaryFile(".txt");
        using var iexcludeFrom = new TemporaryFile(".txt");

        if (backup.Include.Length > 0)
        {
            await using var stream = new StreamWriter(File.OpenWrite(filesFrom.FilePath));
            foreach (var include in backup.Include)
            {
                await stream.WriteLineAsync(Environment.ExpandEnvironmentVariables(include));
            }

            psi.ArgumentList.Add("--files-from");
            psi.ArgumentList.Add(filesFrom.FilePath);
        }
        else
        {
            psi.ArgumentList.Add(".");
        }

        if (backup.Exclude.Length > 0)
        {
            await using var stream = new StreamWriter(File.OpenWrite(excludeFrom.FilePath));
            foreach (var include in backup.Exclude)
            {
                await stream.WriteLineAsync(Environment.ExpandEnvironmentVariables(include));
            }

            psi.ArgumentList.Add("--exclude-file");
            psi.ArgumentList.Add(excludeFrom.FilePath);
        }

        if (backup.Iexclude.Length > 0)
        {
            await using var stream = new StreamWriter(File.OpenWrite(iexcludeFrom.FilePath));
            foreach (var include in backup.Iexclude)
            {
                await stream.WriteLineAsync(Environment.ExpandEnvironmentVariables(include));
            }

            psi.ArgumentList.Add("--iexclude-file");
            psi.ArgumentList.Add(iexcludeFrom.FilePath);
        }

        psi.ArgumentList.Add("--tag");
        psi.ArgumentList.Add(mainTag);

        foreach (var tag in backup.Tags)
        {
            psi.ArgumentList.Add("--tag");
            psi.ArgumentList.Add(tag);
        }

        if (backup.UseVss)
        {
            psi.ArgumentList.Add("--use-fs-snapshot");
        }

        psi.ArgumentList.Add("--compression");
        psi.ArgumentList.Add("max");

        if (args.Contains("--dry-run"))
        {
            psi.ArgumentList.Add("--dry-run");
        }

        foreach (var arg in backup.AdditionalArgs)
        {
            psi.ArgumentList.Add(arg);
        }

        Console.WriteLine($"""
                           cd {psi.WorkingDirectory}
                           """);
        Console.WriteLine($"""
                           restic "{string.Join(@""" """, psi.ArgumentList)}"
                           """);

        using var proc = Process.Start(psi)!;
        await proc.WaitForExitAsync();

        if (proc.ExitCode != 0 && !args.Contains("--ignore-errors"))
        {
            throw new InvalidOperationException("Restic Backup process has exited with code " + proc.ExitCode);
        }
    }
}
finally
{
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
}
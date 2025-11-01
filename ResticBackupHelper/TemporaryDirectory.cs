using System.Diagnostics.CodeAnalysis;

namespace ResticBackupHelper;

public class TemporaryDirectory : IDisposable
{
    public TemporaryDirectory() :
        this(Path.GetTempPath())
    {
    }

    public TemporaryDirectory(string directory)
    {
        Create(Path.Combine(directory, Path.GetRandomFileName()));
    }

    ~TemporaryDirectory()
    {
        Delete();
    }

    public void Dispose()
    {
        Delete();
        GC.SuppressFinalize(this);
    }

    public string FilePath { get; private set; }

    [MemberNotNull(nameof(FilePath))]
    private void Create(string path)
    {
        FilePath = path;
        Directory.CreateDirectory(FilePath);
    }

    private void Delete()
    {
        if (FilePath == null!) return;
        Directory.Delete(FilePath, false);
        FilePath = null!;
    }
}
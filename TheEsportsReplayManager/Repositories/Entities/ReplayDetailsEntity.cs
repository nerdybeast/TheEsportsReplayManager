using SQLite;

namespace TheEsportsReplayManager.Repositories.Entities;

public class ReplayDetailsEntity
{
    [PrimaryKey]
    [AutoIncrement]
    public int Id { get; set; }

    public string FilePath { get; set; } = null!;

    [Ignore]
    public string FileName => Path.GetFileNameWithoutExtension(FilePath);
}

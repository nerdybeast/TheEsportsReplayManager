namespace TheEsportsReplayManager.Models;

public class ReplayDetail
{
    public string? ReplayId { get; set; }
    public string? ReplayName { get; set; }
    public string? FilePath { get; set; }
    public string? FileName => Path.GetFileNameWithoutExtension(FilePath);
}

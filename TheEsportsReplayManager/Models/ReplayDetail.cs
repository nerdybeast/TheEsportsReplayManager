//using EsportsRocketLeagueReplayParser.Models;

namespace TheEsportsReplayManager.Models;

public class ReplayDetail
{
    /// <summary>
    /// Sqlite database Id
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Unique per user who saves a replay (even if multiple players save
    /// the same replay for match)
    /// </summary>
    public string? ReplayId { get; set; }

    public string? ReplayName { get; set; }
    public string? FilePath { get; set; }
    public string? FileName => Path.GetFileNameWithoutExtension(FilePath);
    //public ParsedReplay? Replay { get; set; }
    //public bool IsParsed => Replay != null;

    public bool HasErrors => Errors.Any();

    public List<ReplayDetailError> Errors { get; set; } = new();
}

public class ReplayDetailError
{
    public string Message { get; set; } = null!;
}

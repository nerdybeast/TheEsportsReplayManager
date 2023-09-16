namespace TheEsportsReplayManager.Models;

public class ReplayState : ReplayDetail
{
    public bool Loaded { get; set; }

    public static ReplayState CreateReplayState(ReplayDetail replayDetail)
    {
        return new ReplayState
        {
            ReplayId = replayDetail.ReplayId,
            ReplayName = replayDetail.ReplayName,
            FilePath = replayDetail.FilePath,
        };
    }
}

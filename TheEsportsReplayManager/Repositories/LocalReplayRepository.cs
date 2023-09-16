using TheEsportsReplayManager.Models;

namespace TheEsportsReplayManager.Repositories;

public class LocalReplayRepository : ILocalReplayRepository
{
    private readonly List<ReplayDetail> _replayDetails = new();

    public Task AddIfNotExists(ReplayDetail replayDetail)
    {
        return AddIfNotExists(new List<ReplayDetail> { replayDetail });
    }

    public Task AddIfNotExists(List<ReplayDetail> replayDetails)
    {
        foreach (ReplayDetail replayDetail in replayDetails)
        {
            if (!_replayDetails.Any(x => x.FilePath?.Equals(replayDetail.FilePath) ?? false))
            {
                _replayDetails.Add(replayDetail);
            }
        }

        return Task.CompletedTask;
    }

    public Task<List<ReplayDetail>> GetAllReplays()
    {
        return Task.FromResult(_replayDetails);
    }
}

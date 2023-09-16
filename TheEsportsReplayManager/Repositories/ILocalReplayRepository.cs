using TheEsportsReplayManager.Models;

namespace TheEsportsReplayManager.Repositories;

public interface ILocalReplayRepository
{
    Task<List<ReplayDetail>> GetAllReplays();

    Task AddIfNotExists(ReplayDetail replayDetail);

    Task AddIfNotExists(List<ReplayDetail> replayDetails);
}

using TheEsportsReplayManager.Models;
using TheEsportsReplayManager.Repositories.Entities;

namespace TheEsportsReplayManager.Repositories;

public interface ILocalReplayRepository
{
    Task<List<ReplayDetail>> GetAllReplays();

    Task<ReplayDetailsEntity> AddIfNotExistsAsync(string replayFilePath);

    Task AddIfNotExists(ReplayDetail replayDetail);

    Task AddIfNotExists(List<ReplayDetail> replayDetails);

    Task<ReplayDetail?> GetByFileName(string fileName);
}

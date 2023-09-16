using TheEsportsReplayManager.Models;

namespace TheEsportsReplayManager.Services;

public interface IReplayManagementService
{
    List<ReplayState> LoadedReplays { get; }
    Task<List<ReplayDetail>> GetReplayDetails();
}

using Microsoft.Extensions.Logging;
using TheEsportsReplayManager.Models;
using TheEsportsReplayManager.Repositories;

namespace TheEsportsReplayManager.Services;

public class ReplayManagementService : IReplayManagementService
{
    private readonly ILogger<ReplayManagementService> _logger;
    private readonly ILocalFileSystemService _localFileSystemService;
    private readonly ILocalReplayRepository _localReplayRepository;
    private readonly List<ReplayState> _replays = new();

    public ReplayManagementService(
        ILogger<ReplayManagementService> logger,
        ILocalFileSystemService localFileSystemService,
        ILocalReplayRepository localReplayRepository
    )
    {
        _logger = logger;
        _localFileSystemService = localFileSystemService;
        _localReplayRepository = localReplayRepository;
    }

    public List<ReplayState> LoadedReplays => _replays.Where(x => x.Loaded).ToList();

    public async Task<List<ReplayDetail>> GetReplayDetails()
    {
        try
        {
            List<string> replayFilePaths = await _localFileSystemService.GetReplaysFromDisk();

            List<ReplayDetail> replayDetails = replayFilePaths
                .Select(replayFilePath => new ReplayDetail
                {
                    FilePath = replayFilePath,
                })
                .ToList();

            await _localReplayRepository.AddIfNotExists(replayDetails);
            List<ReplayDetail> allReplayDetails = await _localReplayRepository.GetAllReplays();

            List<ReplayState> allReplayStates = allReplayDetails
                .Select(detail =>
                {
                    ReplayState replayState = ReplayState.CreateReplayState(detail);
                    replayState.Loaded = true;
                    return replayState;
                })
                .ToList();

            foreach (ReplayState replayState in allReplayStates)
            {
                _replays.Add(replayState);
            }

            return allReplayDetails;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get replay details for an unknown reason");
            throw;
        }
    }
}

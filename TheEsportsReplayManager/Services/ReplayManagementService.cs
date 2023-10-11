//using EsportsRocketLeagueReplayParser;
//using EsportsRocketLeagueReplayParser.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using TheEsportsReplayManager.Models;
using TheEsportsReplayManager.Repositories;
using TheEsportsReplayManager.Repositories.Entities;

namespace TheEsportsReplayManager.Services;

public class ReplayManagementService : IReplayManagementService
{
    private readonly ILogger<ReplayManagementService> _logger;
    private readonly ILocalFileSystemService _localFileSystemService;
    private readonly ILocalReplayRepository _localReplayRepository;
    //private readonly IParser _parser;
    private readonly List<ReplayState> _replays = new();

    public ReplayManagementService(
        ILogger<ReplayManagementService> logger,
        ILocalFileSystemService localFileSystemService,
        ILocalReplayRepository localReplayRepository
        //IParser parser
    )
    {
        _logger = logger;
        _localFileSystemService = localFileSystemService;
        _localReplayRepository = localReplayRepository;
        //_parser = parser;
    }

    public List<ReplayState> LoadedReplays => _replays.Where(x => x.Loaded).ToList();

    public async Task<List<ReplayDetail>> GetReplayDetails()
    {
        try
        {
            List<string> replayFilePaths = await _localFileSystemService.GetReplaysFromDisk();
            List<ReplayDetailsEntity> entities = new();
            List<ReplayDetail> allReplayDetails = new();

            foreach (string replayFilePath in replayFilePaths)
            {
                try
                {
                    ReplayDetailsEntity replayDetailsEntity = await _localReplayRepository.AddIfNotExistsAsync(replayFilePath);
                    entities.Add(replayDetailsEntity);
                }
                catch (Exception ex) //Already logged
                {
                    ReplayDetail replayDetail = new()
                    {
                        FilePath = replayFilePath,
                    };

                    replayDetail.Errors.Add(new ReplayDetailError
                    {
                        Message = ex.Message,
                    });

                    allReplayDetails.Add(replayDetail);
                }
            }

            IEnumerable<ReplayDetail> replayDetailsFromEntities = entities.Select(x => new ReplayDetail
            {
                Id = x.Id.ToString(), //TODO: Should this be a string?
                FilePath = x.FilePath,
            });

            allReplayDetails.AddRange(replayDetailsFromEntities);

            return allReplayDetails;








            //List<ReplayDetail> replayDetails = new();

            //foreach (string replayFilePath in replayFilePaths)
            //{
            //    string fileName = Path.GetFileNameWithoutExtension(replayFilePath);
            //    ReplayDetail? replayDetail = await _localReplayRepository.GetByFileName(fileName);

            //    replayDetail ??= new ReplayDetail
            //    {
            //        FilePath = replayFilePath,
            //    };

            //    if (!replayDetail.IsParsed)
            //    {
            //        ParsedReplay parsedReplay = Parse(replayFilePath);
            //        replayDetail.ReplayId = parsedReplay.Id;
            //        replayDetail.ReplayName = parsedReplay.ReplayName;
            //        replayDetail.Replay = parsedReplay;
            //    }

            //    replayDetails.Add(replayDetail);
            //}

            //return replayDetails;







            //var x = Parse(replayFilePaths.FirstOrDefault()!);

            //List<ReplayDetail> replayDetails = replayFilePaths
            //    .Select(replayFilePath => new ReplayDetail
            //    {
            //        FilePath = replayFilePath,
            //    })
            //    .ToList();

            //await _localReplayRepository.AddIfNotExists(replayDetails);
            //List<ReplayDetail> allReplayDetails = await _localReplayRepository.GetAllReplays();

            //List<ReplayState> allReplayStates = allReplayDetails
            //    .Select(detail =>
            //    {
            //        ReplayState replayState = ReplayState.CreateReplayState(detail);
            //        replayState.Loaded = true;
            //        return replayState;
            //    })
            //    .ToList();

            //foreach (ReplayState replayState in allReplayStates)
            //{
            //    _replays.Add(replayState);
            //}

            //return allReplayDetails;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get replay details for an unknown reason");
            throw;
        }
    }
}

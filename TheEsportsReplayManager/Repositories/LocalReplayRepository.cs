using Microsoft.Extensions.Logging;
using SQLite;
using TheEsportsReplayManager.Data;
using TheEsportsReplayManager.Models;
using TheEsportsReplayManager.Repositories.Entities;

namespace TheEsportsReplayManager.Repositories;

public class LocalReplayRepository : ILocalReplayRepository
{
    private readonly ILogger<LocalReplayRepository> _logger;
    private readonly SQLiteAsyncConnection _conn;
    private readonly List<ReplayDetail> _replayDetails = new();

    public LocalReplayRepository(ILogger<LocalReplayRepository> logger)
    {
        _logger = logger;
        _conn = new SQLiteAsyncConnection(TERMConstants.DatabasePath, TERMConstants.SQLiteFlags);
    }

    public async Task<ReplayDetailsEntity> AddIfNotExistsAsync(string replayFilePath)
    {
        try
        {
            //TODO: Move to startup
            CreateTableResult createTableResult = await _conn.CreateTableAsync<ReplayDetailsEntity>();

            ReplayDetailsEntity? existingReplayDetailsEntity = await _conn.Table<ReplayDetailsEntity>()
                .FirstOrDefaultAsync(x => x.FilePath.Equals(replayFilePath));

            if (existingReplayDetailsEntity == null)
            {
                int count = await _conn.InsertAsync(new ReplayDetailsEntity
                {
                    FilePath = replayFilePath,
                });

                if (count != 1)
                {
                    throw new Exception($"Count was not equal to 1 when trying to insert a {nameof(ReplayDetailsEntity)} entity, actual count: {count}");
                }
            }

            ReplayDetailsEntity replayDetailsEntity = await _conn.Table<ReplayDetailsEntity>()
                .FirstAsync(x => x.FilePath.Equals(replayFilePath));

            return replayDetailsEntity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get or add replay at {ReplayFilePath}", replayFilePath);
            throw;
        }
    }

    public async Task AddIfNotExists(ReplayDetail replayDetail)
    {
        if (replayDetail.Id == null)
        {
            int addedCount = await _conn.InsertAsync(replayDetail);
        }

        //bool exists = await FileSystem.Current.AppPackageFileExistsAsync("replays.json");

        //if (!exists)
        //{
        //    File.Create("replays.json");
        //}

        //return AddIfNotExists(new List<ReplayDetail> { replayDetail });
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

    public Task<ReplayDetail?> GetByFileName(string fileName)
    {
        ReplayDetail? replayDetail = _replayDetails.FirstOrDefault(x => x.FileName?.Equals(fileName, StringComparison.OrdinalIgnoreCase) ?? false);
        return Task.FromResult(replayDetail);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheEsportsReplayManager.Services;

public interface ILocalFileSystemService
{
    Task<List<string>> GetReplaysFromDisk();
}

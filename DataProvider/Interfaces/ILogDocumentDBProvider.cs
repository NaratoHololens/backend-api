using System.Collections.Generic;
using Model.Models;
using System.Threading.Tasks;
using System;

namespace DataProvider.Interfaces
{
    public interface ILogDocumentDBProvider
    {
        List<Log> GetLogs(int maxItemCount);
        List<Log> GetLogsByUserUri(string userUri, int maxItemCount);
        List<Log> GetLogsByUserIdAndDate(string userUri, int maxItemCount);
        Task DeleteLog(string id);
        Log GetLogByID(string id);
        Task<string> CreateLog(Log log);
    }
}
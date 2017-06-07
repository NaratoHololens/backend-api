using Model.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ILogManager
    {
        List<LogDto> GetLogs();
        List<LogDto> GetLogsByUserId(string userId);
        LogCountDto GetLogStatisticsByUserId(string userId);
        LogDto GetLogByID(string id);
        Task DeleteLog(string id);
        Task<string> CreateLog(LogDto log);

    }
}

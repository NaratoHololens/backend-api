using AutoMapper;

using Common.Configurations;
using DataProvider.DataProviders;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Model.Dto;
using System.Threading.Tasks;
using Model.Models;
using Microsoft.Azure.Documents.Client;
using System.Linq;
using DataProvider.Interfaces;

namespace Domain.Managers
{
   public class LogManager : ILogManager
   {
        private readonly ILogDocumentDBProvider _logDocumentDBProvider;
        private readonly IMapper _mapper;

        public LogManager(IMapper mapper, ILogDocumentDBProvider logDocumentDBProvider)
        {
            _mapper = mapper;
            _logDocumentDBProvider = logDocumentDBProvider;
        }

        public async Task<string> CreateLog(LogDto logDto)
        {
            var log = _mapper.Map<Log>(logDto);
            return await _logDocumentDBProvider.CreateLog(log);
        }

        public async Task DeleteLog(string id)
        {
            await _logDocumentDBProvider.DeleteLog(id);
        }

        public LogDto GetLogByID(string id)
        {
            var log = _logDocumentDBProvider.GetLogByID(id);

            return _mapper.Map<LogDto>(log);
        }

        public List<LogDto> GetLogs()
        {
            try
            {
                var logs = _logDocumentDBProvider.GetLogs(10);
                return _mapper.Map<List<LogDto>>(logs);
            }
            catch (Exception)
            {

                throw;
            }

            
        }

        public List<LogDto> GetLogsByUserId(string userId)
        {
          var logs = _logDocumentDBProvider.GetLogsByUserUri(userId,10);
          return _mapper.Map<List<LogDto>>(logs);
        }

        public LogCountDto GetLogStatisticsByUserId(string userId)
        {
            List<int> statistics = new List<int>();
            List<DateTime> timespan = new List<DateTime>();
            var logs = _logDocumentDBProvider.GetLogsByUserIdAndDate(userId, 10);

            for (int i = 6; i >= 0; i--)
            {
                DateTime current = DateTime.Now;
                current = current.AddDays(-i);
                timespan.Add(current);
                int  test = logs.FindAll(l => l.Timestamp.Date == current.Date).Count;
               statistics.Add(test);
        
            }
            LogCountDto logCountDto = new LogCountDto();
            logCountDto.Statistics = statistics;
            logCountDto.Timespan = timespan;

            return logCountDto;
        }
    }
}

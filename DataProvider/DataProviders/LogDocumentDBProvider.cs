using DataProvider.Interfaces;
using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents.Client;
using System.Linq;
using System.Threading.Tasks;
using Common.Exceptions;
using Model.Models;

namespace DataProvider.DataProviders
{
    public class LogDocumentDBProvider : ILogDocumentDBProvider
    {


        private readonly IStorageProvider _genericDocumentDBProvider;

        public LogDocumentDBProvider(IStorageProvider genericDocumentDBProvider)
        {
            _genericDocumentDBProvider = genericDocumentDBProvider;
        }


        public List<Log> GetLogs(int maxItemCount)
        {
            var feedOptions = new FeedOptions() { MaxItemCount = maxItemCount };
            try
            {
                return _genericDocumentDBProvider.CreateQuery<Log>(feedOptions).Where(u => u.Type == "LOG").ToList();
            }
            catch (AggregateException AE)
            {
                throw new CollectionException("The collection doesn't exist");
            }
        }

        public List<Log> GetLogsByUserUri(string userId, int maxItemCount )
        {
            var feedOptions = new FeedOptions() { MaxItemCount = maxItemCount };
            try
            {
                return _genericDocumentDBProvider.CreateQuery<Log>(feedOptions).Where(l => l.UserID == userId).ToList();
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public async Task DeleteLog(string id)
        {
            await _genericDocumentDBProvider.DeleteDocument(id);
        }

        public Log GetLogByID(string id)
        {
            var feedOptions = new FeedOptions() { MaxItemCount = 1 };
            return _genericDocumentDBProvider.CreateQuery<Log>(feedOptions).Where(l => l.ID == id).AsEnumerable().First();
        }

        public async Task<string> CreateLog(Log log)
        {
            return await _genericDocumentDBProvider.AddDocument<Log>(log);
        }

        public List<Log> GetLogsByUserIdAndDate(string userId, int maxItemCount)
        {
            var feedOptions = new FeedOptions() { MaxItemCount = maxItemCount };
            return _genericDocumentDBProvider.CreateQuery<Log>(feedOptions).Where(l => l.UserID == userId && l.Type == "LOG").ToList();
        }
    }
}

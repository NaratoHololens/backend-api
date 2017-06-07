using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;

namespace DataProvider.Interfaces
{
    public interface IStorageProvider
    {
        Task<string> AddDocument<T>(T document);
        IQueryable<T> CreateQuery<T>(FeedOptions feedOptions);
        IQueryable<T> CreateQuery<T>(string sql, FeedOptions feedOptions);
        Task DeleteDocument(string id);
        Task<bool> UpdateDocument<T>(T document, string id);
    }
}
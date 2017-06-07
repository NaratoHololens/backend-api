using Common.Exceptions;
using DataProvider.Interfaces;
using Microsoft.Azure.Documents.Client;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataProvider.DataProviders
{
    public class UserDocumentDBProvider : IUserDocumentDBProvider
    {

        private readonly IStorageProvider _genericDocumentDBProvider;

        public UserDocumentDBProvider(IStorageProvider genericDocumentDBProvider)
        {_genericDocumentDBProvider = genericDocumentDBProvider;
        }


        public List<User> GetUsers(int maxItemCount)
        {
            var feedOptions = new FeedOptions() { MaxItemCount = maxItemCount };
            try
            {
                return _genericDocumentDBProvider.CreateQuery<User>(feedOptions).Where(u => u.Type == "USER").ToList();
            }
            catch (AggregateException AE)
            {
                //logging error
                throw new CollectionException("The collection user doesn't exist");
            }
        }

        public User getUserbyId(string id)
        {
            var feedOptions = new FeedOptions() { MaxItemCount = 1 };

            try
            {
                return _genericDocumentDBProvider.CreateQuery<User>(feedOptions).Where(u => u.Id == id).AsEnumerable().First();
                
            }
            catch (AggregateException AE)
            {
                throw new CollectionException("The collection user doesn't exist");
            }
            catch (InvalidOperationException IE)
            {
                throw new DocumentException("user with id " + id + " Doesn't exist");
            }
        }

        public async Task DeleteUser(string id)
        {
            await _genericDocumentDBProvider.DeleteDocument(id);
        }

        public async Task<bool> UpdateUser(User u)
        {
            return await _genericDocumentDBProvider.UpdateDocument<User>(u, u.Id);
        }

        public async Task<string> CreateUser(User user)
        {
            return await _genericDocumentDBProvider.AddDocument<User>(user);
        }

    }
}

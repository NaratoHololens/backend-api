using System.Collections.Generic;
using Model.Models;
using System.Threading.Tasks;

namespace DataProvider.Interfaces
{
    public interface IUserDocumentDBProvider
    {
        List<User> GetUsers(int maxItemCount);
        Task DeleteUser(string id);
        User getUserbyId(string id);
        Task<bool> UpdateUser(User u);
        Task<string> CreateUser(User user);
    }
}
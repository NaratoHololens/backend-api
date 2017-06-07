using Model.Dto;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Model.Models;

namespace Domain.Interfaces
{
    public interface IUserManager
    {
        List<UserDto> GetUsers();
        UserDto GetUserByID(string id);
        Task DeleteUser(string id);
        Task<bool> UpdateUser(UserDto u);
        Task<string> CreateUser(UserDto userDto);
        Task registerFace(UserDto userDto);
        Task<PersonLookupDto> findPersons(UserImageDto user);
        Task<PersonLookupDto> findPersons(UserImageDto user, bool trimJSImageString = false);
        
        }
}

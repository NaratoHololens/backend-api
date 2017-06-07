using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Model.Dto;

namespace DataProvider.Interfaces
{
   public interface ICognitiveFaceProvider
    {
        Task RegisterFace(string personId, string photostring);
        Task<DetectedPersonDto> getPersonFromCognitiveService( UserDto userDto);
        Task deleteUser(string id);
        Task<List<DetectedPersonDto>> detectPerson(Byte[] image);
        Task<List<DetectedPersonDto>> identifyPersons(List<string> faceids);
    }
}

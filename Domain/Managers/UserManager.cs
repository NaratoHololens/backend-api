
using System;
using System.Collections.Generic;
using Domain.Interfaces;
using AutoMapper;
using Model.Dto;
using System.Threading.Tasks;
using System.Linq;
using DataProvider.Interfaces;
using Common.Exceptions;


namespace Domain.Managers
{
    public class UserManager : IUserManager
    {
        private readonly IUserDocumentDBProvider _userDocumentDBprovider;
        private readonly ICognitiveFaceProvider _cognitiveFaceProvider;
        private readonly ISocialMediaProvider _socialMediaProvider;
        private readonly IMapper _mapper;

        public UserManager(IMapper mapper, IUserDocumentDBProvider userDocumentDBprovider, ICognitiveFaceProvider cognitiveFaceProvider, ISocialMediaProvider socialMediaProvider)
        {
            _mapper = mapper;
            _cognitiveFaceProvider = cognitiveFaceProvider;
            _userDocumentDBprovider = userDocumentDBprovider;
            _socialMediaProvider = socialMediaProvider;
        }

        public List<UserDto> GetUsers()
        {
            try
            {
                var users = _userDocumentDBprovider.GetUsers(10);
                return _mapper.Map<List<UserDto>>(users);
            }
            catch (AggregateException AE)
            {
                throw;
            }
            
        }

        public UserDto GetUserByID(string id)
        {
            try
            {
                var user = _userDocumentDBprovider.getUserbyId(id);
                return _mapper.Map<UserDto>(user);
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        
        public async Task DeleteUser(string id)
        {
            await _cognitiveFaceProvider.deleteUser(id);
            await _userDocumentDBprovider.DeleteUser(id);
        }


       
        public async Task<bool> UpdateUser(UserDto u)
        {
            var user = _mapper.Map<Model.Models.User>(u);
            if (u.PhotoString != null && !u.PhotoString.Equals(""))
              await  registerFace(u);

            return await _userDocumentDBprovider.UpdateUser(user);
        }


       public async Task<string> CreateUser(UserDto userDto)
       {
            DetectedPersonDto detectedPersonDto = await _cognitiveFaceProvider.getPersonFromCognitiveService(userDto);
            userDto.Id = detectedPersonDto.personId;
            await registerFace(userDto);
            var user = _mapper.Map<Model.Models.User>(userDto);

            return await _userDocumentDBprovider.CreateUser(user);
       }

        public async Task registerFace(UserDto userDto)
        {
            await _cognitiveFaceProvider.RegisterFace(userDto.Id, userDto.PhotoString);
        }

        public async Task<PersonLookupDto> findPersons(UserImageDto userImage)
        {
            Byte[] image = Convert.FromBase64String(userImage.imageString);

            try
            {
                List<DetectedPersonDto> detectedPersonDtos = await _cognitiveFaceProvider.detectPerson(image);

                return new PersonLookupDto() { users = await Identifypersons(detectedPersonDtos) };
            }
            catch (Exception e)
            {

                throw;
            }
            
        }

        public async Task<PersonLookupDto> findPersons(UserImageDto userImage, bool trimJSImageString = false)
        {
            if (trimJSImageString)
            {
                userImage.imageString = userImage.imageString.Split(',')[1];
            }

            return await findPersons(userImage);
        }

        private async Task<List<UserDto>> Identifypersons(List<DetectedPersonDto> facePersonDtoList)
        {
            List<string> faceids = new List<string>();

            foreach (var item in facePersonDtoList)
            {
                faceids.Add(item.faceId);
            }

             var resultListDetectedUsers =  await _cognitiveFaceProvider.identifyPersons(faceids);

            if (resultListDetectedUsers.First().candidates.Count() == 0)
                throw new CollectionException("No person detected");


            List<UserDto> users = new List<UserDto>();
                foreach (var item in resultListDetectedUsers)
                {
                    TweetDto noTweetFound = new TweetDto("No tweets found");
                    UserDto userDto = _mapper.Map<UserDto>(_userDocumentDBprovider.getUserbyId((item.candidates.First().personId)));
                    userDto.faceRectangle = item.faceRectangle;
                    List<TweetDto> tweets = new List<TweetDto>();
                    if (userDto.Twittername != null)
                    {
                        List<string> tweetTemps = new List<string>();
                        tweetTemps = _socialMediaProvider.GetMessages(userDto.Twittername);

                        foreach (var t in tweetTemps)
                        {
                            TweetDto tweet = new TweetDto(t);
                            tweets.Add(tweet);
                        }

                        if (tweets.Count == 0)
                            tweets.Add(noTweetFound);
                    }
                    else
                    {
                        tweets.Add(noTweetFound);
                    }


                    userDto.tweets = tweets;
                    users.Add(userDto);
                }

                return users;

        }

        
    }
}

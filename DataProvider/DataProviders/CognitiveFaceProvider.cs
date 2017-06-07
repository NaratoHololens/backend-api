using Common.Configurations;
using Common.Exceptions;
using DataProvider.Interfaces;
using Microsoft.Extensions.Options;
using Model.Dto;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DataProvider.DataProviders
{
    public class CognitiveFaceProvider : ICognitiveFaceProvider
    {
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly CognitiveServiceConfiguration _cognitiveServiceConfiguration;

        public CognitiveFaceProvider(IOptions<CognitiveServiceConfiguration> cognitiveServiceConfiguration)
        {
            _cognitiveServiceConfiguration = cognitiveServiceConfiguration.Value;
        }

        public async Task<DetectedPersonDto> getPersonFromCognitiveService(UserDto userDto)
        {
            var data = new
            {
                name = userDto.Firstname + " " + userDto.Lastname
            };
            string dataAsJsonString = JsonConvert.SerializeObject(data);

            DetectedPersonDto detectedPersonDto = await GetObjectFromCognitiveService<DetectedPersonDto>(
                "/persongroups/registeredusers/persons",
                "application/json",
                new StringContent(dataAsJsonString),
                true);

            return detectedPersonDto;
        }

        public async Task RegisterFace(string personId, string photostring)
        {


            string route = "/persongroups/registeredusers/persons/" + personId + "/persistedFaces";

            byte[] image = null;
            try
            {
                string trimmedPhotoString = photostring.Split(',')[1];
                image = Convert.FromBase64String(trimmedPhotoString);
            }
            catch (Exception e)
            {
                logger.Error("the picture couldn't be converted into a string " + e.Message);
                throw;
            }

            try
            {
                await GetObjectFromCognitiveService<DetectedPersonDto>(
                   route,
                   "application/octet-stream",
                   new ByteArrayContent(image),
                   false);
            }
            catch (Exception)
            {

                throw;
            }

            //train person 
            await SendSimpleRequest("persongroups/" + _cognitiveServiceConfiguration.PersonGroup + "/train", System.Net.Http.HttpMethod.Post);

        }

        public async Task<List<DetectedPersonDto>> detectPerson(Byte[] image)
        {
            try
            {
                List<DetectedPersonDto> FacePersonDtoList =
                await GetObjectFromCognitiveService<List<DetectedPersonDto>>(
                   "detect?returnFaceId=true&returnFaceLandmarks=false",
                   "application/octet-stream",
                   new ByteArrayContent(image),
                   false);

                return FacePersonDtoList;

            }
            catch (Exception ex)
            {
                throw new CollectionException("No Person was found");
            }
        }

        public async Task<List<DetectedPersonDto>> identifyPersons(List<string> faceids)
        {
            object data = new
            {
                personGroupId = _cognitiveServiceConfiguration.PersonGroup,
                faceIds = faceids,
                maxNumOfCandidatesReturned = 4
            };

            string dataAsJsonString = JsonConvert.SerializeObject(data);

            return
              await GetObjectFromCognitiveService<List<DetectedPersonDto>>(
                 "identify",
                 "application/json",
                 new StringContent(dataAsJsonString),
                 true);
        }

        public async Task deleteUser(string id)
        {
            await SendSimpleRequest("persongroups/" + _cognitiveServiceConfiguration.PersonGroup + "/persons/" + id, HttpMethod.Delete);
        }

        private async Task<T> GetObjectFromCognitiveService<T>(string route, string contentType, HttpContent content, bool hostNeeded)
        {

            HttpResponseMessage response;
            string responseContentAsString;


            using (HttpClient http = new HttpClient())
            {

                http.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _cognitiveServiceConfiguration.Subkey);

                if (hostNeeded)
                    http.DefaultRequestHeaders.Add("Host", "westeurope.api.cognitive.microsoft.com");

                using (content)
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                    response = await http.PostAsync(_cognitiveServiceConfiguration.Uri + route, content);
                    if (response.IsSuccessStatusCode)
                    {
                        responseContentAsString = response.Content.ReadAsStringAsync().Result;
                        return JsonConvert.DeserializeObject<T>(responseContentAsString);
                    }
                    else
                    {
                        throw new Exception(response.StatusCode + " " + response.ReasonPhrase);
                    }

                }

            }
        }


        private async Task SendSimpleRequest(string uri, HttpMethod method)
        {
            using (HttpClient http = new HttpClient())
            {
                HttpRequestMessage httpRequest = new HttpRequestMessage(method, _cognitiveServiceConfiguration.Uri + uri);
                httpRequest.Headers.Add("Host", "westeurope.api.cognitive.microsoft.com");
                httpRequest.Headers.Add("Ocp-Apim-Subscription-Key", _cognitiveServiceConfiguration.Subkey);

                await http.SendAsync(httpRequest);

               
            }
        }

    }
}


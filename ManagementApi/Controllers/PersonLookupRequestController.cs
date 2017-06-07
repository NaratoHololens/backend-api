using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Narato.Common.Factory;
using Domain.Interfaces;
using Model.Dto;
using Microsoft.AspNetCore.Cors;
using NLog;
using Microsoft.ApplicationInsights;
using Narato.Common;
using Microsoft.AspNetCore.Authorization;

namespace ManagementApi.Controllers
{
    [Route("api/personLookupRequests")]
    [EnableCors("EnableCors")]

    public class PersonLookupRequestController : Controller
    {

        private readonly IResponseFactory _responseFactory;
        private readonly IUserManager _userManager;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private TelemetryClient _telemetry = new TelemetryClient();

        public PersonLookupRequestController(IUserManager userManager, IResponseFactory responseFactory)
        {
            _responseFactory = responseFactory;
            _userManager = userManager;
        }

        [HttpPost("hololensPersonLookup")]
        public async Task<IActionResult> DetectPersonOnPicture(UserImageDto userImage)
        {
            //terug verplaatsen naar userController en data zelf deserializen
                return await _responseFactory.CreatePostResponseAsync(async () => await _userManager.findPersons(userImage), this.GetRequestUri());
      
        }


        [HttpPost("personLookup")]
        public async Task<IActionResult> DetectPersonOnPicture([FromBody]UserImageDto userImage, bool trimJSImageString = false)
        {
            return await _responseFactory.CreatePostResponseAsync(async () => await _userManager.findPersons(userImage, trimJSImageString), this.GetRequestUri());

        }

    }
}

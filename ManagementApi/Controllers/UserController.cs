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
    [Route("api/users")]
    [EnableCors("EnableCors")]
    public class UserController : Controller
    {

        private readonly IResponseFactory _responseFactory;
        private readonly IUserManager _userManager;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private TelemetryClient _telemetry = new TelemetryClient();

        public UserController(IUserManager userManager, IResponseFactory responseFactory)
        {
            _responseFactory = responseFactory;
            _userManager = userManager;
        }

        /// <summary>
        /// returns all the users.
        /// </summary>
        ///  <response code="200">succes</response>
        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            return _responseFactory.CreateGetResponse(() => _userManager.GetUsers(), this.GetRequestUri());

        }
        /// <summary>
        /// Gets a specific user.
        /// </summary>
        /// <param name="id"></param>
        ///  <response code="200">if a user is found successfully</response>
        [HttpGet("{id}")]
        [Authorize]
        public IActionResult Get(string id)
        {
            return _responseFactory.CreateGetResponse(() => _userManager.GetUserByID(id), this.GetRequestUri());
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="userDto"></param>
        ///  <response code="204">if a user is deleted successfully</response>
        /// <response code="401">if not authorized to delete a user</response>
        /// <response code="404">user with this id doesn't exist</response>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]UserDto userDto)
        {
            return await _responseFactory.CreatePostResponseAsync(async () => await _userManager.CreateUser(userDto), this.GetRequestUri());
        }

        /// <summary>
        /// Updates a  user.
        /// </summary>
        /// <param name="userDto"></param>
        ///  <response code="204">if a user is deleted successfully</response>
        /// <response code="401">if not authorized to delete a user</response>
        /// <response code="404">user with this id doesn't exist</response>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UserDto userDto)
        {
            return await _responseFactory.CreatePutResponseAsync(async () => await _userManager.UpdateUser(userDto), this.GetRequestUri());
        }

        /// <summary>
        /// Deletes a specific user.
        /// </summary>
        /// <param name="id"></param>
        ///  <response code="204">if a user is deleted successfully</response>
        /// <response code="401">if not authorized to delete a user</response>
        /// <response code="404">user with this id doesn't exist</response>
        [HttpDelete("{id}")]
        //  [Authorize]
        public async Task<IActionResult> DeletePerson(string id)
        {

            return await _responseFactory.CreateDeleteResponseAsync(async () => await _userManager.DeleteUser(id), this.GetRequestUri());

        }

    }
}

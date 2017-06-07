using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Narato.Common.Factory;
using Domain.Interfaces;
using Model.Dto;
using Narato.Common;
using Microsoft.AspNetCore.Cors;

namespace ManagementApi.Controllers
{
    [EnableCors("EnableCors")]
    [Route("api/logs")]
    public class LogController : Controller
    {
        private readonly IResponseFactory _responseFactory;
        private readonly ILogManager _logManager;

        public LogController(ILogManager logManager, IResponseFactory responseFactory)
        {
            _responseFactory = responseFactory;
            _logManager = logManager;
        }
        /// <summary>
        /// Creates a new log.
        /// </summary>
        /// <param name="logDto"></param>
        ///  <response code="204">if a user is created successfully</response>
        [HttpPost]
        public async Task<IActionResult> Create(LogDto logDto)
        {
                       
        return await _responseFactory.CreatePostResponseAsync(async () =>  await _logManager.CreateLog(logDto), this.GetRequestUri());
            

        }

        /// <summary>
        /// returns all the logs.
        /// </summary>
        ///  <response code="200">succes</response>
        [HttpGet]
        public IActionResult Get()
        {
            return _responseFactory.CreateGetResponse(() => _logManager.GetLogs(), this.GetRequestUri());
        }

        /// <summary>
        /// Get all logs of a specific user.
        /// </summary>
        /// <param name="id"></param>
        ///  <response code="200">if a userId has logs</response>
        [HttpGet]
        [Route("{id}")]
        public  IActionResult Get(string id)
        {
            return _responseFactory.CreateGetResponse( () =>  _logManager.GetLogsByUserId(id), this.GetRequestUri());
        }
        /// <summary>
        /// Get teh statistics of a User.
        /// </summary>
        /// <param name="id"></param>
        ///  <response code="200">succes</response>
        [HttpGet]
        [Route("{id}/stats")]
        public IActionResult GetStatistics(string id)
        {
            return _responseFactory.CreateGetResponse(() => _logManager.GetLogStatisticsByUserId(id), this.GetRequestUri());
        }

    }
}

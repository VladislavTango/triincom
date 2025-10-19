using Microsoft.AspNetCore.Mvc;
using triincom.Core.DTO;
using triincom.Core.Interface;
using triincom.Middlewares;

namespace triincom.Controllers
{
    [Route("api/")]
    [ApiController]
    [ResponseFilter]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public ApplicationController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet("AllFiltered")]
        public IActionResult GetFilteredApplications([FromQuery] ApplicationFilterDto filter)
        {
            var responce = _applicationService.GetFilteredApplicationsAsync(filter);
            return Ok(responce);
        }
        [HttpGet("All")]
        public IActionResult GetApplications()
        {
            var responce = _applicationService.GetAllApplicationsAsync();
            return Ok(responce);
        }
        [HttpPut]
        public IActionResult ChangeStatus([FromBody] ChangeStatusDto changeStatusDto)
        {
            var responce = _applicationService.ChangeApplicationStatus(changeStatusDto);
            return Ok(responce);
        }
        [HttpPost("loans")]
        public IActionResult AddContract([FromBody] AddApplicationDto applicationDto)
        {
            var responce = _applicationService.AddAplication(applicationDto);
            return Ok(responce);
        }
    }
}

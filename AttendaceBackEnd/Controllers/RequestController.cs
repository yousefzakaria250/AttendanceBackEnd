using Infrastructure;
using Infrastructure.Constants;
using Infrastructure.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AttendaceBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequestRepo requestRepo;

        public RequestController(IRequestRepo requestRepo)
        {
            this.requestRepo = requestRepo;
        }

        [HttpPost("AddRequest")]
        public async Task<IActionResult> Add( RequestDto requestDto)
        {
            var res = await requestRepo.Add(requestDto);
            return Ok(res);
        }
        ///
        [HttpGet("GetAllRequestsSupervisior")]
        [Authorize(Roles = "DM")]
        public async Task<IActionResult> GetAll(int deptNo)
        {
            var res = await requestRepo.GetAllRequestOfSupervisior(deptNo);
            return Ok(res);
        }


        [HttpPatch("ChangeRequestStateOfSupervisior")]
        [Authorize(Roles = "DM")]
        public async Task<IActionResult> UpdateState( int Id , int State)
        {
            var res = await requestRepo.ChangeRequestStateBySupervisior(Id, State);
            if (res == null)
                return NotFound("This Request Not Found .");
            return Ok(res); 
        }

        [HttpGet("GetAllRequestOfGm")]
        [Authorize(Roles = "GM")]
        public async Task<IActionResult> GetAllOfGM(int deptNo)
        {
            var res = await requestRepo.GetAllRequestOfGM(deptNo);
            return Ok(res);
        }


        [HttpPatch("ChangeRequestStateByGM")]
        [Authorize(Roles = "GM")]
        public async Task<IActionResult> UpdateStateByGm(int Id, int State)
        {
            var res = await requestRepo.ChangeRequestStateByGM(Id, State);
            if(res== null)
                return NotFound("This Request Not Found .");
            return Ok(res);
        }

        [HttpDelete("DeleteRequest")]
        public async Task<IActionResult> DeleteRequest(int Id)
        {
            var res = await requestRepo.Delete(Id);
            return Ok(res);
        }

    }
}

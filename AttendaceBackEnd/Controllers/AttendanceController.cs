using Data;
using Infrastructure.Repositories.Attendance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AttendaceBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceRepo attendanceRepo;
        private readonly AttendanceContext attendanceContext;

     

        public AttendanceController(IAttendanceRepo attendanceRepo , AttendanceContext attendanceContext)
        {
            this.attendanceRepo = attendanceRepo;
            this.attendanceContext = attendanceContext;
        }

        [HttpPost("AddExcelSheet")]

        public async Task<IActionResult> Add(IFormFile file)
        {
            var res = await attendanceRepo.Add(file);
            return Ok(res);
        }

        [HttpGet("GetAllAttendance")]
        public IActionResult GetAll(DateTime date)
        {
            var res = attendanceContext.GetAllAttendance(date);
            return Ok(res);
        }

    }
}

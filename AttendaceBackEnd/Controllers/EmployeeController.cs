using Infrastructure;
using Infrastructure.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AttendaceBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        private readonly IEmployeeRepo employeeRepo;

        public EmployeeController(IEmployeeRepo employeeRepo)
        {
            this.employeeRepo = employeeRepo;
        }

        [HttpPost("AddEmployee")]
        [Authorize]
        public async Task<IActionResult> Add(EmployeeDto dto , bool flag)
        { 
            var Emp = await employeeRepo.Add(dto , flag);
            return Ok(Emp);
        }

        [HttpGet("GetAllEmployee")]
        public async Task<IActionResult> GetAllEmployee()
        {
            var emp = await employeeRepo.GetAll();
            return Ok(emp);
        }
        [HttpGet("GetAllSupervisior")]
        public async Task<IActionResult> GetAllSupervisior()
        {
            var emp = await employeeRepo.GetAllSupervisior();
            return Ok(emp);
        }
    }
}

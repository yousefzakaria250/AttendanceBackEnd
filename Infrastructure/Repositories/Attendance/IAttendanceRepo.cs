using Data.Attendance;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Attendance
{
    public interface IAttendanceRepo
    {
        public Task<dynamic> Add(IFormFile file);
        //public Task<List<Attendances>> GetAll();
    }
}

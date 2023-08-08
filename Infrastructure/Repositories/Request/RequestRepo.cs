using Data;
using Infrastructure.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class RequestRepo : IRequestRepo
    {

        private readonly AttendanceContext attendanceContext;

        public RequestRepo(AttendanceContext attendanceContext)
        {
            this.attendanceContext = attendanceContext;
        }

        public async Task<Request> Add(RequestDto dto)
        {
            var request = new Request()
            {
                 From = dto.From ,
                 To = dto.To ,
                 Reason = dto.Reason ,
                 State = 0,
                 DepartmentId = dto.DeptId ,
                 EmployeeId  = dto.EmployeeId 
            };
            await attendanceContext.AddAsync(request);
            attendanceContext.SaveChanges();
            return request;
        }

        public async Task<List<Request>> GetAllRequestOfSupervisior(int departmentNo)
        {
            var requests = await attendanceContext.Request
                .Where(w => w.DepartmentId == departmentNo && w.State == 0)
                .ToListAsync();
            return requests;
        }

        public async Task<Request> ChangeRequestStateBySupervisior(int Id, int State)
        {
            var request = await Get(Id);
            if (request == null)
                return request;
            if (State != 1)
            {
                request.State = -1;
                attendanceContext.Entry(request).State = EntityState.Modified;
                attendanceContext.SaveChanges();
            }
           
            request.State = 1;
            attendanceContext.Entry(request).State = EntityState.Modified;
            attendanceContext.SaveChanges();
            return request; 
            
        }

        public async Task<Request> Get(int Id) 
        {
            var request = await attendanceContext.Request.Include(e=>e.Employee).FirstOrDefaultAsync(f => f.Id == Id);
            return request;
        }

        public async Task<Request> ChangeRequestStateByGM(int Id, int State)
        {
            var request = await Get(Id);
            if (request == null)
                return request;
            if (State != 2)
            {
                request.State = -2;
                attendanceContext.Entry(request).State = EntityState.Modified;
                attendanceContext.SaveChanges();
                return request;
            }
            request.State = 2;
            request.Employee.Balance -= 1;
            attendanceContext.Entry(request).State = EntityState.Modified;
            attendanceContext.SaveChanges();
            return request;
        }

        public async Task<List<Request>> GetAllRequestOfGM(int departmentNo = 0)
        {
            if (departmentNo == 0)
            {
                var req = await attendanceContext.Request.Where(w => w.State == 1).ToListAsync();
                return req;
            }
            var requests = await attendanceContext.Request
              .Where(w => w.DepartmentId == departmentNo && w.State == 1)
              .ToListAsync();
            return requests;
    }

        public async Task<Request> Delete(int Id)
        {
            var request = await Get(Id);
            attendanceContext.Entry(request).State = EntityState.Deleted;
            attendanceContext.SaveChanges();
            return request;

        }
    }
}

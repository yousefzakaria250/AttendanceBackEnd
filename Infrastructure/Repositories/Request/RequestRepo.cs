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

        public async Task<Request> Add(RequestDto dto, string UserId)
        {
            var request = new Request()
            {
                 From = dto.From ,
                 To = dto.To ,
                 Reason = dto.Reason ,
                 State = 0,
                 DepartmentId = dto.DeptId ,
                 EmployeeId  = UserId
            };
            await attendanceContext.AddAsync(request);
            attendanceContext.SaveChanges();
            return request;
        }

        public async Task<dynamic> GetAllRequestOfSupervisior(string userId)
        {
            var requests = (from p in attendanceContext.Request
                          join e in attendanceContext.Employee
                          on p.EmployeeId equals e.Id
                          where e.SupervisiorId == userId && p.State == 0
                          select new
                          {
                              Id = p.Id,
                              Fname = e.Fname,
                              Lname = e.Lname,
                              State = p.State,
                              Reason = p.Reason,
                              From = p.From,
                              To = p.To
                          }).ToList();


            return requests;
            ////////////////////////////////////////////////////////////
            //var requests = await attendanceContext.Request
            //    .Join(  attendanceContext.Employee,
            //          p => p.EmployeeId,
            //          e => e.Id,

            //          (p, e) => new {
            //              Id = p.Id ,
            //              Fname = e.Fname ,
            //              Lname = e.Lname ,
            //              State = p.State ,
            //              Reason = p.Reason ,
            //              From = p.From ,
            //              To = p.To
            //          }
            //          )
            //    .Where(w=> w.State == 0)
            //    .ToListAsync();
            //return requests;
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

        public async Task<List<Request>> GetAllRequestOfGM(DateTime date , int departmentNo = 0)
        {
            if (departmentNo == 0)
            {
                var req = await attendanceContext.Request.Where(w => w.State == 1 && (w.From == date || w.To == date) ).ToListAsync();
                return req;
            }
            var requests = await attendanceContext.Request
              .Where(w => w.DepartmentId == departmentNo && w.State == 1 && (w.From == date || w.To == date))
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

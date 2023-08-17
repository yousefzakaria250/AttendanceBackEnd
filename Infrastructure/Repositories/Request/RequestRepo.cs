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
            var SupervisiorRequest = await attendanceContext.Employee.Where(e => (e.Id == UserId && e.SupervisiorId == null)).FirstOrDefaultAsync();
            var request = new Request()
            {
                 From = dto.From ,
                 To = dto.To ,
                 Reason = dto.Reason ,
                 State =  (SupervisiorRequest == null) ? 0 : 1,
                 DepartmentId = dto.DeptId ,
                 EmployeeId  = UserId
            };

            await attendanceContext.AddAsync(request);
            attendanceContext.SaveChanges();
            return request;
        }

        public async Task<dynamic> GetAllRequestOfSupervisior(string userId)
        {
            var requests =await  (from p in attendanceContext.Request
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
                          }).ToListAsync();


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
                return request;
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

        public async Task<dynamic> GetAllRequestOfGM(int departmentNo)
        {

            var requests = 
                (departmentNo == 0) ? await (from p in attendanceContext.Request
                                                        join e in attendanceContext.Employee
                                                        on p.EmployeeId equals e.Id
                                                        where p.State == 1
                                                        select new
                                                        {
                                                            Id = p.Id,
                                                            Fname = e.Fname,
                                                            Lname = e.Lname,
                                                            State = p.State,
                                                            Reason = p.Reason,
                                                            From = p.From,
                                                            To = p.To
                                                        }).ToListAsync() : await (from p in attendanceContext.Request
                                                                                  join e in attendanceContext.Employee
                                                                                  on p.EmployeeId equals e.Id
                                                                                  where p.State == 1 && p.DepartmentId == departmentNo
                                                                                  select new
                                                                                  {
                                                                                      Id = p.Id,
                                                                                      Fname = e.Fname,
                                                                                      Lname = e.Lname,
                                                                                      State = p.State,
                                                                                      Reason = p.Reason,
                                                                                      From = p.From,
                                                                                      To = p.To
                                                                                  }).ToListAsync();

            return requests;

            //if (departmentNo == 0) { 
            //    //{
            //    //    var req =  (date == null)?(await attendanceContext.Request.Where(w => w.State == 1).ToListAsync()) :
            //    //        await attendanceContext.Request.Where(w => w.State == 1 && (w.From == date || w.To == date) ).ToListAsync();
            //    var req = await attendanceContext.Request.Where(w => w.State == 1).ToListAsync();

            //    return req;
            //}
            //var requests = await attendanceContext.Request
            //  .Where(w => w.DepartmentId == departmentNo && w.State == 1 )
            //  .ToListAsync();
            //return requests;
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

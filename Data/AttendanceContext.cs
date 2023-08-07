using Data.Attendance;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class AttendanceContext : DbContext
    {
        public AttendanceContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Employee> Employee { set; get; }
        public DbSet<Department> Department { set; get; }
        public DbSet<Request> Request { set; get; }
        public DbSet<Attendances> Attendances { set; get; }
        public DbSet<ProcAttendance> ProcAttendances { set; get; }

        public List<ProcAttendance> GetAllAttendance(DateTime date)
        {
            var dateParameter = new SqlParameter("@date", date);

            return this.ProcAttendances
                .FromSqlRaw("EXECUTE GetEmployeeByDate @date", dateParameter)
                .ToList();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                               .HasOne(c => c.Supervisior)
                               .WithMany()
                               .HasForeignKey(c => c.SupervisiorId);
        }
    }
}

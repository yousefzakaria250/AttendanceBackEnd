using Data;
using Data.Attendance;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Attendance
{
    public class AttendanceRepo : IAttendanceRepo
    {
        private readonly AttendanceContext context;
        public AttendanceRepo(AttendanceContext context)
        {
            this.context = context;
        }

        public async Task<dynamic> Add(IFormFile formFile)
        {
            if (formFile == null || formFile.Length <= 0)
            {
                return new
                {
                    IsError = true,
                    Message = "الملف فارغ",
                    Code = 400
                };
            }
            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return new
                {
                    IsError = true,
                    Message = "الرجاء إختيار صيغة إكسيل صحيحة",

                    Code = 400
                };// BadRequest("Not support File Extension");
            }
            try
            {
                var list = new List<Attendances>();
                using (var stream = new MemoryStream())
                {
                    formFile.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;
                        for (int row = 2; row <= rowCount; row++)
                        {
                            var Attendance = new Attendances();
                            Attendance.Code = worksheet.Cells[row, 2].Value?.ToString().Trim();
                            Attendance.Name = worksheet.Cells[row, 3].Value?.ToString().Trim();
                           // double value = 45809.39583;
                            DateTime date = DateTime.FromOADate((Double)worksheet.Cells[row, 4].Value);
                            //Console.WriteLine(date.ToString("dd MMMM yyyy h:mm tt"));
                            Attendance.Date = date ;
                            Attendance.Day = worksheet.Cells[row, 5].Value?.ToString().Trim();
                            double cIn1 = (Double)worksheet.Cells[row, 6].Value;
                            Attendance.CheckIn = ConvertDecimalTimeSpan(cIn1);
                            double cIn2 = (Double)worksheet.Cells[row, 7].Value;
                            Attendance.CheckOut = ConvertDecimalTimeSpan(cIn2);
                            list.Add(Attendance);
                        }
                    }
                }
                await context.AddRangeAsync(list);
                context.SaveChanges();
                return new
                {
                    IsError = false,
                    Message = "تمت الاضافة",
                    Data = true,
                    Code = 200
                };
            }
            catch (Exception ex)
            {
                // insert ex exception in databse Error Log Table
                return new 
                {
                    IsError = true,
                    Message = ex.Message,

                    Code = 500
                };
            }


        }

        //public Task<List<Attendances>> GetAll()
        //{
        //    TimeSpan StartTime = new TimeSpan(9, 0, 0);
        //    TimeSpan EndTime = new TimeSpan(17, 0, 0);


        //}



        public static TimeSpan ConvertDecimalTimeSpan(double cIn1)
        {

            //double cIn1 = (Double)worksheet.Cells[row, 6].Value;
            double hour = cIn1 * 24;
            int r1 = (int)hour;
            double Minutes = hour - r1;
            Minutes = Minutes * 60;
            int z = (int)Minutes;
            TimeSpan ts = new TimeSpan(r1, z, 0);
            return ts;
        }

        
    }
}

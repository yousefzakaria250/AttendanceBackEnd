using AspNetCore.Reporting;
using Data;
using Infrastructure.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace AttendaceBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly AttendanceContext attendanceContext;

        public ReportController(IHostingEnvironment host,AttendanceContext attendanceContext)
        {
           _hostingEnvironment = host;
            this.attendanceContext = attendanceContext;
        }


        [HttpGet]
        [Authorize(Roles = "HR,GM")]
        public ActionResult Export_Data(DateTime dateTime)
        {

            var byteRes = new byte[] { };
            string path = _hostingEnvironment.ContentRootPath + "\\Report\\Report1.rdlc";
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            LocalReport report = new LocalReport(path);
            report.AddDataSource("DataSet1",attendanceContext.GetAllAttendance(dateTime));
            var result = report.Execute(RenderType.Pdf, 1);
            
            return File(result.MainStream,
                System.Net.Mime.MediaTypeNames.Application.Octet,
                "ReportName.pdf");
        }

    }
}

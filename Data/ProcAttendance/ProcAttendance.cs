using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class ProcAttendance :EntityBase
    {
        public string Name { set; get;  }
        public DateTime Date { set; get; }
        public string Day { set; get;  }
        public TimeSpan CheckIn { set; get; }
        public TimeSpan CheckOut { set; get; }
        public string CheckInDifference { set; get; }
        public string CheckOutDifference { set; get;}

    }
}

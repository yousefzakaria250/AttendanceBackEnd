using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Attendance
{
    public class Attendances :EntityBase
    {
        public string Code { set; get;  }
        public string Name { set; get;  }
        public DateTime Date { set; get;  }
        public string Day { set; get; }
        public TimeSpan CheckIn { set; get; } = new TimeSpan(0, 0, 0);
        public TimeSpan CheckOut { set; get; } = new TimeSpan(0,0,0);
        
    }
}

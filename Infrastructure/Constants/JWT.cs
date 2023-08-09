using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Constants;

public class JWT
{
    public string secret { get; set; }
    public string ValidIssuer { get; set; }
    public string ValidAudiance { get; set; }
    public double DurationInDays
    {
        get; set;
    }
}

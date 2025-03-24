using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthoringApi.NetCoreApp.Model.AuthoringApi
{
    public class ApiErrorLocation
    {
        public int Line { get; set; }
        public int Column { get; set; }
    }
}

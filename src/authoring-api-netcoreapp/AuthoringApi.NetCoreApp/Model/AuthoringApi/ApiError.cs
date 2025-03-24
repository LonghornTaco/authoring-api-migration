using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthoringApi.NetCoreApp.Model.AuthoringApi
{
    public class ApiError
    {
        public string Message { get; set; }
        public IEnumerable<ApiErrorLocation> Locations { get; set; }
        public IEnumerable<string> Path { get; set; }
    }
}

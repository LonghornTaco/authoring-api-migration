using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthoringApi.NetCoreApp.Model.ManagementApi.Response
{
    public class JobStatus
    {
        public int Total { get; set; }
        public IEnumerable<string> Messages { get; set; }
        public int Processed { get; set; }
        public string JobState { get; set; }
    }
}

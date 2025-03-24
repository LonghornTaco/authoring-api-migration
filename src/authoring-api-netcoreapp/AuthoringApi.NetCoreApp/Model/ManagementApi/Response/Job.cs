using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthoringApi.NetCoreApp.Model.ManagementApi.Response
{
    public class Job
    {
        public string Name { get; set; }
        public string Handle { get; set; }
        public JobStatus Status { get; set; }
        public bool Done { get; set; }
    }
}

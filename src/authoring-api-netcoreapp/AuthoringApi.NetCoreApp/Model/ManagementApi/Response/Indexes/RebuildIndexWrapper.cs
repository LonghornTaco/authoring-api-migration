using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthoringApi.NetCoreApp.Model.ManagementApi.Response.Indexes
{
    public class RebuildIndexWrapper
    {
        public IEnumerable<Job> Jobs { get; set; }
    }
}

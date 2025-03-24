using AuthoringApi.NetCoreApp.Model.AuthoringApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthoringApi.NetCoreApp.Model.ManagementApi.Response.Jobs
{
    public class JobStatusResponse : ApiResponse
    {
        public JobStatusData Data { get; set; }
    }
}

using AuthoringApi.NetCoreApp.Model.AuthoringApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthoringApi.NetCoreApp.Model.ManagementApi.Response.Indexes
{
    public class RebuildIndexResponse : ApiResponse
    {
        public RebuildIndexData Data { get; set; }
    }
}

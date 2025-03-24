using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthoringApi.NetCoreApp.Model.AuthoringApi.Request;

namespace AuthoringApi.NetCoreApp.Model.AuthoringApi.Response.Update
{
    public class UpdateItemResponse : ApiResponse
    {
        public UpdateItemData Data { get; set; }
    }
}

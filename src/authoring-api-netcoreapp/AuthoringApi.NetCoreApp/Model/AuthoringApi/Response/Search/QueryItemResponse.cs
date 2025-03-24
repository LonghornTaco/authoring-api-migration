using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthoringApi.NetCoreApp.Model.AuthoringApi.Response.Search
{
    public class QueryItemResponse : ApiResponse
    {
        public QueryItemData Data { get; set; }
    }
}

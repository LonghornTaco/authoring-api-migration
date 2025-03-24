using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthoringApi.NetCoreApp.Model.AuthoringApi
{
    public class ApiResponse : IApiResponse
    {
        public IEnumerable<ApiError> Errors { get; set; }
        public bool Success => (Errors?.Count() ?? 0) == 0;
    }
}

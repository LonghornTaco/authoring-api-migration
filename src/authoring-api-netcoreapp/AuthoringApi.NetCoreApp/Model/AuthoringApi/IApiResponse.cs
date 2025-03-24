using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthoringApi.NetCoreApp.Model.AuthoringApi
{
    public interface IApiResponse
    {
        IEnumerable<ApiError> Errors { get; }
        bool Success { get; }
    }
}

using AuthoringApi.NetCoreApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthoringApi.NetCoreApp.Controllers
{
    [ApiController]
    [Route("api/management")]
    public class ManagementController : ControllerBase
    {
        private readonly IManagementApiService _managementApi;

        public ManagementController(IManagementApiService managementApi)
        {
            _managementApi = managementApi ?? throw new ArgumentNullException(nameof(managementApi));
        }

        [HttpGet("indexes")]
        public IActionResult GetIndexes()
        {
            var indexes = _managementApi.GetIndexes();
            return Ok(indexes);
        }
    }
}

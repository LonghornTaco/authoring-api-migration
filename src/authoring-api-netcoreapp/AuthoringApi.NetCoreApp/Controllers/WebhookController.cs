using AuthoringApi.NetCoreApp.Model;
using AuthoringApi.NetCoreApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthoringApi.NetCoreApp.Controllers
{
    [ApiController]
    [Route("api/webhooks")]
    public class WebhookController : ControllerBase
    {
        private readonly IAuthoringApiService _authoringApi;

        public WebhookController(IAuthoringApiService authoringApi)
        {
            _authoringApi = authoringApi ?? throw new ArgumentNullException(nameof(authoringApi));
        }

        [HttpPost("updatenavigationtitle")]
        public IActionResult UpdateNavigationTitle([FromBody] ItemSavedRequest request)
        {
            const string titleFieldId = "0efa10f5-63cb-4204-960b-538025e9757b";
            var fieldChange = request.Changes?.FieldChanges?.FirstOrDefault(x => x.FieldId == titleFieldId);
            if (fieldChange != null)
            {
                var query = @"
                    itemId:""" + request.Item.Id + @"""
                    fields:[
                    {name:""NavigationTitle"", value:""" + fieldChange.Value + @"""}
                    ] 
                    ";
                var result = _authoringApi.UpdateItem(query);

                return Ok(result);
            }

            return Ok("The Title field was not updated");
        }
    }
}



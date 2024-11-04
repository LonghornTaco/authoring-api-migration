using AuthoringApi.Webhook.Model;
using AuthoringApi.Webhook.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthoringApi.Webhook.Controllers
{
    [ApiController]
    [Route("api/webhooks")]
    public class WebhookController : ControllerBase
    {
        private readonly IAuthoringApi _authoringApi;

        public WebhookController(IAuthoringApi authoringApi)
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
                    mutation UpdateItem {
                      updateItem(
                        input:{
                          itemId:""" + request.Item.Id + @"""
                          fields:[
                            {name:""NavigationTitle"", value:""" + fieldChange.Value + @"""}
                          ]  
                        }
    
                      ) {
                        item {
                          name
                        }
                      }
                    }
                    ";
                var result = _authoringApi.ExecuteQuery(query);

                return Ok(result);
            }

            return Ok("The Title field was not updated");
        }
    }
}
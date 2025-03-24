using AuthoringApi.NetCoreApp.Model.AuthoringApi.Request;

namespace MayoXm.Edb.Api.Model.Webhooks.Request
{
    [Serializable]
    public class ItemSavedRequest
    {
        public string EventName { get; set; }
        public WebhookRequestItem Item { get; set; }
        public string WebhookItemId { get; set; }
        public string WebhookItemName { get; set; }
        public WebhookChanges Changes { get; set; }
    }
}

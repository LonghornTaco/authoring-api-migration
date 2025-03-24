namespace AuthoringApi.NetCoreApp.Model
{
    [Serializable]
    public class ItemSavedRequest
    {
        public string EventName { get; set; }
        public WebhookItem Item { get; set; }
        public string WebhookItemId { get; set; }
        public string WebhookItemName { get; set; }
        public WebhookChanges Changes { get; set; }
    }
}

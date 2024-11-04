namespace AuthoringApi.Webhook.Model
{
    public class WebhookFieldChange
    {
        public string FieldId { get; set; }
        public string Value { get; set; }
        public string OriginalValue { get; set; }
    }
}

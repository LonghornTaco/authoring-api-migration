namespace AuthoringApi.Webhook.Model
{
    public class WebhookChanges
    {
        public IEnumerable<WebhookFieldChange> FieldChanges { get; set; }
        public bool IsUnversionedFieldChanged { get; set; }
        public bool IsSharedFieldChanged { get; set; }
    }
}

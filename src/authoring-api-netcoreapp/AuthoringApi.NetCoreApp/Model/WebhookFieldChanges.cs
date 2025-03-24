namespace AuthoringApi.NetCoreApp.Model
{
    public class WebhookFieldChange
    {
        public string FieldId { get; set; }
        public string Value { get; set; }
        public string OriginalValue { get; set; }
    }
}

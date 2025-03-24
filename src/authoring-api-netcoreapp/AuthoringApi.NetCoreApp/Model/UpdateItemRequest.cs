namespace AuthoringApi.NetCoreApp.Model
{
    public class UpdateItemRequest
    {
        public string ItemName { get; set; }
        public string FieldName { get; set; }
        public string NewValue { get; set; }
    }
}

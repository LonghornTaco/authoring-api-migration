using MayoXm.Edb.Api.Model.Webhooks.Request;

namespace AuthoringApi.NetCoreApp.Model.AuthoringApi.Request
{
    [Serializable]
    public class WebhookRequestItem
    {
        public string Id { get; set; }
        public string Language { get; set; }
        public int Version { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        public string TemplateId { get; set; }
        public string MasterId { get; set; }
        public IEnumerable<WebhookField> SharedFields { get; set; }
        public IEnumerable<WebhookField> UnversionedFields { get; set; }
        public IEnumerable<WebhookField> VersionedFields { get; set; }
    }
}

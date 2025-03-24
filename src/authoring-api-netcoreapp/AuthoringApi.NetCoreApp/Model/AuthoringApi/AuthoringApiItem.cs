using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthoringApi.NetCoreApp.Model.AuthoringApi
{
    public class AuthoringApiItem
    {
        public string ItemId { get; set; }
        public string DisplayName { get; set; }
        public bool HasPresentation { get; set; }
        public string Language { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public AuthoringApiItem Parent { get; set; }
        public AuthoringApiTemplate Template { get; set; }
        public int Version { get; set; }
        public AuthoringApiFields Fields { get; set; }
        public AuthoringApiChildCollection Children { get; set; }
    }
}

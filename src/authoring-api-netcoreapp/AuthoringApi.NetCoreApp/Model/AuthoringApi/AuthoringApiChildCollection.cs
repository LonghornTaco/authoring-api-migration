using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthoringApi.NetCoreApp.Model.AuthoringApi
{
    public class AuthoringApiChildCollection
    {
        public IEnumerable<AuthoringApiItem> Nodes { get; set; }
    }
}

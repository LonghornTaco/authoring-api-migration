﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthoringApi.NetCoreApp.Model.AuthoringApi.Response.Delete
{
    public class DeleteItemResponse : ApiResponse
    {
        public DeleteItemData Data { get; set; }
    }
}

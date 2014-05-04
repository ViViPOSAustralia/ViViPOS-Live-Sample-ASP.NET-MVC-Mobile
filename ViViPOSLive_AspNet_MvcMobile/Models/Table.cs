using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcMobile.Models
{
    public class Table
    {

        [JsonProperty("table_no")]
        public string TableNo { get; set; }

        [JsonProperty("table_name")]
        public string TableName { get; set; }

    }
}
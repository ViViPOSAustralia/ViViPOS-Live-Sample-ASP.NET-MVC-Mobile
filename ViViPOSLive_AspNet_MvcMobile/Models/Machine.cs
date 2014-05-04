using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcMobile.Models
{
    public class Machine
    {

        public string MacAddress { get; set; }


        public string Serial { get; set; }


        public bool Online { get; set; }
    }
}
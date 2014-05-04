using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcMobile.Messaging
{
    public class Request
    {
        public Meta meta { get; set; }

        public object data { get; set; }
    }
}
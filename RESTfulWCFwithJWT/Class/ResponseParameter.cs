using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RESTfulWCFwithJWT
{
    public class Response
    {
        public string TranCode { get; set; }
        public string Payload { get; set; }
        public string Token { get; set; }
        public string ResponseDescription { get; set; }
    }
}
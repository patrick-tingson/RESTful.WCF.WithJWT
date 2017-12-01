using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace RESTfulWCFwithJWT
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IRESTfulWCFwithJWT" in both code and config file together.
    [ServiceContract]
    public interface IRESTfulWCFwithJWT
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "EchoTest/{alg}/{tranCode}/?jwt={jwt}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json)]
        Response EchoTest(string alg, string tranCode, string jwt); 
    }
}

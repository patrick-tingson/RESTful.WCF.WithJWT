using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace RESTfulWCFwithJWT
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "RESTfulWCFwithJWT" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select RESTfulWCFwithJWT.svc or RESTfulWCFwithJWT.svc.cs at the Solution Explorer and start debugging.
    public class RESTfulWCFwithJWT : IRESTfulWCFwithJWT
    {
        public Response EchoTest(string alg, string tranCode, string jwt)
        {
            var response = new Response();
            try
            {
                var requestProcessor = new RequestProcessor(alg, tranCode, jwt);
                response = requestProcessor.Execute();
            }
            catch(Exception ex)
            {
                //Code for exception handling
            }
            return response;
        }
    }
}

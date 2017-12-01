using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CosumeRESTfulWCFwithJWT
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var requestProcessor = new RequestProcessor();
                requestProcessor.ConsumeEchoTestHS256(); 

                //FOR SPACING
                Console.WriteLine("");
                Console.WriteLine("");
                 
                requestProcessor.ConsumeEchoTestRS256();

            }
            catch (Exception ex)
            {
                //Code for exception handling
                Console.WriteLine(ex.ToString());
            }


            Console.ReadLine();
        } 
    }

}

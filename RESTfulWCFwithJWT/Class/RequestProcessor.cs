using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace RESTfulWCFwithJWT
{

    public class RequestProcessor
    {
        //Sample SecretKey for HS256
        private string secretKey = "1234567890123456";

        //Sample PublicKey PEM format for RS256
        private string pubKey = "-----BEGIN PUBLIC KEY-----MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAoT2r2y1s/BmiOSzW4mhax90NrPZY16D83ax74BxQS1r37Lw20ozK3ZoCWSnJ1vT0Fwd1wFRJ05xZku+dRPkYkWh9Kx+5+QAh7XCZM8e+8DXtxOomx7DZsBPrjw+MU0FpQltkz9Z/2YA3CDR3HQmc0F1YmTs7CQSNxD5vW1gyGgc4y306XKiWKT0B2rCxCNoZmNH2H/Y+5XlHTRVdn3yKTfJM2ga5fCQRbMxb+gP+aANF8S6SyDN1S3gW1ZtY9rXNkXmBZqWHFPJ2LmVQk+S74w+xUjpvAkPgx1o7hkQkf06wLlQRISZ1gbxcsfxYZyKTVVSHn6pPObT25aytqVLmpQIDAQAB-----END PUBLIC KEY-----";

        private string jwt;
        private string tranCode;
        private string alg;
        private JwtSecurityToken jwtSecToken = null; 

        public RequestProcessor(string alg, string tranCode, string jwt)
        {
            //Check if the parameter is filled.
            //Request must always be valid.
           
            if (jwt.Length == 0)
                throw new ArgumentNullException("Invalid Jwt");

            if (tranCode.Length == 0)
                throw new ArgumentNullException("Invalid TranCode");

            if (alg.Length == 0)
                throw new ArgumentNullException("Invalid Algorithm");

            this.jwt = jwt;
            this.tranCode = tranCode;
            this.alg = alg;
        }

        public Response Execute()
        {
            var result = ""; 
            var response = new Response();

            response.Token = jwt;
            response.TranCode = tranCode;

            try
            {
                //Validate token
                var validateToken = Validate();

                if (validateToken != null)
                {
                    //Fill data needed for the Echo Response
                    jwtSecToken = validateToken; 
                    response.ResponseDescription = "SUCCESSFULLY VALIDATED";
                    response.Payload = validateToken.ToString();
                    result = JsonConvert.SerializeObject(response);
                }
                else
                {
                    response.ResponseDescription = "INVALID JWT";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                response.ResponseDescription = ex.ToString();
            }

            return response;
        }

        private JwtSecurityToken Validate()
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                SecurityKey signingKey;

                if (alg == "HS256")
                    signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                else if (alg == "RS256")
                    signingKey = RSASecurityKey();
                else
                    return null; 

                var tokenValidationParameters = new TokenValidationParameters()
                {
                    ValidAudience = "echotesting",
                    ValidateIssuer = false,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = signingKey
                };

                SecurityToken validatedToken;

                tokenHandler.ValidateToken(jwt, tokenValidationParameters, out validatedToken);

                return (JwtSecurityToken)validatedToken;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private RsaSecurityKey RSASecurityKey()
        {
            var rsa = new System.Security.Cryptography.RSACryptoServiceProvider();
            rsa.LoadPublicKeyPEM(pubKey);
            return new RsaSecurityKey(rsa); 
        }
           

        private string GetDataFromJWT(string type)
        {
            var result = jwtSecToken.Claims.SingleOrDefault(w => w.Type == type);
            return result != null ? result.Value : null;
        }
    }
}
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
    public class RequestProcessor
    {

        //Sample SecretKey for HS256
        string secretKey = "1234567890123456";

        //Sample PrivateKey PEM format for RS256
        string privKey = "-----BEGIN RSA PRIVATE KEY-----MIIEowIBAAKCAQEAoT2r2y1s/BmiOSzW4mhax90NrPZY16D83ax74BxQS1r37Lw20ozK3ZoCWSnJ1vT0Fwd1wFRJ05xZku+dRPkYkWh9Kx+5+QAh7XCZM8e+8DXtxOomx7DZsBPrjw+MU0FpQltkz9Z/2YA3CDR3HQmc0F1YmTs7CQSNxD5vW1gyGgc4y306XKiWKT0B2rCxCNoZmNH2H/Y+5XlHTRVdn3yKTfJM2ga5fCQRbMxb+gP+aANF8S6SyDN1S3gW1ZtY9rXNkXmBZqWHFPJ2LmVQk+S74w+xUjpvAkPgx1o7hkQkf06wLlQRISZ1gbxcsfxYZyKTVVSHn6pPObT25aytqVLmpQIDAQABAoIBACfwyONQC1EfYGndS5Vl2CbuAPc5RqSTQk/+6+iF2vXvoL5JmSLqsU3XWoGPsmnG37fcpzRvLKJ4dk/JfyGYupc6VNcb0st1VvIkFC8ZaZjDIxTGE7kfe6z8Ijub1FzDNTm0vfIl5iGQexFTPbY5rViH5ux+GY+QSzWzaY4s+KwtfQ0KWRPNtPYXwwmtiCJAGfN3a5qitJNrMY4gSxoBa0azurkc4N8TXMKxsAHg1S+2oi6UUe52ylr3n/3wsb21RNvaknB1TFiLsHGuQMWyWxAcYky2105ZbtGCjQiFsrHqQqVtwJwWfWkAoul5idIPXtix7i4tJ0A3GZUTmH7mCXkCgYEA06R4rEEufZmt/M27tSc6pWwmLTEj40jeVTBhZEueLORbrw9qRblfBxQf2nioXQm7mltL9Nn0NiOuvuKN4spbNJ/hdFFQ8ReFRatAqpF+fnsD7jX/Ry6FOtupgOopVCHI8/RuYylwF9q3BUFN7JHgzMYsOBYGdVHgRU7LipRuqtsCgYEAwwjwdoul3BKXO8d311TUh8RRuZKY+ENCdtaACrNkKeBQ3Jp1L8uo8SVigUKE/g1SpbtQVQhBULnv9qCxsFscJEu1iQOsQD0SN1WnPkd7pIfglEIHLe1HihawBspcTPsjU1VMTlg8MVEj31a/13sp2t5/J5ONwGaUW5yp+4YzrH8CgYA1fOSujA6m2ZcaRBiDcPWmZw3C8B9kyr6+AusqQN4p9FCjjp7KHk5A6LogKcxLLzGFkjtBF3Bb2mrIfVNklMW2KA3/qqltNQeOkvhV4013w7k7k9P/dmdfd7KADS4CwEMcPJNFZmyY6sLEhjueZUPOHOuCBTb+oYGvonlYfaj2bwKBgEzs5LNNJ1EjAAVzCmCjM+SM2VMhcDZTiQ6IUh5mXTZrJFmujlQYtvzOKwkirIPry1JVDD1NsT1e6TfUb+FIPlROjD6LdVAtBldO8FUPKsRdV4YCeQRzV0ku46T6AefXEjrXJO6tvKgTAdwgQjCCgqKyKWL5vupJS3DK3Py5FP6PAoGBAM7d2P02e+oVGMKc/gHogEAcZBs+prLNfzbwHhyqA2N/GPKSR4CJYtheDOz/tK4FDP/vTWWAE2qAe8L3HUX9vttYrFvP0pJueF4c+Swp9i9/yQQY0k5pMaL0zPx0/ixLsBhm2GxEWXaZgFZzo77UH1USc33pxWUkuEjNV+RBGS8+-----END RSA PRIVATE KEY-----";

        string tranURL = "http://localhost:11119/RESTfulWCFwithJWT.svc/EchoTest";

        string CreateToken(string alg)
        {
            var signedAndEncodedToken = "";

            try
            {
                SecurityKey signingKey;
                SigningCredentials signingCredentials;

                if (alg == "HS256")
                {
                    //HS256
                    signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                    signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
                }
                else
                {
                    //RSA256
                    signingKey = RSASecurityKey();
                    signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.RsaSha256);
                }

                var claimsIdentity = new ClaimsIdentity(new List<Claim>()
                {
                    new Claim("profileId", "8888"),
                    new Claim("profileName", "Juan dela Cruz"),
                    new Claim("jti", System.Guid.NewGuid().ToString())
                });

                var securityTokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = claimsIdentity,
                    SigningCredentials = signingCredentials,
                    Audience = "echotesting",
                    Expires = DateTime.Now.AddMinutes(20),
                    IssuedAt = DateTime.Now,
                    NotBefore = DateTime.Now,
                };

                var expDate = DateTime.Now.AddMinutes(20);
                var tokenHandler = new JwtSecurityTokenHandler();
                var plainToken = tokenHandler.CreateToken(securityTokenDescriptor);

                signedAndEncodedToken = tokenHandler.WriteToken(plainToken);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return signedAndEncodedToken;
        }

        RsaSecurityKey RSASecurityKey()
        {
            var rsa = new System.Security.Cryptography.RSACryptoServiceProvider();
            rsa.LoadPrivateKeyPEM(privKey);
            return new RsaSecurityKey(rsa);
        }

        public void ConsumeEchoTestHS256()
        {
            var result = "";
            var statusCode = "";

            //TEST CONSUME FOR HS256
            var tranUrlForHS256 = string.Format("{0}/{1}/{2}/?jwt={3}",
                                            tranURL,
                                            "HS256",
                                            "TXN",
                                            CreateToken("HS256"));
            try
            {
                //REQUEST
                WebRequest request = WebRequest.Create(tranUrlForHS256);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = tranUrlForHS256.Length;
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(tranUrlForHS256);
                    streamWriter.Close();
                }

                //RESPONSE
                WebResponse response = request.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    statusCode = ((HttpWebResponse)response).StatusCode.ToString();
                    result = streamReader.ReadToEnd();
                }
                Console.WriteLine("TEST CONSUME FOR HS256");
                Console.WriteLine("STATUS CODE: " + statusCode);
                Console.WriteLine("RESPONSE: " + result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ConsumeEchoTestRS256()
        {
            var statusCode = "";
            var result = "";

            //TEST CONSUME FOR RS256
            var tranUrlForRS256 = string.Format("{0}/{1}/{2}/?jwt={3}",
                                            tranURL,
                                            "RS256",
                                            "TXN",
                                            CreateToken("RS256"));


            try
            {
                //REQUEST
                WebRequest request = WebRequest.Create(tranUrlForRS256);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = tranUrlForRS256.Length;
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(tranUrlForRS256);
                    streamWriter.Close();
                }

                //RESPONSE
                WebResponse response = request.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    statusCode = ((HttpWebResponse)response).StatusCode.ToString();
                    result = streamReader.ReadToEnd();
                }
                Console.WriteLine("TEST CONSUME FOR RS256");
                Console.WriteLine("STATUS CODE: " + statusCode);
                Console.WriteLine("RESPONSE: " + result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

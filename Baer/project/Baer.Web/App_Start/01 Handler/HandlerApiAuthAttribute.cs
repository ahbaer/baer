using JWT;
using JWT.Algorithms;
using JWT.Exceptions;
using JWT.Serializers;
using System.Web;
using System.Web.Mvc;

namespace Baer.Web
{
    public class HandlerApiAuthAttribute : AuthorizeAttribute
    {
        private const string secret = "zmb_baer_2020";

        public bool Ignore = true;
        public HandlerApiAuthAttribute(bool ignore = false)
        {
            Ignore = ignore;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (Ignore == true)
            {
                return;
            }
            string token = HttpContext.Current.Request.Headers["Authorization"];

            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                var provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); //
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);

                decoder.Decode(token, secret, verify: true);
                return;
            }
            catch (TokenExpiredException)
            {
                filterContext.Result = new ContentResult() { Content = "Token has expired" };
                return;
            }
            catch (SignatureVerificationException)
            {
                filterContext.Result = new ContentResult() { Content = "Token has invalid signature" };
                return;
            }
        }
    }
}
using System.Security.Cryptography;
using System.Text;
using dotnetEtsyApp.Helpers.Abstract;
using dotnetEtsyApp.Models;
using dotnetEtsyApp.Models.Cache;

namespace dotnetEtsyApp.Helpers.Concrete
{
    public class CodeGenerator : ICodeGenerator
    {
        private string redirectUrl = Global.DomainName + "/EtsyAccess";

        public CodeVerifierModel codeVerifierModel { get; set; }

        // public CodeVerifierModel codeVerifierModel { get ; set ; }


        public CodeGenerator()
        {
            this.codeVerifierModel = GenerateCode();
        }



        public CodeVerifierModel GenerateCode()
        {
            var rng = RandomNumberGenerator.Create();

            var bytes = new byte[32];
            rng.GetBytes(bytes);

            // It is recommended to use a URL-safe string as code_verifier.
            // See section 4 of RFC 7636 for more details.
            var code_verifier = Convert.ToBase64String(bytes)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');

            var code_challenge = string.Empty;
            using (var sha256 = SHA256.Create())
            {
                var challengeBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(code_verifier));
                code_challenge = Convert.ToBase64String(challengeBytes)
                    .TrimEnd('=')
                    .Replace('+', '-')
                    .Replace('/', '_');
            }
            // generate a random 32 byte string only digiset


            this.codeVerifierModel = new CodeVerifierModel
            {
                CodeVerifier = code_verifier,
                CodeChallange = code_challenge,
                // generate random 7 character string without dashes
                State = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 7)
            };
            return this.codeVerifierModel;

        }
        public CodeVerifierModel GenerateUrl(string StoreApi)
        {
            var codeVerifierModel = this.codeVerifierModel;
            codeVerifierModel.Url = $"https://www.etsy.com/oauth/connect?response_type=code&redirect_uri={this.redirectUrl}&scope=email_r&client_id={StoreApi}&state={codeVerifierModel.State}&code_challenge={codeVerifierModel.CodeChallange}&code_challenge_method=S256";
            return codeVerifierModel;
        }
    }
}
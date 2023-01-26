using dotnetEtsyApp.Models;

namespace dotnetEtsyApp.Helpers.Abstract
{
    public interface ICodeGenerator
    {
        public CodeVerifierModel codeVerifierModel { get ; set ; }
        CodeVerifierModel GenerateCode();
        CodeVerifierModel GenerateUrl(string ClientId);
    }
}
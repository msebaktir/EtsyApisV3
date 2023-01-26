namespace dotnetEtsyApp.Models
{
    public class CodeVerifierModel
    {
        public string CodeVerifier { get; set; } = "";
        public string CodeChallange { get; set; } = "";
        public string State { get; set; } = "";
        public string Url { get; set; } = "";
    }
}
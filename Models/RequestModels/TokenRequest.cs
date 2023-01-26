using System.Text.Json.Serialization;

namespace dotnetEtsyApp.Models.RequestModels
{
    public class TokenRequest
    {
        [JsonPropertyName("grant_type")]
        public string GrantType { get; set; }
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }
        [JsonPropertyName("redirect_uri")]
        public string RedirectUri { get; set; }
        [JsonPropertyName("code")]
        public string Code { get; set; }
        [JsonPropertyName("code_verifier")]
        public string CodeVerifier { get; set; }
    }
}
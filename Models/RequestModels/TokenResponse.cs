using System;
using System.Collections.Generic;

using System.Text.Json;
using System.Text.Json.Serialization;
namespace dotnetEtsyApp.Models.RequestModels

{


    public partial class TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("expires_in")]
        public long ExpiresIn { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
    }

    public partial class TokenResponse
    {
        public static TokenResponse FromJson(string json) => JsonSerializer.Deserialize<TokenResponse>(json, dotnetEtsyApp.Models.RequestModels.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this TokenResponse self) => JsonSerializer.Serialize(self, dotnetEtsyApp.Models.RequestModels.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerOptions Settings = new(JsonSerializerDefaults.Web)
        {
            Converters =
            {
                new DateOnlyConverter(),
                new TimeOnlyConverter(),
            },
        };
    }

    public class DateOnlyConverter : JsonConverter<DateOnly>
    {
        private readonly string serializationFormat;
        public DateOnlyConverter() : this(null) { }

        public DateOnlyConverter(string? serializationFormat)
        {
            this.serializationFormat = serializationFormat ?? "yyyy-MM-dd";
        }

        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return DateOnly.Parse(value!);
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString(serializationFormat));
    }
    public class TimeOnlyConverter : JsonConverter<TimeOnly>
    {
        private readonly string serializationFormat;

        public TimeOnlyConverter() : this(null) { }

        public TimeOnlyConverter(string? serializationFormat)
        {
            this.serializationFormat = serializationFormat ?? "HH:mm:ss.fff";
        }

        public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return TimeOnly.Parse(value!);
        }

        public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString(serializationFormat));
    }
}

using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace dotnetEtsyApp.Models
{

    public  class ApiEndPointsModel
    {
        [JsonProperty("BaseURL")]
        public Uri BaseUrl { get; set; }

        [JsonProperty("Version")]
        public string Version { get; set; }

        [JsonProperty("Paths")]
        public List<Path> Paths { get; set; }

        public ApiEndPointsModel( ApiEndPointsModel apiEndPointsModel)
        {
            BaseUrl = apiEndPointsModel.BaseUrl;
            Version = apiEndPointsModel.Version;
            Paths = apiEndPointsModel.Paths;
        } 
    }

    public partial class Path
    {
        [JsonProperty("path")]
        public string PathPath { get; set; }

        [JsonProperty("method")]
        public Method Method { get; set; }

        [JsonProperty("operationId")]
        public string OperationId { get; set; }
    }

    public enum Method { Delete, Get, Patch, Post, Put };


}
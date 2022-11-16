using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Lib.Core.FeatureFlags
{
    public class Feature
    {
        public Feature()
        {
            IsLog = false;
        }
        public bool IsLog { get; set; }
        // The value of IsAzureApplicationInsight depends on IsLog. If IsLog true, then the value will be use.
        public bool IsAzureApplicationInsight { get; set; }
        // The value of IsPluginTraceLog depends on IsLog. If IsLog true, then the value will be use.
        public bool IsPluginTraceLog { get; set; }
        public bool IsRounding { get; set; }
        public int RoundingDigit { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public MidpointRounding RoundingStrategy { get; set;}
    }
}

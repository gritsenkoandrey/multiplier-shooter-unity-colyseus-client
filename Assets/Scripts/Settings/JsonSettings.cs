using Newtonsoft.Json;

namespace Settings
{
    public static class JsonSettings
    {
        public static readonly JsonSerializerSettings Settings = new()
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            Formatting = Formatting.None
        };
    }
}
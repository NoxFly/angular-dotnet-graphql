using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Core.Helpers;

public static class JsonHelper
{
    public static string Serialize(object obj, Formatting formatting = Formatting.Indented)
    {
        return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DateFormatString = "yyyy-MM-dd",
            Formatting = formatting,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        });
    }
}
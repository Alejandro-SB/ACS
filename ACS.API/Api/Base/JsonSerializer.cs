using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACS.API.Api.Base
{
    public sealed class JsonSerializer
    {
        private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings()
        {
            ContractResolver = new JsonContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented
        };

        public static string SerializeObject(object? o)
        {
            return JsonConvert.SerializeObject(o, _settings);
        }

        public sealed class JsonContractResolver : CamelCasePropertyNamesContractResolver { }
    }
}

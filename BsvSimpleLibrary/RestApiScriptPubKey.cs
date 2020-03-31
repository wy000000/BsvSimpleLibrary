using Newtonsoft.Json;

namespace BsvSimpleLibrary
{
    public class RestApiScriptPubKey
    {
        [JsonProperty("asm")]
        public string Asm { get; set; }
        [JsonProperty("hex")]
        public string Hex { get; set; }
        [JsonProperty("reqSigs")]
        public string ReqSigs { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("addresses")]
        public string[] Addresses { get; set; }
        [JsonProperty("isTruncated")]
        public string isTruncated { get; set; }
    }
}
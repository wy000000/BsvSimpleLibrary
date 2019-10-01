using Newtonsoft.Json;

namespace BsvSimpleLibrary
{
    public class BitIndexScriptSig
    {
        [JsonProperty("asm")]
        public string Asm { get; set; }

        [JsonProperty("hex")]
        public string Hex { get; set; }
    }
}
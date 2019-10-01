using Newtonsoft.Json;

namespace BsvSimpleLibrary
{
    public class BitIndexInput
    {
        [JsonProperty("value")]
        public decimal Value { get; set; }
        [JsonProperty("valueSat")]
        public long ValueSatoshis { get; set; }
        [JsonProperty("txid")]
        public string TxId { get; set; }
        [JsonProperty("vout")]
        public int OutIndex { get; set; }
        [JsonProperty("n")]
        public int N { get; set; }
        [JsonProperty("scriptSig")]
        public BitIndexScriptSig ScriptSig { get; set; }
        [JsonProperty("addr")]
        public string Addr { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("sequence")]
        public long Sequence { get; set; }
    }
}
using Newtonsoft.Json;

namespace BsvSimpleLibrary
{
    public class BitIndexTransaction
    {
        [JsonProperty("txid")]
        public string TxId { get; set; }
        [JsonProperty("hash")]
        public string Hash { get; set; }
        [JsonProperty("size")]
        public int Size { get; set; }
        [JsonProperty("version")]
        public int Version { get; set; }
        [JsonProperty("locktime")]
        public int LockTime { get; set; }
        [JsonProperty("vin")]
        public BitIndexInput[] Inputs { get; set; }
        [JsonProperty("vout")]
        public BitIndexOutput[] Outputs { get; set; }
        [JsonProperty("blockhash")]
        public string BlockHash { get; set; }
        [JsonProperty("confirmations")]
        public int Confirmations { get; set; }
        [JsonProperty("time")]
        public long Time { get; set; }
        [JsonProperty("blocktime")]
        public long BlockTime { get; set; }
        [JsonProperty("valueIn")]
        public decimal ValueIn { get; set; }
        [JsonProperty("fees")]
        public decimal Fees { get; set; }
        [JsonProperty("valueOut")]
        public decimal ValueOut { get; set; }
        [JsonProperty("rawtx")]
        public string RawTx { get; set; }
    }
}
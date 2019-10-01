using Newtonsoft.Json;

namespace BsvSimpleLibrary
{
    public class BitIndexAddressInfo
    {
        [JsonProperty("addrStr")]
        public string Address { get; set; }

        [JsonProperty("balance")]
        public decimal Balance { get; set; }

        [JsonProperty("balanceSat")]
        public long BalanceSatoshis { get; set; }

        [JsonProperty("totalReceived")]
        public decimal TotalReceived { get; set; }

        [JsonProperty("totalReceivedSat")]
        public decimal TotalReceivedSatoshi { get; set; }

        [JsonProperty("totalSent")]
        public long TotalSent { get; set; }

        [JsonProperty("totalSentSat")]
        public long TotalSentSatoshi { get; set; }

        [JsonProperty("unconfirmedBalance")]
        public long UnconfirmedBalance { get; set; }

        [JsonProperty("unconfirmedBalanceSat")]
        public long UnconfirmedBalanceSatoshi { get; set; }

        [JsonProperty("unconfirmedTxApperances")]
        public long UnconfirmedTxApperances { get; set; }

        [JsonProperty("txApperances")]
        public long TxApperances { get; set; }

        [JsonProperty("transactions")]
        public string[] Transactions { get; set; }
    }
}
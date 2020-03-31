using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BsvSimpleLibrary
{
    public class RestApiUtxo_class
    {
        //[JsonProperty("address")]
        //public string Address { get; set; }

        [JsonProperty("tx_hash")]
        public string TxId { get; set; }

        [JsonProperty("tx_pos")]
        public uint OutIndex { get; set; }

        [JsonProperty("value")]
        public long Value { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }

        //[JsonProperty("confirmations")]
        //public long Confirmations { get; set; }

        //[JsonProperty("scriptPubKey")]
        //public string ScriptPubKey { get; set; }

        //[JsonProperty("satoshis")]
        //public long Satoshis { get; set; }

        //[JsonProperty("amount")]
        //public decimal Amount { get; set; }
    }
}

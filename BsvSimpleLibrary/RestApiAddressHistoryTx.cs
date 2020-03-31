using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace BsvSimpleLibrary
{
    public class RestApiAddressHistoryTx
    {
        [JsonProperty("tx_hash")]
        public string TxHash { get; set; }
        [JsonProperty("height")]
        public string Height { get; set; }

    }
}

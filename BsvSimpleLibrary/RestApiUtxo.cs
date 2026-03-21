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
		[JsonProperty("tx_hash")]
		public string TxId { get; set; }

		[JsonProperty("tx_pos")]
		public uint OutIndex { get; set; }

		[JsonProperty("value")]
		public long Value { get; set; }

		[JsonProperty("height")]
		public long Height { get; set; }

		[JsonProperty("isSpentInMempoolTx")]
		public bool IsSpentInMempoolTx { get; set; }

		[JsonProperty("status")]
		public string Status { get; set; }
	}

	public class RestApiUtxoResponse
	{
		[JsonProperty("address")]
		public string Address { get; set; }

		[JsonProperty("script")]
		public string Script { get; set; }

		[JsonProperty("result")]
		public List<RestApiUtxo_class> Result { get; set; }

		[JsonProperty("error")]
		public string Error { get; set; }
	}

}

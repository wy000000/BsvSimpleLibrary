using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace BsvSimpleLibrary
{
	public class WocHistoryResultItem
	{
		[JsonProperty("tx_hash")]
		public string TxHash { get; set; }

		[JsonProperty("height")]
		public long? Height { get; set; }
	}

	public class WocHistoryGroup
	{
		[JsonProperty("result")]
		public List<WocHistoryResultItem> Result { get; set; }

		[JsonProperty("error")]
		public string Error { get; set; }
	}

	public class WocAddressHistoryResponse
	{
		[JsonProperty("address")]
		public string Address { get; set; }

		[JsonProperty("script")]
		public string Script { get; set; }

		[JsonProperty("unconfirmed")]
		public WocHistoryGroup Unconfirmed { get; set; }

		[JsonProperty("confirmed")]
		public WocHistoryGroup Confirmed { get; set; }
	}

	public class RestApiAddressHistoryTx
	{
		[JsonProperty("address")]
		public string Address { get; set; }

		[JsonProperty("isConfirmed")]
		public bool IsConfirmed { get; set; }

		[JsonProperty("tx_hash")]
		public string TxHash { get; set; }

		[JsonProperty("height")]
		public long? Height { get; set; }
	}

}

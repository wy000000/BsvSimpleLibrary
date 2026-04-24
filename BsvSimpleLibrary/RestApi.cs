using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using NBitcoin.DataEncoders;
using NBitcoin;
using BsvSimpleLibrary;

namespace BsvSimpleLibrary
{
	public class RestApi_class
	{
		public static string sendTransaction(string uri, string network, string rawtx)
		{
			string contentStr = "{\"txhex\":\"" + rawtx + "\"}";
			Task<string> TaskResponseData = RestApiPostFunction(uri, contentStr, network, bsvConfiguration_class.sendRawTransaction);
			//Dictionary<string, string> responseDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(TaskResponseData.Result);
			//Dictionary<string, string> responseDic = new Dictionary<string, string>();       
			//Console.WriteLine();
			//Console.WriteLine(TaskResponseData.Result);
			return (TaskResponseData.Result);
		}
		public static RestApiTransaction[] getTransactions(string uri, string network, string [] txHashs)
		{
			//Max 20 transactions per request
			if (txHashs.Length>20)
			{
				Console.WriteLine();
				Console.WriteLine("Error: Max 20 transactions per request");
				return (null);
			}
			string contentStr = "{\"txids\":[";
			for(int i=0;i<txHashs.Length-1;i++)
			{
				contentStr = contentStr + "\"" + txHashs[i] + "\",";
			}
			contentStr = contentStr + "\"" + txHashs[txHashs.Length - 1] + "\"]}";
			Task<string> TaskResponseData
				= RestApiPostFunction(uri, contentStr, network, bsvConfiguration_class.getTxs);
			RestApiTransaction[] txs = JsonConvert.DeserializeObject<RestApiTransaction[]>(TaskResponseData.Result);
			return (txs);
		}

		public static RestApiTransaction getTransaction(string uri, string network, string txid)
		{
			Task<string> TaskResponseData = RestApiGetFunction(uri, network, bsvConfiguration_class.getTx, txid);
			RestApiTransaction Btx = JsonConvert.DeserializeObject<RestApiTransaction>(TaskResponseData.Result);
			return (Btx);
		}

		public static string getRawTransaction(string uri, string network, string txid)
		{
			Task<string> TaskResponseData = RestApiGetFunction(uri, network, bsvConfiguration_class.getRawtx, txid);
			string responseData = TaskResponseData.Result;
			return responseData;
			//Dictionary<string, string> responseDic = null;
			//try
			//{
			//	//responseDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(TaskResponseData.Result);
			//	//return (responseDic);
			//}
			//catch (JsonReaderException e)
			//{
			//	Console.WriteLine();
			//	Console.WriteLine(e.Message);
			//	responseDic = new Dictionary<string, string>();
			//	responseDic.Add("Error", e.Message);
			//	return (responseDic);
			//}
		}

		public static RestApiUtxo_class[] getUtxosByAnAddress(string uri, string network, string addr)
		{
			Task<string> TaskResponseData = RestApiGetFunction(uri, network, bsvConfiguration_class.getUtxosByAnAddress, addr);
			string responseData = TaskResponseData.Result;
			try
			{
				RestApiUtxoResponse resp = JsonConvert.DeserializeObject<RestApiUtxoResponse>(responseData);
				RestApiUtxo_class[] utxos = resp?.Result?.ToArray() ?? Array.Empty<RestApiUtxo_class>();
				//上一句等价于下面的代码
				/*RestApiUtxo_class[] utxos;
				if (resp != null)
				{
					if (resp.Result != null)
					{
						utxos = resp.Result.ToArray();
					}
					else
					{
						utxos = Array.Empty<RestApiUtxo_class>();
					}
				}
				else
				{
					utxos = Array.Empty<RestApiUtxo_class>();
				}*/
				Console.WriteLine();
				Console.WriteLine("utxos length:" + utxos.Length);
				return (utxos);
			}
			catch (JsonSerializationException e)
			{
				Console.WriteLine();
				Console.WriteLine(e.Message);
				return (null);
			}
		}
		public class WocAddressHistoryRequest
		{
			[JsonProperty("addresses")]
			public string[] Addresses { get; set; }
		}

		public static RestApiAddressHistoryTx[] getAddressHistory(string uri, string network, string addr)
		{
			string[] addresses= new string[1] { addr };
			RestApiAddressHistoryTx[] historyTxes= getAddressesHistory(uri, network, addresses);
			return (historyTxes);
		}

		public static RestApiAddressHistoryTx[] getAddressesHistory(string uri, string network, string[] addresses)
		{
			WocAddressHistoryRequest requestObj = new WocAddressHistoryRequest
			{
				Addresses = addresses
			};
			string json = JsonConvert.SerializeObject(requestObj);
			Task<string> TaskResponseData = RestApiPostFunction(uri, json, network,
				bsvConfiguration_class.getAddressHistory);
			string responseData = TaskResponseData.Result;
			RestApiAddressHistoryTx[] addrHistory = ParseAddressHistory(responseData);
			return (addrHistory);
		}

		static RestApiAddressHistoryTx[] ParseAddressHistory(string responseData)
		{
			WocAddressHistoryResponse[] raw =
				JsonConvert.DeserializeObject<WocAddressHistoryResponse[]>(responseData);

			List<RestApiAddressHistoryTx> list = new List<RestApiAddressHistoryTx>();

			foreach (WocAddressHistoryResponse addr in raw)
			{
				// Unconfirmed
				if (addr.Unconfirmed != null && addr.Unconfirmed.Result != null)
				{
					foreach (WocHistoryResultItem item in addr.Unconfirmed.Result)
					{
						RestApiAddressHistoryTx tx = new RestApiAddressHistoryTx
						{
							Address = addr.Address,
							IsConfirmed = false,
							TxHash = item.TxHash,
							Height = null
						};
						list.Add(tx);
					}
				}

				// Confirmed
				if (addr.Confirmed != null && addr.Confirmed.Result != null)
				{
					foreach (WocHistoryResultItem item in addr.Confirmed.Result)
					{
						RestApiAddressHistoryTx tx = new RestApiAddressHistoryTx
						{
							Address = addr.Address,
							IsConfirmed = true,
							TxHash = item.TxHash,
							Height = item.Height
						};
						list.Add(tx);
					}
				}
			}
			return list.ToArray(); //List<RestApiAddressHistoryTx> list
		}

		public static string getOpReturnFullData(RestApiTransaction tx)
		{
			if (tx != null)
			{
				if (tx.Outputs != null)
				{
					string opReturnHexStr = null;
					foreach (RestApiOutput output in tx.Outputs)
					{
						opReturnHexStr = output.ScriptPubKey.Hex;
						if (opReturnHexStr.Substring(0, 4) == "006a"
							|| opReturnHexStr.Substring(0, 2) == "6a")
						{
							//HexEncoder hexEncoder = new HexEncoder();
							//byte[] bytes = hexEncoder.DecodeData(opReturnHexStr);
							return (opReturnHexStr);
						}
					}
				}
			}
			return (null);
		}

		public static string getOpReturnData(RestApiTransaction tx, Encoding encoder)
		{
			string s = null;
			byte[] bytes = getOpReturnFullData(tx);
			if (bytes != null)
			{
				int prefixLength = 0;
				if (bytes[0] == 0x00)
					prefixLength = 2;
				else
					prefixLength = 1;
				if (bytes.Length - 2 > 0)
				{
					byte[] strBytes = bytes.Skip(prefixLength).ToArray();
					s = encoder.GetString(strBytes);
				}
			}
			Console.WriteLine(s);
			return (s);
		}

		public static byte[] getOpReturnFullData(string uri, string network, string txid)
		{
			if (txid != null)
			{
				RestApiTransaction tx = getTransaction(uri, network, txid);
				byte[] bytes = getOpReturnFullData(tx);
				return (bytes);
			}
			return (null);
		}
		public static string getOpReturnData(string uri, string network, string txid, Encoding encoder)
		{
			string s = null;
			byte[] bytes = getOpReturnFullData(uri, network, txid);
			if (bytes != null)
			{
				int prefixLength = 0;
				if (bytes[0] == 0x00)
					prefixLength = 2;
				else
					prefixLength = 1;
				if (bytes.Length - prefixLength > 0)
				{
					byte[] strBytes = bytes.Skip(prefixLength).ToArray();
					s = encoder.GetString(strBytes);
				}
			}
			Console.WriteLine(s);
			return (s);
		}

		/// <summary>
		/// 从 OP_RETURN 脚本 hex 中提取所有数据段
		/// 支持 PUSHDATA1 / PUSHDATA2 / PUSHDATA4
		/// </summary>
		static List<byte[]> ExtractOpReturnData(string scriptHex)
		{
			List<byte[]> results = new List<byte[]>();

			// 将 hex 转为 Script
			Script script = Script.FromHex(scriptHex);

			// 遍历所有操作码
			foreach (Op op in script.ToOps())
			{
				if (op.Code == OpcodeType.OP_RETURN || op.Code == OpcodeType.OP_FALSE)
				{
					// 跳过 OP_FALSE 和 OP_RETURN
					continue;
				}

				if (op.PushData != null)
				{
					results.Add(op.PushData);
				}
			}
			return results;
		}

		async static Task<string> RestApiGetFunction(string uri, string network, string FunctionString, string iterm)
		{
			//Common testing requirement. If you are consuming an API in a sandbox/test region, uncomment this line of code ONLY for non production uses.
			//System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
			//Be sure to run "Install-Package Microsoft.Net.Http" from your nuget command line.
			string responseData = null;
			using (var httpClient = new HttpClient()) //{ BaseAddress = baseAddress })
			{
				httpClient.BaseAddress = new Uri(uri);
				httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");
				string uristr = string.Format(FunctionString, network, iterm);
				using (HttpResponseMessage response = await httpClient.GetAsync(uristr))
				{
					responseData = await response.Content.ReadAsStringAsync();
				}
			}
			Console.WriteLine();
			Console.WriteLine(responseData);
			return (responseData);
		}
		async static Task<string> RestApiPostFunction(string uri, string contentStr, string network, string functionStr)
		{
			//Common testing requirement. If you are consuming an API in a sandbox/test region, uncomment this line of code ONLY for non production uses.
			//System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
			//Be sure to run "Install-Package Microsoft.Net.Http" from your nuget command line.
			string responseData = null;
			using (var httpClient = new HttpClient())
			{
				httpClient.BaseAddress = new Uri(uri);
				httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");
				string funStr = string.Format(functionStr, network);
				using (StringContent content = new StringContent(contentStr, bsvConfiguration_class.encoding, "application/json"))
				{
					using (var response = await httpClient.PostAsync(funStr, content))
					{
						responseData = await response.Content.ReadAsStringAsync();
					}
				}
			}
			Console.WriteLine();
			Console.WriteLine(responseData);
			return (responseData);
		}

	}
}

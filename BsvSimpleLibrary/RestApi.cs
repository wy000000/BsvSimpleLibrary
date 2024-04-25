using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using NBitcoin.DataEncoders;
using NBitcoin;

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
        //public static Dictionary<string, string> getRawTransaction(string uri, string network, string txid)
        //{
        //    Task<string> TaskResponseData = RestApiGetFunction(uri, network, bsvConfiguration_class.getRawtx, txid);
        //    Dictionary<string, string> responseDic = null;
        //    try
        //    {
        //        responseDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(TaskResponseData.Result);
        //        return (responseDic);
        //    }
        //    catch (JsonReaderException e)
        //    {
        //        Console.WriteLine();
        //        Console.WriteLine(e.Message);
        //        responseDic = new Dictionary<string, string>();
        //        responseDic.Add("Error", e.Message);
        //        return (responseDic);
        //    }
        //}
        public static RestApiUtxo_class[] getUtxosByAnAddress(string uri, string network, string addr)
        {
            Task<string> TaskResponseData = RestApiGetFunction(uri, network, bsvConfiguration_class.getUtxosByAnAddress, addr);
            string responseData = TaskResponseData.Result;
            try
            {
                RestApiUtxo_class[] utxos = JsonConvert.DeserializeObject<RestApiUtxo_class[]>(responseData);
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
        public static RestApiAddressHistoryTx[] getAddressHistory(string uri, string network, string addr)
        {
            Task<string> TaskResponseData = RestApiGetFunction(uri, network,
                bsvConfiguration_class.getAddressHistory, addr);
            string responseData = TaskResponseData.Result;
            RestApiAddressHistoryTx[] addrHistory
                = JsonConvert.DeserializeObject<RestApiAddressHistoryTx[]>(responseData);
            return (addrHistory);
        }

        public static byte[] getOpReturnFullData(RestApiTransaction tx)
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
                            HexEncoder hexEncoder = new HexEncoder();
                            byte[] bytes = hexEncoder.DecodeData(opReturnHexStr);
                            return (bytes);
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
                using (var response = await httpClient.GetAsync(uristr))
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
                using (StringContent content = new StringContent(contentStr, bsvConfiguration_class.encoding, "application/json"))
                {
                    string funStr = string.Format(functionStr, network);
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

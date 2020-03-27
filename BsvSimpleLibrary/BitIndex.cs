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
    public class BitIndex_class
    {
        public static string sendTransaction(string uri, string network, string rawtx)
        {
            string contentStr = "{\"txhex\":\"" + rawtx + "\"}";
            Task<string> TaskResponseData = bitindexPostFunction(uri, contentStr, network, bsvConfiguration_class.sendRawTransaction);
            //Dictionary<string, string> responseDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(TaskResponseData.Result);
            //Dictionary<string, string> responseDic = new Dictionary<string, string>();       
            //Console.WriteLine();
            //Console.WriteLine(TaskResponseData.Result);
            return (TaskResponseData.Result);
        }
        public static BitIndexTransaction getTransaction(string uri, string network, string txid)
        {
            Task<string> TaskResponseData = bitindexGetFunction(uri, network, bsvConfiguration_class.getTx, txid);
            BitIndexTransaction Btx = JsonConvert.DeserializeObject<BitIndexTransaction>(TaskResponseData.Result);
            return (Btx);
        }
        //public static Dictionary<string, string> getRawTransaction(string uri, string network, string txid)
        //{
        //    Task<string> TaskResponseData = bitindexGetFunction(uri, network, bsvConfiguration_class.getRawtx, txid);
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
        public static BitIndexUtxo_class[] getUtxosByAnAddress(string uri, string network, string addr)
        {
            Task<string> TaskResponseData = bitindexGetFunction(uri, network, bsvConfiguration_class.getUtxosByAnAddress, addr);
            string responseData = TaskResponseData.Result;
            try
            {
                BitIndexUtxo_class[] utxos = JsonConvert.DeserializeObject<BitIndexUtxo_class[]>(responseData);
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
        //public static BitIndexAddressInfo getAddressInfo(string uri, string network, string addr)
        //{
        //    Task<string> TaskResponseData = bitindexGetFunction(uri, network, bsvConfiguration_class.getAddressInfo, addr);
        //    string responseData = TaskResponseData.Result;
        //    BitIndexAddressInfo addrInfo = JsonConvert.DeserializeObject<BitIndexAddressInfo>(responseData);
        //    return (addrInfo);
        //}

        public static byte[] getOpReturnData(string uri, string network, string txid)
        {
            BitIndexTransaction tx = getTransaction(uri, network, txid);
            if (tx.Outputs!=null)
            {
                int subLength = 0;
                string opReturnHexStr = null;
                foreach (BitIndexOutput output in tx.Outputs)
                {
                    subLength = 0;
                    if (output.ScriptPubKey.Type == bsvConfiguration_class.opReturnType)
                    {
                        subLength = 4;
                        opReturnHexStr = output.ScriptPubKey.Hex;
                        break;                        
                    }
                    if(output.ScriptPubKey.Type == "nonstandard")
                    {
                        opReturnHexStr = output.ScriptPubKey.Hex;
                        if (opReturnHexStr.Substring(0, 2)=="6a")
                        {
                            subLength = 4;
                            break;
                        }
                    }
                }
                string s = opReturnHexStr.Substring(subLength);
                HexEncoder hexEncoder = new HexEncoder();
                byte[] bytes = hexEncoder.DecodeData(s);
                return (bytes);
            }
            return (null);
        }
        public static string getOpReturnData(string uri, string network, string txid, Encoding encoder)
        {
            string s = null;
            byte[] bytes = getOpReturnData(uri, network, txid);
            if(bytes!=null)
                s = encoder.GetString(bytes);
            return (s);
        }

        async static Task<string> bitindexGetFunction(string uri, string network, string FunctionString, string iterm)
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
        async static Task<string> bitindexPostFunction(string uri, string contentStr, string network, string functionStr)
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

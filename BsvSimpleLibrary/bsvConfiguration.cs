using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BsvSimpleLibrary
{
    public class bsvConfiguration_class
    {
        public static readonly string bitindexUri = "https://api.bitindex.network/";
        public static readonly string testNetwork = "test";
        public static readonly string mainNetwork = "main";
        public static readonly string NbitTestNet = "TestNet";
        public static readonly string NbitMainNet = "Main";
        public static readonly string sendRawTransaction = "api/v3/{0}/tx/send";
        public static readonly string getTx = "api/v3/{0}/tx/{1}";
        public static readonly string getRawtx = "api/v3/{0}/rawtx/{1}";
        public static readonly string getUtxosByAnAddress = "api/v3/{0}/addr/{1}/utxo";
        public static readonly string getAddressInfo = "api/v3/{0}/addr/{1}";
        public static readonly int maxLengthOfOpReturnData = 100000;
        public static readonly string opReturnType = "nulldata";
        public static readonly int opReturnLength = 4;
        public static Encoding encoding = new UTF8Encoding();



    }
}

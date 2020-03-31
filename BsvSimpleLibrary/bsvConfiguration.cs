using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BsvSimpleLibrary
{
    public class bsvConfiguration_class
    {
        public static readonly string RestApiUri = "https://api.whatsonchain.com/";
        public static readonly string testNetwork = "test";
        public static readonly string mainNetwork = "main";
        public static readonly string NbitTestNet = "TestNet";
        public static readonly string NbitMainNet = "Main";
        public static readonly string sendRawTransaction = "v1/bsv/{0}/tx/raw";
        public static readonly string getTx = "v1/bsv/{0}/tx/hash/{1}";
        public static readonly string getTxs = "v1/bsv/{0}/txs";
        public static readonly string getRawtx = "v1/bsv/{0}/tx/{1}/hex";
        public static readonly string getUtxosByAnAddress = "v1/bsv/{0}/address/{1}/unspent";
        public static readonly string getAddressHistory = "v1/bsv/{0}/address/{1}/history ";
        public static readonly int maxLengthOfOpReturnData = 100000;
        public static readonly string opReturnType = "nulldata";
        //public static readonly int opReturnLength = 4;
        public static Encoding encoding = new UTF8Encoding();

    }
}

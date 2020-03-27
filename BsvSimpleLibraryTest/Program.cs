using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using NBitcoin.DataEncoders;
using BsvSimpleLibrary;
using NBitcoin;

namespace bsv
{
    
    class Program
    {
        static void Main(string[] args)
        {
            string destAddress = "mqpWc9BCjbQvj5DyEZCyRrF9X3X9r8iMWJ";//test
            string privateKey = ""; //(test) your private key
            string txid = "b83711fbef90ae6a509741d859f77b5fea6b419abdd9ee9b818efa0af5c86b63";
            string uri = bsvConfiguration_class.bitindexUri;
            string network = bsvConfiguration_class.testNetwork;
            string opReturnData = "bsv test";

            Dictionary<string, string> response;

            ////send bsv and / or write data.
            //response = bsvTransaction_class.send(privateKey, 0, network, null, null, opReturnData, 1, 0);

            ////get opreturn data
            ////byte[] bytes = BitIndex_class.getOpReturnData(uri, network, txid);
            //string s = BitIndex_class.getOpReturnData(uri, network, txid, bsvConfiguration_class.encoding);

            ////get tx
            //BitIndexTransaction tx = BitIndex_class.getTransaction(uri, network, txid);

            ////get utxo
            //BitIndexUtxo_class[] utxos = BitIndex_class.getUtxosByAnAddress(uri, network, destAddress);

            ////get BSV price based on USDT from OKEX
            //double price = BsvPrice_class.getBsvPriceOnUSDT();
            //double priceOnSat = BsvPrice_class.getSatoshiPriceOnCent();

            /*Unavailable at present. More functions will be listed on furture version.
            ////get address Info
            //BitIndexAddressInfo addrInfo = BitIndex_class.getAddressInfo(uri, network, destAddress);
            ////get raw tx
            //response = BitIndex_class.getRawTransaction(uri, network, txid);
            */


            Console.WriteLine("press any key to exist");
            Console.ReadKey();

        }
    }
}

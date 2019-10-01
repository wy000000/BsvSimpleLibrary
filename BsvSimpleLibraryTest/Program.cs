﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
//using Newtonsoft.Json;
//using NBitcoin;
using BsvSimpleLibrary;

namespace bsv
{
    class Program
    {
        static void Main(string[] args)
        {
            string destAddress = "n3Ro8jRU3MNmdhL9KNRkCT6ikQxT26iPaR";//test
            string privateKey = ""; //test
            string txid = "d6ad0a5fe9f4b7187641b8d9fa9d49754395d338d3fc07887c606a26415a961f";
            string uri = bsvConfiguration_class.bitindexUri;
            string network = bsvConfiguration_class.testNetwork;

            Dictionary<string, string> response;

            //send bsv and / or write data.
            response = bsvTransaction_class.send(privateKey, 1000, network, destAddress, null, "bsv test");

            //get opreturn data
            //byte[] bytes = BitIndex_class.getOpReturnData(uri, network, txid);
            string s = BitIndex_class.getOpReturnData(uri, network, txid, bsvConfiguration_class.encoding);

            //get raw tx
            response = BitIndex_class.getRawTransaction(uri, network, txid);

            //get tx
            BitIndexTransaction tx = BitIndex_class.getTransaction(uri, network, txid);

            //get address Info
            BitIndexAddressInfo addrInfo = BitIndex_class.getAddressInfo(uri, network, destAddress);

            //get utxo
            BitIndexUtxo_class[] utxos = BitIndex_class.getUtxosByAnAddress(uri, network, destAddress);

            Console.WriteLine("press any key to exist");
            Console.ReadKey();

        }
    }
}
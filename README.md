# BsvSimpleLibrary
This is a simple example to send bsv satoshis to a destination address from a source address and/or write/read data to/from BSV blockchain. 
bitcoinSV. 
BSV. 
If changeBackAddress is null, the sending address would be set as default changeBackAddress. 
Set the "donationSatoshi"= 0 if do not donate. 
If success, return the txid; else return error information. 

Ver0.26 fixed data read/write errors caused by dependency package upgrades. Be compatible with Nbitcoin 10.0.3 and NBitcoin.Altcoins 6.0.3.
Ver 0.25 Adapted RestApi_class.getUtxosByAnAddress() to align with the updated whatsonchain.com  REST API schema.
Ver 0.24 fixed another bug of getOpReturnData function.
Ver 0.23 fixed a bug of getOpReturnData function.
Ver 0.22 is updated to be compatible with Nbitcoin 5.0.76. 
Ver 0.21 added a tx sending function to pass the sent tx, tx fee and donation fee out. 
Ver 0.20 fixed a bug of getOpReturnData function. 
Ver 0.19, the omitted getOpReturnData(RestApiTransaction tx, Encoding encoder) function is re-added. 
Ver 0.18 is updated OP_Return to OP_False Op_Return. 
Ver 0.17 is added with get address history and txs. function names are changed. 
Ver 0.16 is moved to whatsonchain.com since RestApi.network is unavailable. More functions will be listed in future version. 
ver 0.1.5. Bsv price is added. It is from OKEx based on USDT. 
AES is deleted. It is move to BitcoinSVCryptor library. Both of them are not compatible.

            string destAddress = "mqpWc9BCjbQvj5DyEZCyRrF9X3X9r8iMWJ";//test            
            string privateKey = ""; //(test) your private key
            string txid = "d45bdda15e197e068288012f1764fd10cf884f5befcafb7d545af55f9d6e9cf0";
            txid = "fa20def75284a01d033eb5d140f97f86a1df793d380e38c469b792c3b1083721";

            string uri = bsvConfiguration_class.RestApiUri;
            string network = bsvConfiguration_class.testNetwork;
            string opReturnData = "bsv test";

            Dictionary<string, string> response;

            ////send bsv and / or write data.
            //response = bsvTransaction_class.send(privateKey, 0, network, null, null, opReturnData, 1, 0);
            //Transaction tx = null;
            //long txfee = 0;
            //long donationFee = 0;
            //response = bsvTransaction_class.send(privateKey, 0, network, out tx, out txfee, out donationFee,
            //    null, null, opReturnData, 1, 0);
            //Console.WriteLine("tx fee: " + txfee);

            //////get opreturn data
            ////byte[] bytes = RestApi_class.getOpReturnFullData(uri, network, txid);
            //string s = RestApi_class.getOpReturnData(uri, network, txid, bsvConfiguration_class.encoding);

            ////get tx
            //RestApiTransaction tx = RestApi_class.getTransaction(uri, network, txid);

            ////get raw tx
            //string rawTx = RestApi_class.getRawTransaction(uri, network, txid);
            //byte[] bytes = new HexEncoder().DecodeData(rawTx);

            ////get utxo
            //RestApiUtxo_class[] utxos = RestApi_class.getUtxosByAnAddress(uri, network, destAddress);

            ////get address Info
            //RestApiAddressHistoryTx[] addrHistory = RestApi_class.getAddressHistory(uri, network, destAddress);

            ////get addresses Info
            //string[] addresses = { "miTguHu7Q9Zhpfhsi3BbefL2bG1sNSdhSf", destAddress };
            //RestApiAddressHistoryTx[] addressesHistory = RestApi_class.getAddressesHistory(uri, network, addresses);

            ////get txs. Max 20 transactions per request
            //string[] txHashs ={"fa20def75284a01d033eb5d140f97f86a1df793d380e38c469b792c3b1083721",
            //	"21b3b70f51bee8882fa40778a6fc68eab33239f20b01a559f110c2ba229f8c98",
            //	"fc4471fb3761da4cc317b09a4fae5a68a11f8db41e703cd75125310f39a975fc" };
            //RestApiTransaction[] txs = RestApi_class.getTransactions(uri, network, txHashs);
            //foreach (RestApiTransaction tx in txs)
            //{
            //	string s = RestApi_class.getOpReturnData(tx, bsvConfiguration_class.encoding);
            //	Console.WriteLine(s);
            //}

            /*Unavailable at present.
            ////get BSV price based on USDT from OKEX
            //double price = BsvPrice_class.getBsvPriceOnUSDT();
            //double priceOnSat = BsvPrice_class.getSatoshiPriceOnCent();
            */

https://www.nuget.org/packages/BsvSimpleLibrary/

12Nu5u5pgP7XvFGj31t71cdsmmWR7W2f83

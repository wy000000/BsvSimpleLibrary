# BsvSimpleLibrary
This is a simple example to Send bsv satoshis to an address from an address and/or write/read data to/from BSV blockchain. 
If changeBackAddress is null, the sending address would be set as default changeBackAddress. 
Set the "donationSatoshi"= 0 if do not donate. 
If success, return the txid; else return error information. 

ver 0.16 is moved to whatsonchain.com since bitindex.network is unavailable. More functions will be listed in future version.

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

Open BSV License.

https://www.nuget.org/packages/BsvSimpleLibrary/

12Nu5u5pgP7XvFGj31t71cdsmmWR7W2f83

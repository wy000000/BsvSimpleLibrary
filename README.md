# BsvSimpleLibrary
This is a simple example to Send bsv satoshis to an address from an address and/or write/read data to/from BSV blockchain. 
If changeBackAddress is null, the sending address would be set as default changeBackAddress. 
The fee would be set to 1.0x Sat/B automatically. 
Set the "donationSatoshi"= 0 if do not donate. 
If success, return the txid; else return error information. 

            string destAddress = "n3Ro8jRU3MNmdhL9KNRkCT6ikQxT26iPaR";//test
            string privateKey = ""; //(test) your private key
            string txid = "d6ad0a5fe9f4b7187641b8d9fa9d49754395d338d3fc07887c606a26415a961f";
            string uri = bsvConfiguration_class.RestApiUri;
            string network = bsvConfiguration_class.testNetwork;
            string opReturnData = "bsv test";

            Dictionary<string, string> response;

            ////send bsv and / or write data.
            //response = bsvTransaction_class.send(privateKey, 1000, network, destAddress, null, opReturnData);

            ////get opreturn data
            ////byte[] bytes = RestApi_class.getOpReturnData(uri, network, txid);
            //string s = RestApi_class.getOpReturnData(uri, network, txid, bsvConfiguration_class.encoding);

            ////get raw tx
            //response = RestApi_class.getRawTransaction(uri, network, txid);

            ////get tx
            //RestApiTransaction tx = RestApi_class.getTransaction(uri, network, txid);

            ////get address Info
            //RestApiAddressInfo addrInfo = RestApi_class.getAddressInfo(uri, network, destAddress);

            ////get utxo
            //RestApiUtxo_class[] utxos = RestApi_class.getUtxosByAnAddress(uri, network, destAddress);

            ////AES is moved to BitcoinSVCryptor library.
            
            ////get BSV price based on USDT from OKEX
            //double price = BsvPrice_class.getBsvPriceOnUSDT();
            //double priceOnSat = BsvPrice_class.getSatoshiPriceOnCent();

            Console.WriteLine("press any key to exist");
            Console.ReadKey();

Open BSV License.

https://www.nuget.org/packages/BsvSimpleLibrary/

18WrLbAU54S8N16jMHomkhqpMtkPHLh3Dv

# BsvSimpleLibrary
This is a simple example to Send bsv satoshis to an address from an address and/or write/read data to/from BSV blockchain. 
If changeBackAddress is null, the sending address would be set as default changeBackAddress. 
The fee would be set to 1.0x Sat/B automatically. 
Set the "donationSatoshi"= 0 if do not donate. 
If success, return the txid; else return error information. 

            string destAddress = "n3Ro8jRU3MNmdhL9KNRkCT6ikQxT26iPaR";//test
            string privateKey = ""; //test
            string txid = "d6ad0a5fe9f4b7187641b8d9fa9d49754395d338d3fc07887c606a26415a961f";
            string uri = bsvConfiguration_class.bitindexUri;
            string network = bsvConfiguration_class.testNetwork;
            string opReturnData = "bsv test";

            Dictionary<string, string> response;

            //send bsv and / or write data.
            response = bsvTransaction_class.send(privateKey, 1000, network, destAddress, null, opReturnData);

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
            
            //AES
            byte[] ciphers = AES_class.aesEncryptStringToBytes(opReturnData, privateKey);
            string plainStr = AES_class.aesDecryptStringFromBytes(ciphers, privateKey);

            Console.WriteLine("press any key to exist");
            Console.ReadKey();

Open BSV License.

https://www.nuget.org/packages/BsvSimpleLibrary/

18WrLbAU54S8N16jMHomkhqpMtkPHLh3Dv

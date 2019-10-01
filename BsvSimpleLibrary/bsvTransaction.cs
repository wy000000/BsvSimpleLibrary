using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBitcoin;
using NBitcoin.Altcoins;

namespace BsvSimpleLibrary
{
    public class bsvTransaction_class
    {
        /// <summary>
        /// Send bsv satoshis to an address from an address and/or write data to BSV blockchain. 
        /// If changeBackAddress is null, it would be set to the sending address automatically. 
        /// The fee would be set to 1.0x Sat/B automatically. 
        /// Set the "donationSatoshi" parameter to <1000 if do not want to donate. The minimum acceptable donation is 1000. 
        /// If success, return the txid; else return error information. 
        /// </summary>
        /// <param name="privateKeyStr"></param>
        /// <param name="sendSatoshi"></param>
        /// <param name="network"></param>
        /// <param name="destAddress"></param>
        /// <param name="changeBackAddress">If changeBackAddress is null, it would be set to the sending address automatically. </param>
        /// <param name="opreturnData">If opreturnData is not null, the data would be write to the blockchain.</param>
        /// <param name="donationSatoshi">Set the "donationSatoshi" parameter to 0 if do not want to donate.</param>
        /// <returns>If success, return the txid; else return error information</returns>
        public static Dictionary<string, string> send(string privateKeyStr, long sendSatoshi, string network,
            string destAddress = null, string changeBackAddress = null, string opreturnData = null, long donationSatoshi = 1000)
        {
            Dictionary<string, string> response = new Dictionary<string, string>();
            long donationSat = 0;
            if (donationSatoshi >= 1000)
                donationSat = donationSatoshi;
            BitcoinSecret privateKey = null;
            try { privateKey = new BitcoinSecret(privateKeyStr); }
            catch (FormatException e)
            {
                Console.WriteLine();
                Console.WriteLine(e.Message);
                return (null);
            }
            Network networkFlag = privateKey.Network;
            if (changeBackAddress == null)
                changeBackAddress = privateKey.GetAddress().ToString();
            Transaction tx = null;
            if (networkFlag.Name == bsvConfiguration_class.NbitTestNet)
            {
                tx = ForkIdTransaction.Create(NBitcoin.Altcoins.BCash.Instance.Testnet);
                if (network != bsvConfiguration_class.testNetwork)
                {
                    string err = string.Format("Error. the privake key is for {0}, but the selected network is for {1}.",
                        bsvConfiguration_class.NbitTestNet, network);
                    Console.WriteLine();
                    Console.WriteLine(err);
                    response.Add("Error.", err);
                    return (response);
                }
            }
            if (networkFlag.Name == bsvConfiguration_class.NbitMainNet)
            {
                tx = ForkIdTransaction.Create(NBitcoin.Altcoins.BCash.Instance.Mainnet);
                if (network != bsvConfiguration_class.mainNetwork)
                {
                    string err = string.Format("Error. the privake key is for {0}, but the selected network is for {1}.",
                        bsvConfiguration_class.NbitMainNet, network);
                    Console.WriteLine();
                    Console.WriteLine(err);
                    response.Add("Error.", err);
                    return (response);
                }
            }
            BitIndexUtxo_class[] utxos = BitIndex_class.getUtxosByAnAddress(bsvConfiguration_class.bitindexUri, network,
                privateKey.GetAddress().ToString());
            long balance;
            //long fee;
            long estimatedFee;
            long outSum;
            if (addin(sendSatoshi, tx, utxos, out balance, out estimatedFee,
                destAddress, changeBackAddress, opreturnData, donationSat))
            {
                outSum = addout(tx, opreturnData, destAddress, changeBackAddress, sendSatoshi,
                    balance, estimatedFee, donationSat, network);
                decimal feeRate = sign(tx, privateKeyStr, utxos, balance, sendSatoshi, donationSat);
                response = BitIndex_class.sendTransaction(bsvConfiguration_class.bitindexUri, network, tx.ToHex());
                return (response);
            }
            return (response);
        }
        static bool addin(long sendSatoshi, Transaction tx, BitIndexUtxo_class[] utxos, out long balance, out long estimatedFee,
            string destAddress, string changeBackAddress, string opreturnData, long donationSat)
        {           
            balance = 0;
            long neededSatoshi = sendSatoshi+donationSat;
            estimatedFee= 150;
            if (opreturnData != null)
                estimatedFee += opreturnData.Length;
            neededSatoshi += estimatedFee;
            foreach (BitIndexUtxo_class utxo in utxos)
            {
                OutPoint outPoint = new OutPoint(uint256.Parse(utxo.TxId), utxo.OutIndex);
                TxIn txin = new TxIn(outPoint);
                //txin.ScriptSig = Script.FromHex(utxo.ScriptPubKey);
                tx.Inputs.Add(txin);
                neededSatoshi += 150;
                estimatedFee += 150;
                balance += utxo.Satoshis;
                if (balance >= neededSatoshi)
                    return (true);
            }
            if (balance < neededSatoshi)
                return (false);
            return (true);
        }

        static long addout(Transaction tx, string opreturnData, string destAddress, string changeBackAddress,
            long sendSatoshi, long balance, long estimatedFee, long donationSat, string network)
        {
            long outSum = 0;
            if (destAddress != null)
            {
                BitcoinAddress outAddress = BitcoinAddress.Create(destAddress);
                TxOut txout = new TxOut(new Money(sendSatoshi), outAddress.ScriptPubKey);
                tx.Outputs.Add(txout);
                outSum += sendSatoshi;
            }
            if (donationSat >= 1000 && network == bsvConfiguration_class.mainNetwork)
            {
                BitcoinAddress outAddress = BitcoinAddress.Create("18WrLbAU54S8N16jMHomkhqpMtkPHLh3Dv");
                TxOut txout = new TxOut(new Money(donationSat), outAddress.ScriptPubKey);
                tx.Outputs.Add(txout);
                outSum += donationSat;
            }
            if (opreturnData!=null)
            {
                byte[] opretrunBytes = Encoding.UTF8.GetBytes(opreturnData);
                TxNullDataTemplate opreturnTemplate = new TxNullDataTemplate(bsvConfiguration_class.maxLengthOfOpReturnData);
                Script opreturnScript = opreturnTemplate.GenerateScriptPubKey(opretrunBytes);
                tx.Outputs.Add(new TxOut()
                {
                    Value = Money.Zero,
                    ScriptPubKey = opreturnScript
                });
            }            
            //put the change back address at last.
            if (changeBackAddress != null)
            {
                BitcoinAddress changeAddress = BitcoinAddress.Create(changeBackAddress);
                //fee = tx.ToBytes().Length + 50;
                long changeBackSatoshi = balance - sendSatoshi - estimatedFee - donationSat;
                TxOut txback = new TxOut(new Money(changeBackSatoshi), changeAddress.ScriptPubKey);
                tx.Outputs.Add(txback);
                outSum += changeBackSatoshi;
            }
            return (outSum);
        }

        static decimal sign(Transaction tx, string privateKeyStr, BitIndexUtxo_class[] utxos, long balance, long sendSatoshi, long donationSat)
        {
            //the change back address must be at last.
            List<Coin> coinList = new List<Coin>();
            foreach(BitIndexUtxo_class utxo in utxos)
                coinList.Add(new Coin(uint256.Parse(utxo.TxId), utxo.OutIndex, new Money(utxo.Satoshis),
                    Script.FromHex(utxo.ScriptPubKey)));
            BitcoinSecret privateKey = new BitcoinSecret(privateKeyStr);
            Coin[] coins = coinList.ToArray();
            tx.Sign(privateKey, coins);
            //reSign to adjust fee
            //the change back address must be at last.
            long fee = tx.ToBytes().Length + 20;
            long changBackSatoshi = balance - sendSatoshi - fee - donationSat;
            tx.Outputs.Last().Value = changBackSatoshi;
            tx.Sign(privateKey, coins);
            decimal feeRate = (decimal)fee / tx.ToBytes().Length;
            Console.WriteLine();
            Console.WriteLine("fee rate: {0}", feeRate);
            return (feeRate);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using NBitcoin;
using NBitcoin.Altcoins;
//using System.Runtime.InteropServices;

namespace BsvSimpleLibrary
{
    public class bsvTransaction_class
    {
        //[DllImport("KERNEL32")]
        //static extern bool QueryPerformanceCounter(out long lpPerformanceCount);
        /// <summary>
        /// Send bsv satoshis to an address from an address and/or write/read data to/from BSV blockchain. 
        /// If changeBackAddress is null, the sending address would be set as default changeBackAddress.. 
        /// The fee would be set to 1.0x Sat/B automatically. 
        /// Set the "donationSatoshi" =0 if do not donate.  
        /// If success, return the txid; else return error information. 
        /// </summary>
        /// <param name="privateKeyStr"></param>
        /// <param name="sendSatoshi"></param>
        /// <param name="network"></param>
        /// <param name="destAddress"></param>
        /// <param name="changeBackAddress">If changeBackAddress is null, it would be set to the sending address automatically. </param>
        /// <param name="opreturnData">If opreturnData is not null, the data would be write to the blockchain.</param>
        /// <param name="feeSatPerByte">fee rate is represented by Satoshis per Byte</param>
        /// <param name="donationSatoshi">Set the "donationSatoshi" parameter = 0 if do not donate.</param>
        /// <returns>If success, return the txid; else return error information</returns>
        public static Dictionary<string, string> send(string privateKeyStr, long sendSatoshi, string network,
            string destAddressStr = null, string changeBackAddressStr = null,
            string opreturnData = null, double feeSatPerByte = 1, long donationSatoshi = 100)
        {
            Dictionary<string, string> response = new Dictionary<string, string>();
            long donationSat = setDonationSatoshi(donationSatoshi);
            ///////////////////////////
            BitcoinSecret privateKey = null;
            try { privateKey = new BitcoinSecret(privateKeyStr); }
            catch (FormatException e)
            {
                Console.WriteLine();
                Console.WriteLine(e.Message);
                response.Add("Error", e.Message);
                return (response);
            }
            Network networkFlag = privateKey.Network;
            BitcoinAddress destAddress = null;
            if (destAddressStr == null && sendSatoshi > 0)
            {
                string err = " the destAddress is null, but the sendSatoshi is not 0";
                Console.WriteLine();
                Console.WriteLine("Error: " + err);
                response.Add("Error", err);
                return (response);
            }
            if (destAddressStr != null)
                destAddress = BitcoinAddress.Create(destAddressStr);
            BitcoinAddress changeBackAddress = null;
            if (changeBackAddressStr == null)
                changeBackAddress = privateKey.GetAddress();
            else
                changeBackAddress = BitcoinAddress.Create(changeBackAddressStr);
            ////////////////////////////////////////
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
            Script scriptPubKey = privateKey.ScriptPubKey;
            ////////////////////////////
            RestApiUtxo_class[] utxos = RestApi_class.getUtxosByAnAddress(bsvConfiguration_class.RestApiUri, network,
                privateKey.GetAddress().ToString());
            //long balance;
            //long fee;
            //long estimatedFee;
            //long outSum;
            addout(tx, opreturnData, destAddress, changeBackAddress, sendSatoshi, donationSat, network);
            long changeBacksats = addin(sendSatoshi, tx, utxos, donationSat, feeSatPerByte, scriptPubKey);
            sign(tx, privateKeyStr, utxos, changeBacksats, scriptPubKey);
            string responseStr = RestApi_class.sendTransaction(bsvConfiguration_class.RestApiUri, network, tx.ToHex());
            response.Add("send info", responseStr);
            //if (addin(sendSatoshi, tx, utxos, out balance, out estimatedFee,
            //    destAddress, changeBackAddress, opreturnData, donationSat))
            //{
            //    addout(tx, opreturnData, destAddress, changeBackAddress, sendSatoshi,
            //        balance, estimatedFee, donationSat, network);
            //    sign(tx, privateKeyStr, utxos, balance, sendSatoshi, donationSat, feeSatPerByte);
            //    response = RestApi_class.sendTransaction(bsvConfiguration_class.RestApiUri, network, tx.ToHex());
            //    return (response);
            //}
            return (response);
        }
        static long setDonationSatoshi(long donationSatoshi)
        {
            if (donationSatoshi == 0 || donationSatoshi >= 1000)
                return (donationSatoshi);
            else
            {
                //long l;
                //QueryPerformanceCounter(out l);                
                DateTime dt = DateTime.Now;
                int seed = (int)dt.Ticks;
                //Console.WriteLine("tick=" + seed);
                Random rand = new Random(seed);
                int v = rand.Next(1000 + (int)donationSatoshi);
                //Thread.Sleep(1);
                //Console.WriteLine("v=" + v);
                if (v >= 1000)
                    return (v);
                else
                    return (0);
            }
        }
        /// <summary>
        /// return satoshis of chang back
        /// </summary>
        /// <param name="sendSatoshi"></param>
        /// <param name="tx"></param>
        /// <param name="utxos"></param>
        /// <param name="donationSat"></param>
        /// <param name="feeSatPerByte"></param>
        /// <returns></returns>
        static long addin(long sendSatoshi, Transaction tx, RestApiUtxo_class[] utxos, long donationSat,
            double feeSatPerByte, Script scriptPubKey)
        {
            long satsInTxInputs = 0;
            long neededSatoshi = sendSatoshi + donationSat;
            long fee = 0;
            //long estimatedFee = 150;
            //if (opreturnData != null)
            //    neededSatoshi += opreturnData.Length;
            //estimatedFee += opreturnData.Length;            
            foreach (RestApiUtxo_class utxo in utxos)
            {
                OutPoint outPoint = new OutPoint(uint256.Parse(utxo.TxId), utxo.OutIndex);
                TxIn txin = new TxIn(outPoint);
                txin.ScriptSig = scriptPubKey;//Script.FromHex(utxo.ScriptPubKey);
                tx.Inputs.Add(txin);
                //neededSatoshi += 150;
                //estimatedFee += 150;
                satsInTxInputs += utxo.Value;
                fee = getTxFee(tx, feeSatPerByte);
                neededSatoshi += fee;
                if (satsInTxInputs >= neededSatoshi)
                    break;
            }
            //if (satsInTxInputs < neededSatoshi)
            //    return (false);
            long changBackSatoshi = satsInTxInputs - sendSatoshi - fee - donationSat;
            Console.WriteLine();
            Console.WriteLine("fee : {0}", fee);
            return (changBackSatoshi);
        }
        static long getTxFee(Transaction tx, double feeSatPerByte)
        {
            double feeDouble = (tx.ToBytes().Length + tx.Inputs.Count * 81) * feeSatPerByte;
            long fee = Convert.ToInt64(Math.Ceiling(feeDouble));
            return (fee);
        }
        static void addout(Transaction tx, string opreturnData,
            BitcoinAddress destAddress, BitcoinAddress changeBackAddress,
            long sendSatoshi, long donationSat, string network)
        {
            //long outSum = 0;
            if (destAddress != null)
            {
                //BitcoinAddress outAddress = BitcoinAddress.Create(destAddress);
                TxOut txout = new TxOut(new Money(sendSatoshi), destAddress.ScriptPubKey);
                tx.Outputs.Add(txout);
                //outSum += sendSatoshi;
            }
            if (donationSat >= 1000 && network == bsvConfiguration_class.mainNetwork)
            {
                BitcoinAddress outAddress = BitcoinAddress.Create("199Kjhv6PLS8xn61y2fmJjvun2XwqA1UMm");
                TxOut txout = new TxOut(new Money(donationSat), outAddress.ScriptPubKey);
                tx.Outputs.Add(txout);
                //outSum += donationSat;
            }
            if (opreturnData != null)
            {
                byte[] strBytes = Encoding.UTF8.GetBytes(opreturnData);
                byte[] opretrunBytes = new byte[] { 0, 106 }.Concat(strBytes).ToArray();
                Script opreturnScript = new Script(opretrunBytes);
                //TxNullDataTemplate opreturnTemplate = new TxNullDataTemplate(bsvConfiguration_class.maxLengthOfOpReturnData);
                //opreturnScript = opreturnTemplate.GenerateScriptPubKey(opretrunBytes);
                tx.Outputs.Add(new TxOut()
                {
                    Value = Money.Zero,
                    ScriptPubKey = opreturnScript
                });
            }
            //put the change back address at last.
            //BitcoinAddress changeAddress = BitcoinAddress.Create(changeBackAddress);
            //fee = tx.ToBytes().Length + 50;
            //long changeBackSatoshi = balance - sendSatoshi - estimatedFee - donationSat;
            TxOut txback = new TxOut(new Money(0), changeBackAddress.ScriptPubKey);
            tx.Outputs.Add(txback);
            //outSum += changeBackSatoshi;            
            //return (outSum);
        }
        static void sign(Transaction tx, string privateKeyStr, RestApiUtxo_class[] utxos, long changeBackSatoshi, 
            Script scriptPubKey)
            //long satsInTxInputs, long sendSatoshi, long donationSat, double feeSatPerByte)
        {
            //the change back address must be at last.
            List<Coin> coinList = new List<Coin>();
            foreach (RestApiUtxo_class utxo in utxos)
                coinList.Add(new Coin(uint256.Parse(utxo.TxId), utxo.OutIndex, new Money(utxo.Value), scriptPubKey));
            BitcoinSecret privateKey = new BitcoinSecret(privateKeyStr);
            Coin[] coins = coinList.ToArray();
            //tx.Sign(privateKey, coins);
            //reSign to adjust fee
            //the change back address must be at last.  
            //double feeDouble = tx.ToBytes().Length * feeSatPerByte;
            //long fee = Convert.ToInt64(Math.Ceiling(feeDouble));
            //long changBackSatoshi = satsInTxInputs - sendSatoshi - fee - donationSat;
            tx.Outputs.Last().Value = changeBackSatoshi;
            tx.Sign(privateKey, coins);
            //decimal feeRate = (decimal)fee / tx.ToBytes().Length;
            //Console.WriteLine();
            //Console.WriteLine("fee : {0}", fee);
            //return (fee);
        }
    }
}

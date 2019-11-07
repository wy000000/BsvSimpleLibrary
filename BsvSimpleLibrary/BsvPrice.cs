using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OKExSDK;
using OKExSDK.Models.Spot;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


namespace BsvSimpleLibrary
{
    public class BsvPrice_class
    {
        public static double getBsvPriceOnUSDT()
        {
            Task<JObject> t = OKExSpotApi.getTickerByInstrumentIdAsync("BSV-USDT");
            JObject b = t.Result;
            SpotTicker bsv = JsonConvert.DeserializeObject<SpotTicker>(t.Result.ToString());
            double price = double.Parse(bsv.last);
            Console.WriteLine(price+" USDT/Bsv");
            return (price);
        }
        public static double getSatoshiPriceOnCent()
        {
            double price = getBsvPriceOnUSDT();
            double priceSatOnCent = price / 1000000;
            Console.WriteLine(priceSatOnCent+" Cent/Sat");
            return (priceSatOnCent);
        }
    }
}

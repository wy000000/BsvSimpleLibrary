using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OKExSDK.Models.Spot;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OKExSDK
{
    class OKExSpotApi
    {
        readonly static string BASEURL = "https://www.okex.com/";
        readonly static string SPOT_SEGMENT = "api/spot/v3";

        ///// <summary>
        ///// 获取币对信息
        ///// </summary>
        ///// <returns></returns>
        //public async Task<JContainer> getInstrumentsAsync()
        //{
        //    var url = $"{this.BASEURL}{this.SPOT_SEGMENT}/instruments";
        //    using (var client = new HttpClient())
        //    {
        //        var res = await client.GetAsync(url);
        //        var contentStr = await res.Content.ReadAsStringAsync();
        //        if (contentStr[0] == '[')
        //        {
        //            return JArray.Parse(contentStr);
        //        }
        //        return JObject.Parse(contentStr);
        //    }
        //}

        /// <summary>
        /// 获取某个ticker信息
        /// </summary>
        /// <param name="instrument_id">币对</param>
        /// <returns></returns>
        public static async Task<JObject> getTickerByInstrumentIdAsync(string instrument_id)
        {
            var url = $"{BASEURL}{SPOT_SEGMENT}/instruments/{instrument_id}/ticker";
            using (var client = new HttpClient())
            {
                var res = await client.GetAsync(url);
                var contentStr = await res.Content.ReadAsStringAsync();
                return JObject.Parse(contentStr);
            }
        }
    }    
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockHq.Entities
{
    public class HisStockHq
    {
        [JsonProperty("status")]
        public int Status;

        [JsonProperty("hq")]
        public string[][] Hq;

        [JsonProperty("code")]
        public string Code;

        [JsonProperty("stat")]
        public object[] Stat;
    }
}

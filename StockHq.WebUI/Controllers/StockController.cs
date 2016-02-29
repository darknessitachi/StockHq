using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flurl.Http;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data.SqlClient;
using System.Threading.Tasks;
using StockHq.Entities;
using Newtonsoft.Json;
using StockHq.Core.Util;

namespace StockHq.WebUI.Controllers
{
    public class StockController : Controller
    {
        public async Task<ActionResult> Index()
        {
            //删除老数据
            var executeNum = await new SqlConnection(DBSetting.StockHq).ExecuteAsync(@"DELETE FROM StockHq");
            //http://q.stock.sohu.com/hisHq?code=cn_002055&start=20160226&end=20160226&stat=1&order=D&period=d&callback=historySearchHandler&rt=jsonp
            var dict = new Dictionary<string, string>();
            dict.Add("首旅酒店", "600258");
            dict.Add("恒生电子", "600570");
            dict.Add("得润电子", "002055");
            dict.Add("世纪星源", "000005");
            foreach (var item in dict)
            {
                string url = string.Format(@"http://q.stock.sohu.com/hisHq?code=cn_{0}&start=20160101&end=20160226&stat=1&order=D&period=d&callback=historySearchHandler&rt=jsonp", item.Value);
                var data = await url.GetStringAsync();
                var stock = JsonConvert.DeserializeObject<List<Stock>>(data.Replace(@"historySearchHandler(", "").Replace(")", "").Replace("%", ""));
                if (stock.Count() < 0 || stock[0].Status != 0)
                {
                    return Json(new { IsError = true, Msg = string.Empty, Data = string.Empty });
                }

                for (int j = 0; j < stock[0].Hq.Count(); j++)
                {
                    await new SqlConnection(DBSetting.StockHq).InsertAsync(new StockHq.Entities.StockHq
                    {
                        Code = item.Value,
                        Name = item.Key,
                        Date = stock[0].Hq[j][0].ToString(),
                        Open = decimal.Parse(stock[0].Hq[j][1]),
                        Close = decimal.Parse(stock[0].Hq[j][2]),
                        Highest = decimal.Parse(stock[0].Hq[j][6]),
                        Lowest = decimal.Parse(stock[0].Hq[j][5]),
                        Change = decimal.Parse(stock[0].Hq[j][3]),
                        Chg = decimal.Parse(stock[0].Hq[j][4]),
                        Rate = decimal.Parse(stock[0].Hq[j][9]),
                        Volume = int.Parse(stock[0].Hq[j][7]),
                        Turnover = decimal.Parse(stock[0].Hq[j][8])
                    });
                }
            }
            return View();
        }


    }
}
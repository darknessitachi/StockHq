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
    /// <summary>
    /// 抓取交易数据
    /// </summary>
    public class StockController : Controller
    {
        /// <summary>
        /// 获得历史交易数据
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            try
            {
                #region 查询所有股票
                var stocks = await new SqlConnection(DBSetting.StockHq).QueryAsync<Stocks>("SELECT *FROM Stocks").ContinueWith(t => t.Result.ToList());
                if (stocks.Count() <= 0)
                {
                    return Json(new { IsError = true, Msg = string.Empty, Data = string.Empty });
                }
                #endregion

                #region  获取连续下跌天数
                /*

                            {
                                "stkc": "600876",
                                "stkn": "洛阳玻璃",
                                "cp": "27.23",
                                "pl": "-29.820051",
                                "ta": "466893",
                                "tr": "18.674227",
                                "day": "3",
                                "ps": "-8.12",
                                "isup": "0",
                                "qst": "2016-02-25",
                                "jst": "2016-02-29"
                            }
            */
                foreach (var item in stocks)
                {
                    var lowHq = await string.Format(@"http://hqquery.jrj.com.cn/alxzd.do?sort=day&page=1&size=20&order=desc&isup=0&ids={0}&_dc=1456802790072", item.Code).WithTimeout(15).GetStringAsync();
                    var lowHqArrs = lowHq.Remove(0, lowHq.IndexOf('[') + 5).Replace("]\r\n}", "").Replace("{", "").Replace("}", "").Replace(@"\", "").Split(',');
                    if (lowHqArrs.Count() < 10)
                    {
                        continue;
                    }
                    await new SqlConnection(DBSetting.StockHq).ExecuteAsync("UPDATE Stocks SET lowdays= @lowdays ,lowchg=@lowchg ,lowclose=@lowclose,lowrate=@lowrate,lowvolume=@lowvolume,lowturnover=@lowturnover,lowbegin=@lowbegin,lowend=@lowend WHERE code=@code", new
                    {
                        lowdays = int.Parse(lowHqArrs[6].Split(':')[1]),
                        lowchg = decimal.Parse(lowHqArrs[3].Split(':')[1]),
                        lowclose = decimal.Parse(lowHqArrs[2].Split(':')[1]),
                        lowrate = decimal.Parse(lowHqArrs[5].Split(':')[1]),
                        lowvolume = int.Parse(lowHqArrs[4].Split(':')[1]),
                        lowturnover = decimal.Parse(lowHqArrs[7].Split(':')[1]),
                        lowbegin = lowHqArrs[9].Split(':')[1],
                        lowend = lowHqArrs[10].Split(':')[1],
                        code = item.Code
                    });
                }
                #endregion

                #region  获取连续上涨天数
                foreach (var item in stocks)
                {
                    var highHq = await string.Format(@"http://hqquery.jrj.com.cn/alxzd.do?sort=day&page=1&size=20&order=desc&isup=1&ids={0}&_dc=1456904310757", item.Code).WithTimeout(15).GetStringAsync();
                    var highHqArrs = highHq.Remove(0, highHq.IndexOf('[') + 5).Replace("]\r\n}", "").Replace("{", "").Replace("}", "").Replace(@"\", "").Split(',');
                    if (highHqArrs.Count() < 10)
                    {
                        continue;
                    }
                    await new SqlConnection(DBSetting.StockHq).ExecuteAsync("UPDATE Stocks SET highdays= @highdays ,highchg=@highchg ,highclose=@highclose,highrate=@highrate,highvolume=@highvolume,highturnover=@highturnover,highbegin=@highbegin,highend=@highend WHERE code=@code", new
                    {
                        highdays = int.Parse(highHqArrs[6].Split(':')[1]),
                        highchg = decimal.Parse(highHqArrs[3].Split(':')[1]),
                        highclose = decimal.Parse(highHqArrs[2].Split(':')[1]),
                        highrate = decimal.Parse(highHqArrs[5].Split(':')[1]),
                        highvolume = int.Parse(highHqArrs[4].Split(':')[1]),
                        highturnover = decimal.Parse(highHqArrs[7].Split(':')[1]),
                        highbegin = highHqArrs[9].Split(':')[1],
                        highend = highHqArrs[10].Split(':')[1],
                        code = item.Code
                    });
                }
                #endregion

                #region 删除老数据
                var executeNum = await new SqlConnection(DBSetting.StockHq).ExecuteAsync(@"DELETE FROM StockHq");
                #endregion

                #region 获得历史交易数据 [按日]
                foreach (var item in stocks)
                {
                    string url = string.Format(@"http://q.stock.sohu.com/hisHq?code=cn_{0}&start={1}&end={2}&stat=1&order=D&period=d&callback=historySearchHandler&rt=jsonp", item.Code, DateTime.Now.AddDays(-30).ToString("yyyyMMdd"), DateTime.Now.ToString("yyyyMMdd"));
                    var data = await url.WithTimeout(15).GetStringAsync();
                    if (data.Length < 30)
                    {
                        continue;
                    }
                    var stock = JsonConvert.DeserializeObject<List<HisStockHq>>(data.Replace(@"historySearchHandler(", "").Replace(")", "").Replace("%", ""));
                    if (stock.Count() < 0 || stock[0].Status != 0)
                    {
                        continue;
                        //return Json(new { IsError = true, Msg = string.Empty, Data = string.Empty });
                    }
                    for (int j = 0; j < stock[0].Hq.Count(); j++)
                    {
                        await new SqlConnection(DBSetting.StockHq).InsertAsync(new StockHq.Entities.StockHq
                        {
                            StockId = item.Id,
                            Date = stock[0].Hq[j][0].ToString(),
                            Open = decimal.Parse(stock[0].Hq[j][1]),
                            Close = decimal.Parse(stock[0].Hq[j][2]),
                            Highest = decimal.Parse(stock[0].Hq[j][6]),
                            Lowest = decimal.Parse(stock[0].Hq[j][5]),
                            Change = decimal.Parse(stock[0].Hq[j][3]),
                            Chg = decimal.Parse(stock[0].Hq[j][4]),
                            Rate = decimal.Parse(stock[0].Hq[j][9]),
                            Volume = int.Parse(stock[0].Hq[j][7]),
                            Turnover = decimal.Parse(stock[0].Hq[j][8]),
                            Type = "day"
                        });
                    }
                }
                #endregion

                #region 获得历史交易数据 [按周]
                foreach (var item in stocks)
                {
                    string url = string.Format(@"http://q.stock.sohu.com/hisHq?code=cn_{0}&start={1}&end={2}&stat=1&order=D&period=w&callback=historySearchHandler&rt=jsonp&r=0.6330537682505002&0.07230534508623476", item.Code, DateTime.Now.AddDays(-90).ToString("yyyyMMdd"), DateTime.Now.ToString("yyyyMMdd"));
                    var data = await url.WithTimeout(15).GetStringAsync();
                    if (data.Length < 30)
                    {
                        continue;
                    }
                    var stock = JsonConvert.DeserializeObject<List<HisStockHq>>(data.Replace(@"historySearchHandler(", "").Replace(")", "").Replace("%", ""));
                    if (stock.Count() < 0 || stock[0].Status != 0)
                    {
                        continue;
                    }
                    for (int j = 0; j < stock[0].Hq.Count(); j++)
                    {
                        await new SqlConnection(DBSetting.StockHq).InsertAsync(new StockHq.Entities.StockHq
                        {
                            StockId = item.Id,
                            Date = stock[0].Hq[j][0].ToString(),
                            Open = decimal.Parse(stock[0].Hq[j][1]),
                            Close = decimal.Parse(stock[0].Hq[j][2]),
                            Highest = decimal.Parse(stock[0].Hq[j][6]),
                            Lowest = decimal.Parse(stock[0].Hq[j][5]),
                            Change = decimal.Parse(stock[0].Hq[j][3]),
                            Chg = decimal.Parse(stock[0].Hq[j][4]),
                            Rate = decimal.Parse(stock[0].Hq[j][9]),
                            Volume = int.Parse(stock[0].Hq[j][7]),
                            Turnover = decimal.Parse(stock[0].Hq[j][8]),
                            Type = "week"
                        });
                    }
                }
                #endregion

                return Content("success");
            }
            catch (Exception)
            {
                return Content("fail");
            }
        }
    }
}
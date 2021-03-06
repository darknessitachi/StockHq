﻿using StockHq.Core.Util;
using StockHq.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Dapper;

namespace StockHq.WebUI.Controllers
{
    /// <summary>
    /// 行情数据
    /// </summary>
    public class HqController : Controller
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            return View();
        }

        /// <summary>
        /// 获得数据
        /// </summary>
        /// <param name="jtStartIndex"></param>
        /// <param name="jtPageSize"></param>
        /// <param name="jtSorting"></param>
        /// <returns></returns>

        public async Task<ActionResult> GetStockHqAsync(int jtStartIndex = 0, int jtPageSize = 3000, string jtSorting = "lowend desc,lowdays desc ,lowchg desc")
        {
            try
            {
                /*
                   string cmdText = @"SELECT * FROM(SELECT ROW_NUMBER() OVER (ORDER BY @jtSorting) AS Num, * FROM Stocks) AS A
                                     WHERE Num > @beginSize AND Num <= @endSize";
                  var stocks = await new SqlConnection(DBSetting.StockHq).QueryAsync<Stocks>(cmdText, new
                  {
                      beginSize = jtStartIndex,
                      endSize = jtStartIndex + jtPageSize,
                      jtSorting = jtSorting
                  }).ContinueWith(t => t.Result.ToList());
                 */
                string cmdText = @"SELECT B.[NearDate],b.stockid,A.* FROM(SELECT ROW_NUMBER() OVER(ORDER BY " + jtSorting + @") AS Num, * FROM Stocks) AS A
                                   INNER JOIN(SELECT DISTINCT stockid, MAX([date]) AS NearDate FROM StockHq
                                   GROUP BY  stockid) AS B ON A.ID=B.stockid WHERE Num > @beginSize AND Num <= @endSize ORDER BY " + jtSorting;
                var stocks = await new SqlConnection(DBSetting.StockHq).QueryAsync<Stocks>(cmdText, new
                {
                    beginSize = jtStartIndex,
                    endSize = jtStartIndex + jtPageSize,
                    jtSorting = jtSorting
                }).ContinueWith(t => t.Result.ToList());
                if (stocks == null)
                {
                    return Json(new { IsError = true, Msg = string.Empty, Data = string.Empty });
                }
                return Json(new { Result = "OK", Records = stocks });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        /// <summary>
        /// 日交易详情
        /// </summary>
        /// <param name="stockId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetStockHqDetailAsync(int stockId, string type = "day")
        {
            try
            {
                string cmdText = @"SELECT TOP 10 * FROM StockHq WHERE StockId=@StockId AND [type]=@type ORDER BY [DATE] DESC";
                var stockHq = await new SqlConnection(DBSetting.StockHq).QueryAsync<StockHq.Entities.StockHq>(cmdText, new
                {
                    StockId = stockId,
                    type = type
                }).ContinueWith(t => t.Result.ToList());
                if (stockHq == null)
                {
                    return Json(new { IsError = true, Msg = string.Empty, Data = string.Empty });
                }
                return Json(new { Result = "OK", Records = stockHq });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}
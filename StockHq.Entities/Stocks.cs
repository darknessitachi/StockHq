using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockHq.Entities
{
    [Table("Stocks")]
    public class Stocks
    {
        /// <summary>
        ///  主键ID
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 证券代码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 证券简称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 下跌天数
        /// </summary>
        public int Lowdays { get; set; }
        /// <summary>
        /// 涨跌幅
        /// </summary>
        public decimal Lowchg { get; set; }
        /// <summary>
        /// 收盘价
        /// </summary>
        public decimal Lowclose { get; set; }
        /// <summary>
        /// 换手率
        /// </summary>
        public decimal Lowrate { get; set; }
        /// <summary>
        /// 成交量
        /// </summary>
        public int Lowvolume { get; set; }
        /// <summary>
        /// 阶段涨跌额
        /// </summary>
        public decimal Lowturnover { get; set; }
        /// <summary>
        /// 下跌开始时间
        /// </summary>
        public string Lowbegin { get; set; }
        /// <summary>
        /// 下跌结束时间
        /// </summary>
        public string Lowend { get; set; }


        /// <summary>
        /// 上涨天数
        /// </summary>
        public int Highdays { get; set; }
        /// <summary>
        /// 涨跌幅
        /// </summary>
        public decimal Highchg { get; set; }
        /// <summary>
        /// 收盘价
        /// </summary>
        public decimal Highclose { get; set; }
        /// <summary>
        /// 换手率
        /// </summary>
        public decimal Highrate { get; set; }
        /// <summary>
        /// 成交量
        /// </summary>
        public int Highvolume { get; set; }
        /// <summary>
        /// 阶段涨跌额
        /// </summary>
        public decimal Highturnover { get; set; }
        /// <summary>
        /// 上涨开始时间
        /// </summary>
        public string Highbegin { get; set; }
        /// <summary>
        /// 上涨结束时间
        /// </summary>
        public string Highend { get; set; }


        /// <summary>
        /// 最近一个交易日的时间
        /// </summary>
        public string NearDate { get; set; }
    }
}

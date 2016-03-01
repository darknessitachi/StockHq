using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;

namespace StockHq.Entities
{

    //https://github.com/StackExchange/dapper-dot-net/tree/master/Dapper.Contrib
    [Table("StockHq")]
    public class StockHq
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 外键
        /// </summary>
        public int StockId { get; set; }
        /// <summary>
        /// 交易时间
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// 开盘价
        /// </summary>
        public decimal Open { get; set; }
        /// <summary>
        /// 收盘价
        /// </summary>
        public decimal Close { get; set; }
        /// <summary>
        /// 最高价
        /// </summary>
        public decimal Highest { get; set; }
        /// <summary>
        /// 最低价
        /// </summary>
        public decimal Lowest { get; set; }
        /// <summary>
        /// 涨跌额
        /// </summary>
        public decimal Change { get; set; }
        /// <summary>
        /// 涨跌幅
        /// </summary>
        public decimal Chg { get; set; }
        /// <summary>
        /// 成交量(手)	
        /// </summary>
        public int Volume { get; set; }
        /// <summary>
        /// 成交金额(万)
        /// </summary>
        public decimal Turnover { get; set; }
        /// <summary>
        /// 换手率
        /// </summary>
        public decimal Rate { get; set; }
    }
}

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
        /// 主键ID
        /// </summary>
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Lowdays { get; set; }
        public decimal Lowchg { get; set; }
        public decimal Lowclose { get; set; }
        public decimal Lowrate { get; set; }
        public int Lowvolume { get; set; }
        public decimal Lowturnover { get; set; }
        public string Lowcreate { get; set; }
    }
}

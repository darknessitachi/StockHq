using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockHq.Core.Util
{
    public class DBSetting
    {
        public static string App
        {
            get { return System.Configuration.ConfigurationManager.ConnectionStrings["App"].ConnectionString; }
        }
        public static string StockHq
        {
            get { return System.Configuration.ConfigurationManager.ConnectionStrings["StockHq"].ConnectionString; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StockHq.WebUI.Controllers
{
    public class HqController : Controller
    {
        public async Task<ActionResult> Index()
        {
            return View();
        }
    }
}
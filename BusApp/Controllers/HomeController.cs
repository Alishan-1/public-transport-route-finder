using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GettingApiData;

namespace BusApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
        /// <summary>
        /// Test Api Request
        /// </summary>
        /// <returns></returns>
        public ActionResult tar()
        {
            RootObject busStops = LTASingapore.test();
            return Json(busStops, JsonRequestBehavior.AllowGet);
        }
    }
}
using SAKA.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;

namespace SAKA.WEBUI.MVC.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var adress = new EndpointAddress("http://localhost:62872/KPIService.svc");
            var binding = new BasicHttpBinding();
            var channel = ChannelFactory<IKPIService>.CreateChannel(binding, adress);

            ViewBag.ScoreList = channel.GetScorecard();

            return View();
        }
    }
}
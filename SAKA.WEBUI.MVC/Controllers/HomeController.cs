using SAKA.DTO;
using SAKA.Service.Contract;
using SAKA.WEBUI.MVC.Models;
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

           // ViewBag.ScoreList = channel.GetScorecard();
            var scorelist = channel.GetScorecard().Select(c => new ScoreModel
            {
                Name = c.NAME,
                Date = FormatDate(c.DATE, c.PERIOD),
                Value = c.VALUE + " " + c.UNIT,
                Statu = GetImage(c.STATU)
            }).ToList();
            // ViewBag.ScoreList = scorelist;
            //ViewBag["ScoreList"] = scorelist;

            ViewBag.GaugeList = Gauge();

            return View(scorelist);
        }

        public List<GaugeModel> Gauge()
        {
            var adress = new EndpointAddress("http://localhost:62872/KPIService.svc");
            var binding = new BasicHttpBinding();
            var channel = ChannelFactory<IKPIService>.CreateChannel(binding, adress);

            var gaugelist = new List<GaugeModel>();

            var genislik = 800;
            var yukseklik = 30;
            //görüntü daha güzel olsun diye genislikOranı 0.5 yaptım
            var genislikOranı = 0.5;

            foreach (var x in channel.GetGauge())
            {
                var g = new GaugeModel();

               g.max=(int)Math.Round(Math.Max(x.TARGET_MAX, x.VALUE) * ((decimal)(1 + genislikOranı)),0);
               g.leftSide =(int) Math.Round(genislik * x.TARGET_MIN / g.max, 0);
               g.middle =(int) Math.Round(genislik * (x.TARGET_MAX - x.TARGET_MIN) / g.max, 0);
               g.rightSide = genislik - g.leftSide - g.middle;
               g.value = (int)Math.Round(genislik * x.VALUE / g.max, 0);

                g.leftColor = x.DIRECTION == Direction.positive ? "red" : "green";
                g.rightColor = g.leftColor == "red" ? "green" : "red";
                g.Name = x.NAME;
                gaugelist.Add(g);
            }

            return gaugelist;
        }

        private string FormatDate(DateTime date, Period period)
        {
            if (period == Period.Year) { return date.Year.ToString(); }
            if (period == Period.Month) { return date.Month + "." + date.Year; }
            //return date.Day + " " + date.Month + " " + date.Year;
            //return date.ToString("dd.MM.yyyy");
            return date.ToShortDateString();
        }
        protected string GetImage(Statu statu)
        {

            if (statu == Statu.Bad) return "red.gif";
            if (statu == Statu.Good) return "green.gif";
            return "yellow.gif";
        }
    }
}
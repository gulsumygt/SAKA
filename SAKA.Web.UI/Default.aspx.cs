using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ServiceModel;
using SAKA.Service.Contract;
using SAKA.DTO;

namespace SAKA.Web.UI
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var adress = new EndpointAddress("http://localhost:62872/KPIService.svc");
            var binding = new BasicHttpBinding();
            var channel = ChannelFactory<IKPIService>.CreateChannel(binding, adress);

            var scorecardList = channel.GetScorecard();
            
         
            this.SCORECARD.DataSource = scorecardList.Select(c => new
            {
               
                c.NAME,
                Date = FormatDate(c.DATE, c.PERIOD),
                c.PERIOD,
                VALUE = c.VALUE + " " + c.UNIT,
                STATUS =GetImage(c.STATU)
            });
            SCORECARD.DataBind();

            //var list = channel.GetKpiValue();
            //Response.Write(list);

            //var count = channel.count();
            //var kpiName = channel.AddKpi();

            //Response.Write(count);
            //Response.Write(kpiName);


        }
        private string FormatDate(DateTime date, Period period)
        {
            if (period == Period.Year) { return date.Year.ToString(); }
            if (period == Period.Month) { return date.Month + " " + date.Year; }
            return date.Day + " " + date.Month + " " + date.Year;
        }
        protected string GetImage(Statu statu)
        {

            if (statu == Statu.Bad) return "~/image/red.gif";
            if (statu == Statu.Good) return "~/image/green.gif";
            return "~/image/yellow.gif";
        }
    }
}
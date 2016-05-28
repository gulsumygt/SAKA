using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ServiceModel;
using SAKA.Service.Contract;
using SAKA.DTO;
using System.Drawing;

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
                status =GetImage(c.STATU)
            });
            SCORECARD.DataBind();

            var genislik = 400;
            var yukseklik = 30;
            var genislikOranı = 0.2;

            foreach(var x in channel.GetGauge())
            {
                var max = Math.Max(x.TARGET_MAX, x.VALUE) * ((decimal)(1 + genislikOranı));
                var genislikLeft =Math.Round(genislik * x.TARGET_MIN / max,0);
                var genislikNotr = Math.Round(genislik * (x.TARGET_MAX - x.TARGET_MIN) / max, 0);
                var genislikRight = genislik - genislikLeft - genislikNotr;
                var genislikValue = Math.Round(genislik * x.VALUE / max, 0);

                Table table = new Table();
                table.Style.Add(HtmlTextWriterStyle.MarginBottom, "20px");

                TableRow row1 = new TableRow();
                TableCell row1cell1 = new TableCell();
                row1.Width = Unit.Pixel(genislik);
                row1cell1.Style.Add(HtmlTextWriterStyle.PaddingLeft, genislikValue + "px");
                row1cell1.Text = x.VALUE + " " + x.UNIT + "<br/><img src=/image/triangle.png>";
                row1.Cells.Add(row1cell1);

                table.Rows.Add(row1);

                TableRow row2 = new TableRow();
                TableCell row2cell1 = new TableCell();
                row2.Width = Unit.Pixel(genislik);
                row2cell1.Width = Unit.Pixel((int)genislikLeft);
                row2cell1.Height = Unit.Pixel(yukseklik);
                row2cell1.BackColor = x.DIRECTION == Direction.positive ? System.Drawing.Color.Green : System.Drawing.Color.Red;
                row2.Cells.Add(row2cell1);
           

                TableCell row2cell2 = new TableCell();
                row2cell2.Width = Unit.Pixel((int)genislikNotr);
                // row2cell1.Height = Unit.Pixel(yukseklik);
                row2cell2.BackColor = System.Drawing.Color.Yellow;
                row2.Cells.Add(row2cell2);
                

                TableCell row2cell3 = new TableCell();
                row2cell3.Width = Unit.Pixel((int)genislikRight);
                // row2cell1.Height = Unit.Pixel(yukseklik);
                row2cell3.BackColor = row2cell1.BackColor == System.Drawing.Color.Red ? Color.Green : Color.Red;
                row2.Cells.Add(row2cell3);
                table.Rows.Add(row2);

                TableRow row3 = new TableRow();
                row3.Width = Unit.Pixel(genislik);
                TableCell row3cell1 = new TableCell();
                row3cell1.HorizontalAlign = HorizontalAlign.Center;
                row3cell1.Text = x.NAME;

                row3.Cells.Add(row3cell1);
                table.Rows.Add(row3);

                P1.Controls.Add(table);

            }


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
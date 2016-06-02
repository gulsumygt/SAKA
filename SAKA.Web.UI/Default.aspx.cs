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
                status = GetImage(c.STATU)
            });
            SCORECARD.DataBind();

            var genislik = 400;
            var yukseklik = 30;
            //görüntü daha güzel olsun diye genislikOranı 0.5 yaptım
            var genislikOranı = 0.5;

            foreach (var x in channel.GetGauge())
            {
                var max = Math.Max(x.TARGET_MAX, x.VALUE) * ((decimal)(1 + genislikOranı));
                var genislikLeft = Math.Round(genislik * x.TARGET_MIN / max, 0);
                var genislikNotr = Math.Round(genislik * (x.TARGET_MAX - x.TARGET_MIN) / max, 0);
                var genislikRight = genislik - genislikLeft - genislikNotr;
                var genislikValue = Math.Round(genislik * x.VALUE / max, 0);

                Table table = new Table();
                table.Style.Add(HtmlTextWriterStyle.MarginBottom, "20px");
                table.Width = Unit.Pixel(genislik);

                TableRow row1 = new TableRow();
                TableCell row1cell1 = new TableCell();
                //tablonun ilk satırı tek hücreden oluşuyor ve bu hücre ikinci satırdaki hücreler kadar o yüzden columnspan=3 ekledim
                row1cell1.ColumnSpan = 3;
                row1cell1.Style.Add("padding-Left", genislikValue + "px");
                row1cell1.Text = x.VALUE + " " + x.UNIT + "<br/><img src=/image/arrow.png>";
                row1.Cells.Add(row1cell1);

                table.Rows.Add(row1);

                TableRow row2 = new TableRow();

                TableCell row2cell1 = new TableCell();
                row2cell1.Width = Unit.Pixel((int)genislikLeft);
                row2cell1.Height = Unit.Pixel(yukseklik);
                row2cell1.BackColor = x.DIRECTION == Direction.positive ? Color.Red : Color.Green;
                row2.Cells.Add(row2cell1);


                TableCell row2cell2 = new TableCell();
                row2cell2.Width = Unit.Pixel((int)genislikNotr);
                row2cell2.BackColor = Color.Yellow;
                row2.Cells.Add(row2cell2);


                TableCell row2cell3 = new TableCell();
                row2cell3.Width = Unit.Pixel((int)genislikRight);
                row2cell3.BackColor = row2cell1.BackColor == Color.Red ? Color.Green : Color.Red;
                row2.Cells.Add(row2cell3);
                table.Rows.Add(row2);

                TableRow row3 = new TableRow();
                TableCell row3cell1 = new TableCell();
                row3cell1.HorizontalAlign = HorizontalAlign.Center;
                //tablonun üçüncü satırı tek hücreden oluşuyor ve bu hücre ikinci satırdaki hücreler kadar o yüzden columnspan=3 ekledim
                row3cell1.ColumnSpan = 3;
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
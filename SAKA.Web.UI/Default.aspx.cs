using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ServiceModel;
using SAKA.Service.Contract;

namespace SAKA.Web.UI
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var adress = new EndpointAddress("http://localhost:62872/KPIService.svc");
            var binding = new BasicHttpBinding();
            var channel = ChannelFactory<IKPIService>.CreateChannel(binding, adress);

            var count = channel.count();

            Response.Write(count);


        }
    }
}
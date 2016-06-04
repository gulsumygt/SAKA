using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAKA.WEBUI.MVC.Models
{
    public class GaugeModel
    {
        public string Name { get; set; }
        public int max { get; set; }
        public int value { get; set; }
        public int leftSide { get; set; }
        public int  middle{ get; set; }
        public int rightSide { get; set; }

        public string leftColor { get; set; }
        public string rightColor { get; set; }
    }
}
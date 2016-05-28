using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAKA.DTO
{
    public enum Direction
    {
        positive=1,
        negative=0
    };
    public class DTO_Gauge
    {
        public string NAME { get; set; }
        public decimal VALUE { get; set; }
        public Direction DIRECTION { get; set; }
        public decimal TARGET_MIN { get; set; }
        public decimal TARGET_MAX { get; set; }
        public string UNIT { get; set; }
    }
}

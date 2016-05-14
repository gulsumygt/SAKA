using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAKA.DTO
{
    public class ScoreCard
    {
        public string NAME { get; set; }
        public decimal VALUE { get; set; }
        public string UNIT { get; set; }
        public DateTime DATE { get; set; }
        public Period PERIOD { get; set; }
        public Statu STATU { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAKA.DTO
{
    public class DTO_Student
    {
        public int ID { get; set; }
        public string NAME { get; set; }
        public int SCORE { get; set; }
        public DTO_Student(int id,string name,int score)
        {
            ID = id;
            NAME = name;
            SCORE = score;
        }
    }
}

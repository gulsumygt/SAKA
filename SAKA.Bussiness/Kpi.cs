﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAKA.DTO;

namespace SAKA.Bussiness
{
   public  class Kpi
    {
        public static int count()
        {
            return 3;
        }

        public static void getStudents()
        {
            var list = new List<DTO_Student>();
            var student=new DTO_Student[]
            {
                new DTO_Student(1,"nazlı",30),
                new DTO_Student(2,"gülsüm",50),
                new DTO_Student(3,"ali",60),
                new DTO_Student(4,"emre",45),
                new DTO_Student(5,"elif",65)
            };

            list.AddRange(student);
            
            //where metodu
            var sorgu1 = list.Where(c => c.SCORE >= 50).Select(c => c.NAME).ToList();

            //2. yöntem
            /*var sorgu1 = (from x in list
                          where x.SCORE >= 50
                          select x.NAME).ToList(); */
            
            foreach(var k in sorgu1)
            {
                Console.WriteLine(k);
            }

            var sorgu2 = list.Where(c => c.NAME.StartsWith("e")).Select(c => new { c.NAME, c.SCORE }).ToList();

            foreach(var k in sorgu2)
            {
                Console.WriteLine(k.NAME + ":" + k.SCORE);
            }

            var sorgu3 = list.Select(c => new { c.NAME, c.SCORE }).OrderBy(c => c.SCORE).ToList();

        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAKA.DTO;
using Kardelen.Data;

namespace SAKA.Bussiness
{
    public class Kpi
    {
        public static ScoreCard[] GetScorecard()
        {
            using (var dc = new SAKADataDataContext())
            {
                var listKpi = dc.KPIs.Where(kpi => kpi.KPI_VALUEs.Any()).Select(c => new
                {
                    c.ID,
                    c.NAME,
                    c.PERIOD,
                    c.UNIT,
                    c.TARGET,
                    c.THRESHOLD,
                    c.THRESHOLD_TYPE,
                    c.DIRECTION
                }).ToList();

                var listDTO = new List<ScoreCard>();

                foreach (var kpi in listKpi)
                {
                    var value = dc.KPI_VALUEs.Where(c => c.KPI_ID == kpi.ID).OrderByDescending(c => c.DATE).Select(c => new { c.VALUE, c.DATE }).First();

                    var item = new ScoreCard();

                    item.NAME = kpi.NAME;
                    item.UNIT = kpi.UNIT;
                    item.DATE = value.DATE;
                    item.VALUE = value.VALUE;
                    item.PERIOD = (Period)kpi.PERIOD;
                    item.STATU = Kpi.CalculateStatu(kpi.THRESHOLD, kpi.THRESHOLD_TYPE, kpi.DIRECTION, kpi.TARGET, value.VALUE);

                    listDTO.Add(item);
                }

                return listDTO.ToArray();
            }
        }

        public static DTO_Gauge[] GetGauge()
        {
            using (var dc = new SAKADataDataContext())
            {
                var listKpi = dc.KPIs.Where(c => c.KPI_VALUEs.Any()).Select(c => new
                {
                    c.NAME,
                    c.ID,
                    c.THRESHOLD,
                    c.THRESHOLD_TYPE,
                    c.DIRECTION,
                    c.TARGET,
                    c.UNIT
                });

                var listGauge = new List<DTO_Gauge>();
                foreach (var dto in listKpi)
                {
                    var value = dc.KPI_VALUEs.Where(c => c.KPI_ID == dto.ID).Select(c => new { c.VALUE, c.DATE }).OrderByDescending(c => c.DATE).First();
                    var item = new DTO_Gauge();
                    var sapma = dto.THRESHOLD_TYPE ? dto.THRESHOLD : dto.TARGET * dto.THRESHOLD / 100;

                    item.NAME = dto.NAME;
                    item.UNIT = dto.UNIT;
                    item.VALUE = value.VALUE;
                    item.DIRECTION = dto.DIRECTION ? Direction.positive : Direction.negative;
                    item.TARGET_MAX = dto.TARGET + sapma;
                    item.TARGET_MIN = dto.TARGET - sapma;

                    listGauge.Add(item);
                }
                return listGauge.ToArray();
            }
        }
        public static void CalculateKpiValue()
        {
            using (var dc = new SAKADataDataContext())
            {
                var kpis = dc.KPIs.Where(c => c.CODE != null).Select(c => new
                {
                    c.ID,
                    c.CONNSTRING,
                    c.CODE,
                    c.PERIOD,
                    c.TARGET,
                    c.THRESHOLD,
                    c.THRESHOLD_TYPE
                });

                foreach (var kpi in kpis)
                {
                    var sql = new SQL(kpi.CONNSTRING);
                    var SpNAme = "SP_" + kpi.CODE;
                    var period = (Period)kpi.PERIOD;
                    var date = default(DateTime);
                    var kpiValue = default(decimal);

                    if (period == Period.Year)
                    {
                        date = new DateTime(DateTime.Now.Year, 1, 1);
                        kpiValue = Convert.ToDecimal(sql.SelectObject(SpNAme, date.Year));
                    }
                    else if (period == Period.Month)
                    {
                        date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        kpiValue = Convert.ToDecimal(sql.SelectObject(SpNAme, date.Year, date.Month));
                    }
                    else
                    {
                        date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        kpiValue = Convert.ToDecimal(sql.SelectObject(SpNAme, date.Year, date.Month, date.Day));
                    }

                    var itemValue = dc.KPI_VALUEs.FirstOrDefault(c => c.KPI_ID == kpi.ID && c.DATE == date);

                    if (itemValue == null)
                    {
                        itemValue = new KPI_VALUE();

                        itemValue.KPI_ID = kpi.ID;
                        itemValue.DATE = date;

                        dc.KPI_VALUEs.InsertOnSubmit(itemValue);
                    }

                    itemValue.TARGET = kpi.TARGET;
                    itemValue.THRESHOLD = kpi.THRESHOLD;
                    itemValue.THRESHOLD_TYPE = kpi.THRESHOLD_TYPE;
                    itemValue.VALUE = kpiValue;

                    dc.SubmitChanges();
                }
            }
        }
        private static Statu CalculateStatu(decimal threshold, bool thresholdType, bool direction, decimal target, decimal value)
        {
            var sapma = thresholdType ? threshold : target * threshold / 100;

            if (target + sapma < value)
            {
                return direction ? Statu.Good : Statu.Bad;
            }

            if (target - sapma > value)
            {
                return direction ? Statu.Bad : Statu.Good;
            }

            return Statu.Notr;
        }


        //public static decimal GetKpiValue()
        //{
        //    using(SAKADataDataContext dc=new SAKADataDataContext())
        //    {
        //        var KpiValue = dc.KPI_VALUEs.Where(c => c.KPI.NAME == "ciro").Select(c => c.VALUE).First();

        //        return KpiValue;
        //    }
        //}


        //public static string AddKpi()
        //{
        //    using (SAKADataDataContext dc = new SAKADataDataContext())
        //    {
        //        var Kpi = new KPI();

        //        Kpi.ID = Guid.NewGuid();
        //        Kpi.NAME = "deneme";
        //        Kpi.TARGET = 30;
        //        Kpi.THRESHOLD = 3;
        //        Kpi.THRESHOLD_TYPE = false;
        //        Kpi.PERIOD = "Y";
        //        Kpi.UNIT = "adet";
        //        Kpi.DIRECTION = true;
        //        Kpi.CREATİON_DATE = DateTime.Now;

        //        dc.KPIs.InsertOnSubmit(Kpi);
        //        dc.SubmitChanges();

        //        return Kpi.NAME;

        //    }
        //}
        //public static int count()
        //{
        //    return 3;
        //}

        //public static void getStudents()
        //{
        //    var list = new List<DTO_Student>();
        //    var student=new DTO_Student[]
        //    {
        //        new DTO_Student(1,"nazlı",30),
        //        new DTO_Student(2,"gülsüm",50),
        //        new DTO_Student(3,"ali",60),
        //        new DTO_Student(4,"emre",45),
        //        new DTO_Student(5,"elif",65)
        //    };

        //    list.AddRange(student);

        //    //where metodu
        //    var sorgu1 = list.Where(c => c.SCORE >= 50).Select(c => c.NAME).ToList();

        //    //2. yöntem
        //    /*var sorgu1 = (from x in list
        //                  where x.SCORE >= 50
        //                  select x.NAME).ToList(); */

        //    foreach(var k in sorgu1)
        //    {
        //        Console.WriteLine(k);
        //    }

        //    var sorgu2 = list.Where(c => c.NAME.StartsWith("e")).Select(c => new { c.NAME, c.SCORE }).ToList();

        //    foreach(var k in sorgu2)
        //    {
        //        Console.WriteLine(k.NAME + ":" + k.SCORE);
        //    }

        //    var sorgu3 = list.Select(c => new { c.NAME, c.SCORE }).OrderBy(c => c.SCORE).ToList();

        //}

    }
}

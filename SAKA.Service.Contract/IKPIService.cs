using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using SAKA.DTO;

namespace SAKA.Service.Contract
{
    [ServiceContract]
    public interface IKPIService
    {
        [OperationContract]
        ScoreCard[] GetScorecard();
        [OperationContract]
        DTO_Gauge[] GetGauge();

        //[OperationContract]
        //decimal GetKpiValue();

        //[OperationContract]
        //int count();
        //[OperationContract]
        //string AddKpi();
    }
}

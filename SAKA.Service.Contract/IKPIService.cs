using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
namespace SAKA.Service.Contract
{
    [ServiceContract]
    public interface IKPIService
    {
        [OperationContract]
        int count();
    }
}

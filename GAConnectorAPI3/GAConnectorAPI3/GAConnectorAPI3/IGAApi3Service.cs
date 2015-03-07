using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Description;
using System.Text;

namespace GAConnectorAPI3
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IGAApi3Service" in both code and config file together.
    [ServiceContract]
    public interface IGAApi3Service
    {
        [OperationContract]
        [WebGet]
        string ExtractData(string Profile, string Dimensions, string Metrics, string StartDate, string EndDate, string extraCols, string extraData);
    }
}

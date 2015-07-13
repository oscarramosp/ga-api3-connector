using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace GAConnectorAPI3
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "GAApi3Service" in both code and config file together.
    public class GAApi3Service : IGAApi3Service
    {
        public string ExtractData(string Profile, string Dimensions, string Metrics, string StartDate, string EndDate)
        {
            DateTime dtStartDate = new DateTime(Convert.ToInt16((StartDate.ToString().Split('-'))[0]), Convert.ToInt16((StartDate.ToString().Split('-'))[1]), Convert.ToInt16((StartDate.ToString().Split('-'))[2]));
            DateTime dtEndDate = new DateTime(Convert.ToInt16((EndDate.ToString().Split('-'))[0]), Convert.ToInt16((EndDate.ToString().Split('-'))[1]), Convert.ToInt16((EndDate.ToString().Split('-'))[2]));
            string[] ArrDimensions = Dimensions.Split(',');
            string[] ArrMetrics = Metrics.Split(',');
            GAExtractor extractor = new GAExtractor();
            return extractor.GetData(Profile, ArrDimensions, ArrMetrics, dtStartDate, dtEndDate);
        }

        public System.IO.Stream ExtractDataRaw(string Profile, string Dimensions, string Metrics, string StartDate, string EndDate)
        {
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
            string response = string.Empty;
            try
            {
                DateTime dtStartDate = new DateTime(Convert.ToInt16((StartDate.ToString().Split('-'))[0]), Convert.ToInt16((StartDate.ToString().Split('-'))[1]), Convert.ToInt16((StartDate.ToString().Split('-'))[2]));
                DateTime dtEndDate = new DateTime(Convert.ToInt16((EndDate.ToString().Split('-'))[0]), Convert.ToInt16((EndDate.ToString().Split('-'))[1]), Convert.ToInt16((EndDate.ToString().Split('-'))[2]));
                string[] ArrDimensions = Dimensions.Split(',');
                string[] ArrMetrics = Metrics.Split(',');
                GAExtractor extractor = new GAExtractor();
                response = extractor.GetData(Profile, ArrDimensions, ArrMetrics, dtStartDate, dtEndDate);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("ERROR");
                sb.AppendLine(ex.Message);
                response = sb.ToString();
            }
            return new MemoryStream(Encoding.UTF8.GetBytes(response));
        }

        public System.IO.Stream GetProfilesRaw()
        {
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
            string response = string.Empty;
            try
            {
                GAExtractor extractor = new GAExtractor();
                response = extractor.GetProfiles();
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("ERROR");
                sb.AppendLine(ex.Message);
                response = sb.ToString();
            }
            return new MemoryStream(Encoding.UTF8.GetBytes(response));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Apis.Analytics.v3.Data;

namespace GAConnectorAPI3
{
    public class AnalyticDataPoint
    {
        public AnalyticDataPoint()
        {
            Rows = new List<IList<string>>();
        }

        public IList<GaData.ColumnHeadersData> ColumnHeaders { get; set; }
        public List<IList<string>> Rows { get; set; }
    }
}

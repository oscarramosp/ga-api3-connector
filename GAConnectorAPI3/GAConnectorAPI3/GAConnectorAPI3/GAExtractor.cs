using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Analytics.v3;
using Google.Apis.Analytics.v3.Data;

namespace GAConnectorAPI3
{
    public class GAExtractor
    {
        AnalyticsService Service { get; set; }

        public string GetData(string Profile, string[] Dimensions, string[] Metrics, DateTime StartDate, DateTime EndDate)
        {
            StringBuilder sb = new StringBuilder();

            AnalyticDataPoint data = new AnalyticDataPoint();
            if (!Profile.Contains("ga:"))
                Profile = string.Format("ga:{0}", Profile);

            ServiceLogin();

            GaData response = null;
            do
            {
                int startIndex = 1;
                if (response != null && !string.IsNullOrEmpty(response.NextLink))
                {
                    Uri uri = new Uri(response.NextLink);
                    var paramerters = uri.Query.Split('&');
                    string s = paramerters.First(i => i.Contains("start-index")).Split('=')[1];
                    startIndex = int.Parse(s);
                }

                var request = BuildAnalyticRequest(Profile, Dimensions, Metrics, StartDate, EndDate, startIndex);
                response = request.Execute();
                data.ColumnHeaders = response.ColumnHeaders;
                data.Rows.AddRange(response.Rows);

            } while (!string.IsNullOrEmpty(response.NextLink));

            return AnalyticDataPointToString(data);
        }

        void ServiceLogin()
        {
            var serviceAccountMail = ConfigurationManager.AppSettings["ServiceAccountMail"].ToString();
            var keyPath = ConfigurationManager.AppSettings["KeyPath"].ToString();
            var keyword = ConfigurationManager.AppSettings["Keyword"].ToString();

            var certificate = new X509Certificate2(keyPath, keyword, X509KeyStorageFlags.Exportable);

            ServiceAccountCredential credential = new ServiceAccountCredential(
                   new ServiceAccountCredential.Initializer(serviceAccountMail)
                   {
                       Scopes = new[] { AnalyticsService.Scope.AnalyticsReadonly }
                   }.FromCertificate(certificate));

            Service = new AnalyticsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "API Project DWH",
            });
        }

        private DataResource.GaResource.GetRequest BuildAnalyticRequest(string profileId, string[] dimensions, string[] metrics,
                                                                        DateTime startDate, DateTime endDate, int startIndex)
        {
            DataResource.GaResource.GetRequest request = Service.Data.Ga.Get(profileId, startDate.ToString("yyyy-MM-dd"),
                                                                                endDate.ToString("yyyy-MM-dd"), string.Join(",", metrics));
            request.Dimensions = string.Join(",", dimensions);
            request.StartIndex = startIndex;
            return request;
        }

        private string AnalyticDataPointToString(AnalyticDataPoint analyticDataPoint)
        {
            string delimiter = "|";

            StringBuilder sb = new StringBuilder();
            string cabecera = string.Empty;

            //DataHeaders
            if (analyticDataPoint.ColumnHeaders != null && analyticDataPoint.ColumnHeaders.Count > 0)
            {
                foreach (GaData.ColumnHeadersData columna in analyticDataPoint.ColumnHeaders)
                {
                    cabecera = (string.IsNullOrEmpty(cabecera)) ? columna.Name : cabecera + delimiter + columna.Name;
                }
            }
            sb.AppendLine(cabecera);

            //Data
            string data = string.Empty;
            foreach (IList<string> filas in analyticDataPoint.Rows)
            {
                for (int i = 0; i < analyticDataPoint.ColumnHeaders.Count; i++)
                {
                    data = (string.IsNullOrEmpty(data)) ? filas[i].Replace("\"", "") : data + delimiter + filas[i].Replace("\"", "");
                }
                sb.AppendLine(data);
                data = string.Empty;
            }

            return sb.ToString();
        }

        public string GetProfiles()
        {
            StringBuilder sb = new StringBuilder();
            ServiceLogin();
            string delimiter = "|";
            ManagementResource.AccountSummariesResource.ListRequest list = Service.Management.AccountSummaries.List();
            list.MaxResults = 1000;

            AccountSummaries feed = list.Execute();
            List<AccountSummary> allRows = new List<AccountSummary>();

            while (feed.Items != null)
            {
                allRows.AddRange(feed.Items);
                // We will know we are on the last page when the next page token is
                // null.
                // If this is the case, break.
                if (feed.NextLink == null)
                {
                    break;
                }
                // Prepare the next page of results             
                list.StartIndex = feed.StartIndex + list.MaxResults;
                // Execute and process the next page request
                feed = list.Execute();

            }
            feed.Items = allRows;

            var cabecera = "AccountId" + delimiter
                            + "AccountName" + delimiter
                            + "WebPropertyId" + delimiter
                            + "WebProperty" + delimiter
                            + "ProfileId" + delimiter
                            + "Profile";
            sb.AppendLine(cabecera);

            //Get account summary and display them.
            foreach (AccountSummary account in feed.Items)
            {
                // Account
                foreach (WebPropertySummary wp in account.WebProperties)
                {
                    // Web Properties within that account
                    //Don't forget to check its not null. Believe it or not it could be.
                    if (wp.Profiles != null)
                    {
                        foreach (ProfileSummary profile in wp.Profiles)
                        {
                            // Profiles with in that web property.
                            sb.AppendLine(account.Id + delimiter +
                                            account.Name + delimiter +
                                            wp.Id + delimiter +
                                            wp.Name+ delimiter +
                                            profile.Id + delimiter +
                                            profile.Name);
                        }
                    }
                }
            }
            return sb.ToString();
        }


    }
}

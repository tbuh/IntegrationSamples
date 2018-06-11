using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace IntegrationSamples.Infrastructure
{

    public class SAPModel
    {
        public string ProductId { get; set; }
        public string ProductCategory { get; set; }
        public string ProductName { get; set; }
        public string ProductShortName { get; set; }
        public string Description { get; set; }
    }

    public class SAPService
    {
        public async Task<string> Get()
        {
            try
            {
                string api = $"https://sandbox.api.sap.com/ocbapi/platform-web/odata/services/productservice/Products(1)";
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("demoDB", "string");
                    client.DefaultRequestHeaders.Add("AppType", "string");
                    //client.DefaultRequestHeaders.Add("Content-Type", "application/json");
                    //client.DefaultRequestHeaders.Add("Accept", "string");
                    client.DefaultRequestHeaders
                        .Accept                        
                        .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders
                        .Accept
                        .Add(new MediaTypeWithQualityHeaderValue("string"));

                    client.DefaultRequestHeaders.Add("APIKey", "Qv3z8J83dK866pT8ZNbTVxsJ3FPFGcdB");

                    client.Timeout = TimeSpan.FromSeconds(15);
                    var resp = await client.GetAsync(api);

                    if (resp.IsSuccessStatusCode)
                    {
                        var result = resp.Content.ReadAsStringAsync().Result;
                        return result;
                    }
                    return null;

                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
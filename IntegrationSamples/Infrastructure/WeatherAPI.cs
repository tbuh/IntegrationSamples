using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationSamples.Infrastructure
{
    public class WeatherAPI
    {
        public WeatherAPI()
        {

        }

        public async Task<OpenWeatherMap> Get(string city)
        {
            try
            {
                string api = $"http://api.openweathermap.org/data/2.5/forecast?q={city}&units=metric&APPID={AppSettings.OpenWeatherAppID}";
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(15);
                    var resp = await client.GetAsync(api);

                    if (resp.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<OpenWeatherMap>(resp.Content.ReadAsStringAsync().Result);
                        return result;
                    }
                    return null;

                }
            }
            catch (Exception ex)
            {
                return new OpenWeatherMap { Error = ex.Message };
            }
        }
    }
}

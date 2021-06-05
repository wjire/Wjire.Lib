using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;

namespace Acb.Shield.MqError
{
    public class HttpHelper
    {
        private const string Code = "icb@9527";
        public static string Url;

        public static async Task<List<MqErrorPageDto>> Get()
        {
            string api = $"mq/page2?code={Code}";
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(Url);
                var response = await client.GetStringAsync(api);
                if (string.IsNullOrWhiteSpace(response) == false)
                {
                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MqErrorPageDto>>(response);
                    return result;
                }
                return new List<MqErrorPageDto>();
            }
        }

        public static async Task<string> Send(string id)
        {
            try
            {
                string api = $"mq/send?code={Code}&id={id}";
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Url);
                    var response = await client.GetStringAsync(api);
                    return response;
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public static async Task<string> Delete(string id)
        {
            try
            {
                string api = $"mq/delete?code={Code}";
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Url);
                    var request = new
                    {
                        Id = id,
                    };
                    var response = await client.PostAsJsonAsync(api, request);
                    var message = await response.Content.ReadAsStringAsync();
                    return message;
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public static async Task<string> SendEvent(string id, string @event)
        {
            try
            {
                string api = $"mq/sendEvent?code={Code}";
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Url);
                    var request = new
                    {
                        Id = id,
                        Event = @event
                    };
                    var response = await client.PostAsJsonAsync(api, request);
                    var message = await response.Content.ReadAsStringAsync();
                    return message;
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}

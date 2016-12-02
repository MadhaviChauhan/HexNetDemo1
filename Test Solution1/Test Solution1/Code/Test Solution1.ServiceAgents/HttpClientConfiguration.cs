using Test_Solution1.ServiceAgents.Interface;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Test_Solution1.ServiceAgents
{
    public class HttpClientConfiguration : IHttpClientConfiguration
    {

        private HttpClient _client;

        public HttpClientConfiguration()
        {
            _client = new HttpClient();
        }

        public HttpClient GetConfiguration()
        {
            string url = ConfigurationManager.AppSettings["WebAPIURL"];
            _client.BaseAddress = new Uri(url);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return _client;
        }

    }
}

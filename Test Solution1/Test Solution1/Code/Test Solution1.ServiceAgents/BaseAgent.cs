using AutoMapper;
using Newtonsoft.Json;
using System.Net.Http;

namespace Test_Solution1.ServiceAgents
{
    public class BaseAgent
    {

        public T GetData<T, U>(string requestUri)
        {
            string responseData = null;

            using (var client = new HttpClientConfiguration().GetConfiguration())
            {
                HttpResponseMessage response = client.GetAsync(requestUri).Result;

                if (response.IsSuccessStatusCode)
                    responseData = response.Content.ReadAsStringAsync().Result;
            }

            return Mapper.Map<T>(JsonConvert.DeserializeObject<U>(responseData)); ;
        }

        public void PutData<T>(string requestUri, T value)
        {
            using (var client = new HttpClientConfiguration().GetConfiguration())
            {

                HttpResponseMessage response = client.PutAsJsonAsync(requestUri, Mapper.Map<T>(value)).Result;
            }
        }

        public void PostData<T>(string requestUri, T value)
        {
            using (var client = new HttpClientConfiguration().GetConfiguration())
            {
                HttpResponseMessage response = client.PostAsJsonAsync(requestUri, Mapper.Map<T>(value)).Result;
            }
        }

        public void DeleteData(string requestUri)
        {
            using (var client = new HttpClientConfiguration().GetConfiguration())
            {
                HttpResponseMessage response = client.DeleteAsync(requestUri).Result; ;
            }
        }



    }
}

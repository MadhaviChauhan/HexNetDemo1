using System.Net.Http;

namespace Test_Solution1.ServiceAgents.Interface
{
    public interface IHttpClientConfiguration
    {
        HttpClient GetConfiguration();
    }
}

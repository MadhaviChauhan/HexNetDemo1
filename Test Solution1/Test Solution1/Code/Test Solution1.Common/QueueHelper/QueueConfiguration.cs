using System.Collections.Generic;
using System.Linq;

namespace Test_Solution1.Common.QueueHelper
{
    public class QueueConfiguration
    {
        public string ExchangeName { get; set; }
        public string QueueName { get; set; }
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RoutingKey { get; set; }

        public static Dictionary<string, string> RabbitMQConfiguration(string[] RmqHost, string[] RmqConfig)
        {
            var hostConfig =
                RmqHost.Select((s, i) => new { s, i }).GroupBy(x => x.i / 2).ToDictionary(g => g.First().s, g => g.Last().s);
            var QueueConfig =
                RmqConfig.Select((s, i) => new { s, i })
                         .GroupBy(x => x.i / 2)
                         .ToDictionary(g => g.First().s, g => g.Last().s);

            QueueConfig.ToList().ForEach(x => hostConfig[x.Key] = x.Value);

            return hostConfig;
        }
    }
}
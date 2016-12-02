using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Test_Solution1.Common.QueueHelper
{
    public class QueuePublisher : IQueuePublisher
    {
        private bool canHandle;
        private IModel channel;
        private IConnection connection;
        private Subscription subscription;
        public QueueConfiguration QueueConfiguration { get; set; }

        public event Func<byte[], bool> OnMessageReceived;

        public void PublishMessage<T>(T message) where T : class
        {
            //Contract.Requires<ArgumentNullException>(QueueConfiguration.HostName != null,
            //                                         "Argument cannot be null, Set HostName in QueueConfiguration");
            //Contract.Requires<ArgumentNullException>(QueueConfiguration.UserName != null,
            //                                         "Argument cannot be null, Set UserName in QueueConfiguration");
            //Contract.Requires<ArgumentNullException>(QueueConfiguration.Password != null,
            //                                         "Argument cannot be null, Set Password in QueueConfiguration");

            var factory = new ConnectionFactory
            {
                HostName = QueueConfiguration.HostName,
                UserName = QueueConfiguration.UserName,
                Password = QueueConfiguration.Password
            };

            using (connection = factory.CreateConnection())
            {
                using (channel = connection.CreateModel())
                {
                    // Declare Exchange, Queue and bind it together
                    channel.QueueBind(QueueConfiguration.QueueName, QueueConfiguration.ExchangeName,
                                      QueueConfiguration.RoutingKey);

                    //Publish the message
                    var payload = TypeToByteArray(message);
                    channel.BasicPublish(QueueConfiguration.ExchangeName, QueueConfiguration.RoutingKey, null, payload);
                }
            }
        }


        public void ReceiveMessage()
        {
            var factory = new ConnectionFactory
            {
                HostName = QueueConfiguration.HostName,
                UserName = QueueConfiguration.UserName,
                Password = QueueConfiguration.Password
            };

            using (connection = factory.CreateConnection())
            {
                using (channel = connection.CreateModel())
                {
                    // Bind the Queue to the exchange with the routing key
                    channel.QueueBind(QueueConfiguration.QueueName, QueueConfiguration.ExchangeName,
                                      QueueConfiguration.RoutingKey);

                    subscription = new Subscription(channel, QueueConfiguration.QueueName, false);

                    foreach (BasicDeliverEventArgs e in subscription)
                    {
                        if (OnMessageReceived != null)
                        {
                            canHandle = OnMessageReceived(e.Body);

                            if (canHandle)
                                subscription.Ack();
                        }
                    }
                }
            }
        }

        public static byte[] TypeToByteArray<T>(T obj) where T : class
        {
            var formatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream())
            {
                formatter.Serialize(memoryStream, obj);
                var arrayObj = memoryStream.ToArray();
                return arrayObj;
            }
        }

        public static T ByteArrayToType<T>(byte[] byteArray) where T : new()
        {
            MemoryStream memoryStream = null;

            var binaryFormatter = new BinaryFormatter();
            try
            {
                //Prepare for decompress
                var ms = new MemoryStream(byteArray);

                var rByte = ms.Read(byteArray, 0, byteArray.Length);

                // Convert the decompressed bytes into a stream
                memoryStream = new MemoryStream(byteArray, 0, rByte);

                // deserialize the stream into an object graph
                return (T)binaryFormatter.Deserialize(memoryStream);
            }
            finally
            {
                if (memoryStream != null)
                {
                    memoryStream.Close();
                    memoryStream.Dispose();
                }
            }
        }
    }
}
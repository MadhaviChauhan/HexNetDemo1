using System;
namespace Test_Solution1.Common.QueueHelper
{
    public interface IQueuePublisher
    {
        QueueConfiguration QueueConfiguration { get; set; }
        void PublishMessage<T>(T message) where T : class;
        void ReceiveMessage();
        event Func<byte[], bool> OnMessageReceived;
    }
}
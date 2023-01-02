using Azure.Messaging.ServiceBus;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Shared.ExternalServices.Configurations;
using Shared.ExternalServices.Constant;
using Shared.ExternalServices.DTOs;
using Shared.ExternalServices.Interfaces;
using System.Text;

namespace Shared.ExternalServices.APIServices
{
    public class MessagingService : IMessagingService
    {
        private readonly MessagingServiceSetting _messagingSetting;
        private readonly ServiceBusClient _client;
        public MessagingService(IOptions<MessagingServiceSetting> messagingSetting)
        {
            _messagingSetting = messagingSetting.Value;
            _client = new ServiceBusClient(_messagingSetting.ConnectionString);
        }
        public ServiceBusProcessor ConsumeMessage(string topicName, string subscriptionName)
        {
            return _client.CreateProcessor(topicName, subscriptionName);
        }

        public async Task PublishTopicMessage(dynamic message, string subscriberName)
        {
            message.Id = Guid.NewGuid();

            var jsonMessage = JsonConvert.SerializeObject(message);
            var busMessage = new Message(Encoding.UTF8.GetBytes(jsonMessage))
            {
                PartitionKey = Guid.NewGuid().ToString(),
                Label = subscriberName
            };

            ISenderClient topicClient = new TopicClient(_messagingSetting.ConnectionString, _messagingSetting.TopicName);
            await topicClient.SendAsync(busMessage);
            Console.WriteLine($"Sent message to {topicClient.Path}");
            await topicClient.CloseAsync();

        }

        public async Task PublishTopicMessage(List<string> messages, string subscriberName)
        {
            List<Message> busMessages = new();
            var partitionId = Guid.NewGuid().ToString();
            messages.ForEach(x =>
            {
                var busMessage = new Message(Encoding.UTF8.GetBytes(x))
                {
                    PartitionKey = partitionId,
                    Label = subscriberName
                };
                busMessages.Add(busMessage);
            });

            ISenderClient topicClient = new TopicClient(_messagingSetting.ConnectionString, _messagingSetting.TopicName);
            await topicClient.SendAsync(busMessages);
            Console.WriteLine($"Sent message to {topicClient.Path}");
            await topicClient.CloseAsync();

        }
    }
}

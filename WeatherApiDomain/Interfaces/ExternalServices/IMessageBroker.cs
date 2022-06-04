using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApiDomain.Interfaces.ExternalServices
{
    public interface IMessageBroker
    {
        Task PublishMessage(string queueName, ServiceBusMessage message);
    }
}

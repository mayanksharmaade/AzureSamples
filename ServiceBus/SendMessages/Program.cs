// See https://aka.ms/new-console-template for more information
using Azure.Messaging.ServiceBus;

const   string BusConnectionstring= "Endpoint=sb://********.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ERuAplr7wxWBmWTxvUmGXTuASJqm8SrH3+ASbFv8fgs=";
const string QueueName = "mayankservicebus-queue-1";

const int MaxNumberofMessages = 5;

ServiceBusClient client;
ServiceBusSender sender;

client = new ServiceBusClient(BusConnectionstring);
sender = client.CreateSender(QueueName);

using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();
for (int i = 1; i <= MaxNumberofMessages; i++)
{
    if (!messageBatch.TryAddMessage(new ServiceBusMessage($"This Message is-{i}")))
    {
        Console.WriteLine($"Message-{i} was not added ");
    }
}

try {
    await sender.SendMessagesAsync(messageBatch);
    Console.WriteLine("Messages have been sent ");

}

catch (Exception ex)
{
    Console.WriteLine("Exception occured:" + ex.Message);
}
finally
{
    await sender.DisposeAsync();
    await client.DisposeAsync();

}

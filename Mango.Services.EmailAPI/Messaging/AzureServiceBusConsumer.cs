using Azure.Messaging.ServiceBus;
using Mango.Services.EmailAPI.Models.DTOs;
using Mango.Services.EmailAPI.Services;
using Newtonsoft.Json;
using System.Text;
using Mango.Services.EmailAPI.Message;

namespace Mango.Services.EmailAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly IConfiguration _configuration;

        private readonly string _serviceBusConnectionString;
        private readonly string _emailCartQueueName;
        private readonly string _emailRegistrationQueueName;
        private readonly string _orderCreated_Topic;
        private readonly string _orderCreated_Email_Subscription;

        private ServiceBusProcessor _emailOrderPlacedProcessor;
        private ServiceBusProcessor _emailCartProcessor;
        private ServiceBusProcessor _emailRegistrationProcessor;

        private readonly EmailService _emailService;

        public AzureServiceBusConsumer(IConfiguration configuration, EmailService emailService)
        {
            _configuration = configuration;

            _emailService = emailService;

            _serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionStrings");

            _emailCartQueueName = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue");
            _emailRegistrationQueueName = _configuration.GetValue<string>("TopicAndQueueNames:EmailRegistrationQueue");

            _orderCreated_Topic = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic");
            _orderCreated_Email_Subscription = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreated_Email_Subscription");

            var client = new ServiceBusClient(_serviceBusConnectionString);
            _emailCartProcessor = client.CreateProcessor(_emailCartQueueName);
            _emailRegistrationProcessor = client.CreateProcessor(_emailRegistrationQueueName);
            _emailOrderPlacedProcessor = client.CreateProcessor(_orderCreated_Topic, _orderCreated_Email_Subscription);
        }

        public async Task Start()
        {
            _emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
            _emailCartProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailCartProcessor.StartProcessingAsync();

            _emailRegistrationProcessor.ProcessMessageAsync += OnEmailRegistrationRequestReceived;
            _emailRegistrationProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailRegistrationProcessor.StartProcessingAsync();

            _emailOrderPlacedProcessor.ProcessMessageAsync += OnOrderPlacedRequestReceived;
            _emailOrderPlacedProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailOrderPlacedProcessor.StartProcessingAsync();
        }

        private async Task OnOrderPlacedRequestReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            var objMessage = JsonConvert.DeserializeObject<RewardsMessage>(body);
            await _emailService.LogOrderPlaced(objMessage);
            await args.CompleteMessageAsync(args.Message);
        }

        public async Task Stop()
        {
            await _emailCartProcessor.StopProcessingAsync();
            await _emailCartProcessor.DisposeAsync();
        }

        private async Task OnEmailRegistrationRequestReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            RegistrationRequestDTO objMessage = JsonConvert.DeserializeObject<RegistrationRequestDTO>(body);
            await _emailService.EmailRegistrationAndLog(objMessage);
            await args.CompleteMessageAsync(args.Message);
        }

        private async Task OnEmailCartRequestReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            CartDTO objMessage = JsonConvert.DeserializeObject<CartDTO>(body);
            try
            {
                await _emailService.EmailCartAndLog(objMessage);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}
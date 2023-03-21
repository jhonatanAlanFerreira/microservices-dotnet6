﻿using GeekShopping.OrderAPI.Messages;
using GeekShopping.OrderAPI.RabbitMQSender;
using GeekShopping.OrderAPI.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace GeekShopping.OrderAPI.MessageConsumer
{
    public class RabbitMQPaymentConsumer : BackgroundService
    {
        private readonly OrderRepository _repository;
        private IConnection _connection;
        private IModel _channel;
        private const string ExchangeName = "FanoutPaymentUpdateExchange";
        private const string PaymentOrderUpdateQueueName = "PaymentOrderUpdateQueueName";

        public RabbitMQPaymentConsumer(OrderRepository repository, IRabbitMQMessageSender rabbitMQMessageSender, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _repository = repository;

            var factory = new ConnectionFactory();

            if (webHostEnvironment.IsDevelopment())
            {
                factory.HostName = configuration.GetValue<String>("RabbitMqDev:HostName");
                factory.UserName = "guest";
                factory.Password = "guest";
            }
            else
            {
                factory.HostName = configuration.GetValue<String>("RabbitMqProd:HostName");
                factory.UserName = "guest";
                factory.Password = "guest";
            }

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct);
            _channel.QueueDeclare(PaymentOrderUpdateQueueName, false, false, false, null);
            _channel.QueueBind(PaymentOrderUpdateQueueName, ExchangeName, "PaymentOrder");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (chanel, evt) =>
            {
                var content = Encoding.UTF8.GetString(evt.Body.ToArray());
                UpdatePaymentResultVO vo = JsonSerializer.Deserialize<UpdatePaymentResultVO>(content);
                UpdatePaymentStatus(vo).GetAwaiter().GetResult();
                _channel.BasicAck(evt.DeliveryTag, false);
            };
            _channel.BasicConsume(PaymentOrderUpdateQueueName, false, consumer);
            return Task.CompletedTask;
        }

        private async Task UpdatePaymentStatus(UpdatePaymentResultVO vo)
        {

            try
            {
                await _repository.UpdateOrderPaymentStatus(vo.OrderId, vo.Status);
            }
            catch (Exception)
            {
                //Log
                throw;
            }
        }
    }
}

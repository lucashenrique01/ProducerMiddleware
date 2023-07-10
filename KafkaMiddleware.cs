using System.IO;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace ProducerMiddleware
{
    public class KafkaMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IProducer<string, string> _producer;
        private readonly string _kafkaTopic;

        public KafkaMiddleware(RequestDelegate next, IProducer<string, string> producer)
        {
            _next = next;
            _producer = producer;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Method == "POST")
            {
                using (var reader = new StreamReader(context.Request.Body))
                {
                    var requestBody = await reader.ReadToEndAsync();
                    ModelEvent evento = JsonSerializer.Deserialize<ModelEvent>(requestBody);
                    // Enviar mensagem para o Kafka
                    var message = new Message<string, string>
                    {
                        Key = null,
                        Value = requestBody
                    };
                    Console.WriteLine(evento.topico);
                    _producer.Produce(evento.topico, message, DeliveryHandler);
                    _producer.Poll(TimeSpan.FromSeconds(2));
                }
            }

            await _next(context);            
        }

        private void DeliveryHandler(DeliveryReport<string, string> report)
        {
            Console.WriteLine(report.Status);
        }
    }  
}
using Confluent.Kafka;

namespace ProducerMiddleware
{
    public static class KafkaMiddlewareExtensions
    {
        public static IApplicationBuilder UseKafkaMiddleware(this IApplicationBuilder app)
        {
            var producerConfig = app.ApplicationServices.GetRequiredService<ProducerConfig>();
            var producer = new ProducerBuilder<string, string>(producerConfig).Build();

            return app.UseMiddleware<KafkaMiddleware>(producer);
        }
    }
}
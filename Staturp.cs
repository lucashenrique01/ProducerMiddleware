using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Confluent.Kafka;

namespace ProducerMiddleware
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            
            var producerConfig = new ProducerConfig{
                BootstrapServers = "localhost:9092"
             };
            Configuration.Bind("ProducerConfig", producerConfig);
            services.AddSingleton(producerConfig);
            // services.AddSingleton<IProducer<string, string>>(_ => new ProducerBuilder<string, string>(producerConfig).Build());

            services.AddControllers();            

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseKafkaMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
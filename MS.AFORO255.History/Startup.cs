using Aforo255.Cross.Event.Src;
using Aforo255.Cross.Event.Src.Bus;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MS.AFORO255.Deposit.Messages.Events;
using MS.AFORO255.History.Messages.EventHandlers;
using MS.AFORO255.History.Messages.Events;
using MS.AFORO255.History.Repositories;
using MS.AFORO255.History.Services;

namespace MS.AFORO255.History
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            services.Configure<Mongosettings>(opt =>
            {
                opt.Connection = Configuration.GetSection("mongo:cn").Value;
                opt.DatabaseName = Configuration.GetSection("mongo:database").Value;
            });
            services.AddScoped<IHistoryService, HistoryService>();
            services.AddScoped<IMongoBookDBContext, MongoBookDBContext>();

            /*Start - RabbitMQ*/
            services.AddMediatR(typeof(Startup));
            services.AddRabbitMQ();

            services.AddTransient<TransactionEventHandler>();
            services.AddTransient<IEventHandler<TransactionCreatedEvent>, TransactionEventHandler>();

            services.AddTransient<WithdrawalEventHandler>();
            services.AddTransient<IEventHandler<WithdrawalCreatedEvent>, WithdrawalEventHandler>();
            /*End - RabbitMQ*/
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            ConfigureEventBus(app);
            ConfigureEventBusWithdrawal(app);
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<TransactionCreatedEvent, TransactionEventHandler>();
        }
        private void ConfigureEventBusWithdrawal(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<WithdrawalCreatedEvent, WithdrawalEventHandler>();
        }
    }
}

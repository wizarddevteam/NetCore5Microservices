using Aforo255.Cross.Event.Src;
using Aforo255.Cross.Event.Src.Bus;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MS.AFORO255.Notification.Messages.EventHandlers;
using MS.AFORO255.Notification.Messages.Events;
using MS.AFORO255.Notification.Repositories;
using MS.AFORO255.Notification.Services;

namespace MS.AFORO255.Notification
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
            services.AddDbContext<ContextDatabase>(
              opt =>
              {
                  opt.UseMySQL(Configuration["cn:mariadb"]);
               });
            services.AddScoped<INotificationService, NotificationService>();
            /*Start - RabbitMQ*/
            services.AddMediatR(typeof(Startup));
            services.AddRabbitMQ();

            services.AddTransient<NotificationEventHandler>();
            services.AddTransient<IEventHandler<NotificationCreatedEvent>, NotificationEventHandler>();

            services.AddTransient<NotificationWithdrawalEventHandler>();
            services.AddTransient<IEventHandler<NotificationWithdrawalCreatedEvent>, NotificationWithdrawalEventHandler>();
            /*End - RabbitMQ*/
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            ConfigureEventBus(app);
            ConfigureWithdrawalEventBus(app);
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<NotificationCreatedEvent, NotificationEventHandler>();
        }

        private void ConfigureWithdrawalEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<NotificationWithdrawalCreatedEvent, NotificationWithdrawalEventHandler>();
        }
    }
}

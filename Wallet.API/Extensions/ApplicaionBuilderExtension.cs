using Wallet.Application.Interfaces.Services.Messaging;

namespace Wallet.API.Extensions
{
    public static class ApplicaionBuilderExtension
    {
        public static IAzureServiceBusConsumer ServiceBusConsumer { get; set; }
        public static IServiceScopeFactory ScopeFactory { get; set; }
        public static IApplicationBuilder UseAzureServiceBusConsume(this IApplicationBuilder app)
        {
            ScopeFactory = app.ApplicationServices.GetService<IServiceScopeFactory>();
            using var scope = ScopeFactory.CreateScope();
            ServiceBusConsumer = scope.ServiceProvider.GetRequiredService<IAzureServiceBusConsumer>();
            var hostApplicationLife = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            hostApplicationLife.ApplicationStarted.Register(OnStart);
            hostApplicationLife.ApplicationStarted.Register(OnStop);

            return app;
        }

        private static void OnStop()
        {
            ServiceBusConsumer.StopAutoUpdateDistributorAccountEvent();
        }

        private static void OnStart()
        {
            ServiceBusConsumer.StartAutoUpdateDistributorAccountEvent();
        }
    }
}

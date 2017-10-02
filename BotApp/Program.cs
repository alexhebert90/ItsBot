using System;
using ItsBot;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace BotApp
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        private static void Configure()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            Configuration = builder.Build();
        }

        private const string ClientId = "ClientId";
        private const string ClientSecret = "ClientSecret";
        private const string UserAgent = "UserAgent";

        public static async Task Main(string[] args)
        {
            // Set up configuration.
            Configure();

            string clientId = Configuration[ClientId];
            string clientSecret = Configuration[ClientSecret];
            string userAgent = Configuration[UserAgent];

            Console.WriteLine("Press any key to start.");
            Console.ReadLine();

            var bot = new Bot(new BotCredentials(clientId, clientSecret, userAgent));

            while(true)
            {
                await bot.RunOnceAsync();

                Console.WriteLine();
                Console.WriteLine("Waiting...");
                Console.WriteLine();              
            }

        }
    }
}

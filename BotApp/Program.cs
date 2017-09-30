using System;
using ItsBot;
using System.Threading.Tasks;
using System.Linq;
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

        private const string CLIENT_ID = "ClientId";
        private const string CLIENT_SECRET = "ClientSecret";
        private const string USER_AGENT = "UserAgent";

        public static async Task Main(string[] args)
        {
            // Set up configuration.
            Configure();

            string clientId = Configuration[CLIENT_ID];
            string clientSecret = Configuration[CLIENT_SECRET];
            string userAgent = Configuration[USER_AGENT];

            Console.WriteLine("Press any key to start.");
            Console.ReadLine();

            var bot = new Bot(new ApiCredentials(clientId, clientSecret, userAgent));

            while(true)
            {
                var result = await bot.GetFilteredCommentsAsync();
                Console.WriteLine($"Its: {result.Collection.Sum(i => i.ItsMatches.Count)}");
                Console.WriteLine($"It's: {result.Collection.Sum(i => i.It_sMatches.Count)}");

                Console.WriteLine();
                Console.WriteLine("Waiting...");
                Console.WriteLine();              
            }

        }
    }
}

namespace ProductAPI
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

    
            host.SeedDatabase();
            
            host.Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>() .ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();
                        logging.AddConsole(); // Add additional logging providers as needed
                    });
                });
    }
}
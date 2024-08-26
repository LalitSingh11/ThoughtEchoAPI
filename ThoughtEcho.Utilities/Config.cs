using Microsoft.Extensions.Configuration;

namespace ThoughtEcho.Utilities
{
    public static class Config 
    {
        private static readonly IConfiguration _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        public static string GetMailSettings(string key)
        {
            var mailSettings = _configuration.GetSection("MailSettings");
            return mailSettings[key]!;
        }
    }
}

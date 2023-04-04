using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MapleScreenScraper
{
    internal class Discord
    {
        private const string webhookURL = "https://discord.com/api/webhooks/1078267150281752589/oJ1hBa7IXDMze8cH3mSob4qhokulSS1POG1142aGgU679YMcjVtow12ZJgBNck7SoYhR";

        public static async Task PushMessageToDiscordChannel(string message)
        {
            var client = new HttpClient();
            var json = JsonSerializer.Serialize(new { content = message });

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(webhookURL, content);
            var responseString = await response.Content.ReadAsStringAsync();

            Console.WriteLine("Response: " + responseString);
        }
    }
}

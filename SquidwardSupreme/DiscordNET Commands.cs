using Discord;
using Discord.Commands;
using Discord.Webhook;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SquidwardSupreme
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        private async Task Ping()
        {
            await ReplyAsync("Pong! 🏓 **" + Program._client.Latency + "ms**");
        }

        [Command("r")]
        [Summary("Returns a random anime image.")]
        public async Task RandomImageAsync()
        {
            string @imageDir = Program.GetRandomImageDir();
            //Console.WriteLine("Got file: " + imageDir);
            //await ReplyAsync(embed: new EmbedBuilder { ImageUrl = @imageDir }.Build());

            //var embed = new EmbedBuilder()
            //{
            //    ImageUrl = $"attachment://{Path.GetFileName(imageDir).ToString()}"}.Build();
            //await Context.Channel.SendFileAsync(@imageDir, embed: embed);

            await Context.Channel.SendFileAsync(@imageDir);


            Console.WriteLine(@imageDir);
        }
    }
}
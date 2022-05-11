using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SquidwardSupreme;
using Discord;

namespace SquidwardSupreme
{
    // Create a module with the '*' prefix
    [Group("*")]
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("r")]
        [Summary("Returns a random anime image.")]
        public async Task RandomImageAsync(int num = 0)
        {
            Console.WriteLine("*r command in progress.");
            await ReplyAsync(embed: new EmbedBuilder { ImageUrl = Program.GetRandomImageDir() }.Build());
        }

        // ~say hello world -> hello world
        [Command("say")]
        [Summary("Echoes a message.")]
        public Task SayAsync([Remainder] [Summary("The text to echo")] string echo)
            => ReplyAsync(echo);

        // ReplyAsync is a method on ModuleBase 
    }
}
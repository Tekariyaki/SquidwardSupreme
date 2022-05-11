﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SquidwardSupreme
{
    class Program
    {
        static void Main(string[] args) => new Program().RunBot().GetAwaiter().GetResult();

        // Main directory to anime images. This will never change.
        static string dir = @"D:\Stuff\Pictures\Anime";
        // Main array to be filled with all image directories at startup.
        public static string[] animeImageDirs;
        // RNG to be used throughout.
        public static Random rnd = new Random();

        // Creating the necessary variables
        public static DiscordSocketClient _client; 
        private CommandService _commands;
        private IServiceProvider _services;

        private BotConfig config;

        // Runbot task
        public async Task RunBot()
        {
            _client = new DiscordSocketClient(); // Define _client
            _commands = new CommandService(); // Define _commands

            _services = new ServiceCollection() // Define _services
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();

            // If there isn't a bot config, create one
            if (!File.Exists("config.json"))
            {
                config = new BotConfig()
                {
                    prefix = "*",
                    token = "NzY3MzQwNTI4OTM2NTUwNDEy.X4wfjg.BfLR3dhv6bigyp3aw1-GJy8uBvE",
                    game = ""
                };
                File.WriteAllText("config.json", JsonConvert.SerializeObject(config, Formatting.Indented));
            }
            else
            {
                config = JsonConvert.DeserializeObject<BotConfig>(File.ReadAllText("config.json"));
            }

            string botToken = config.token; // Make a string for the token

            _client.Log += Log; // Logging

            await RegisterCommandsAsync(); // Call registercommands

            await _client.LoginAsync(TokenType.Bot, botToken); // Log into the bot user

            await _client.StartAsync(); // Start the bot user

            await _client.SetGameAsync(config.game); // Set the game the bot is playing

            // On startup, create and populate array of anime images.
            animeImageDirs = InitialiseAnimeArray();

            await Task.Delay(-1); // Delay for -1 to keep the console window opened
        }

        private async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync; // Messagerecieved

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), null); // Add module to _commands
        }

        private Task Log(LogMessage arg) // Logging
        {
            Console.WriteLine(arg); // Print the log to Console
            return Task.CompletedTask; // Return with completedtask
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            string messageLower = arg.Content.ToLower(); // Convert the message to a Lower
            var message = arg as SocketUserMessage; // Create a variable with the message as SocketUserMessage
            if (message is null || message.Author.IsBot) return; // Checks if the message is empty or sent by a bot
            int argumentPos = 0; // Sets the argpos to 0 (the start of the message)
            if (message.HasStringPrefix(config.prefix, ref argumentPos) || message.HasMentionPrefix(_client.CurrentUser, ref argumentPos)) // If the message has the prefix at the start or starts with someone mentioning the bot
            {
                var context = new SocketCommandContext(_client, message); // Create a variable called context
                var result = await _commands.ExecuteAsync(context, argumentPos, _services); // Create a veriable called result
                if (!result.IsSuccess) // If the result is unsuccessful
                {
                    Console.WriteLine(result.ErrorReason); // Print the error to console
                    // await message.Channel.SendMessageAsync(result.ErrorReason); // Print the error to the channel where the error was caused (e.g "Unknown Command")
                }
            }
        }

        // Initialise array full of all anime images, so the rand is the same each time.
        // File names will be used in each entry.
        // An index is chosen at random, and the file name is gotten.
        // Check whether there is a file with that name, then send it.
        // If no file or error, then try again. Use try/catch.
        public static string[] InitialiseAnimeArray()
        {
            string[] fileDirs = Directory.GetFiles(dir);
            //foreach (string s in fileDirs)
            //{
            //    Console.WriteLine(s);
            //}
            return fileDirs;
        }

        public static string GetRandomImageDir()
        {
            string @animeImageDir;
            int index = rnd.Next(0, Convert.ToInt32(animeImageDirs.Count()));
            @animeImageDir = animeImageDirs[index];
            return @animeImageDir;
        }
    }

    class BotConfig
    {
        public string token { get; set; }
        public string prefix { get; set; }
        public string game { get; set; }
    }
}

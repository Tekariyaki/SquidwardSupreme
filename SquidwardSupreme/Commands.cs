using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.VoiceNext;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using SquidwardSupreme;

namespace SquidwardSupreme
{
    public class Commands
    {
        // AQ 790075485102800937
        // Test 790076211496878141
        static readonly string FabioRoleIDToTag = "790075485102800937";
        static readonly string AQPrestonNSFWChannelID = "452047052612960267";
        static readonly string TestPrestonNSFWChannelID = "497816392670773258";

        //[Command("join"), Description("Joins a voice channel.")]
        //public async Task Join(CommandContext ctx, DiscordChannel chn = null)
        //{
        //    // check whether VNext is enabled
        //    var vnext = ctx.Client.GetVoiceNextClient();
        //    if (vnext == null)
        //    {
        //        // not enabled
        //        await ctx.RespondAsync("VNext is not enabled or configured.");
        //        return;
        //    }

        //    // check whether we aren't already connected
        //    var vnc = vnext.GetConnection(ctx.Guild);
        //    if (vnc != null)
        //    {
        //        // already connected
        //        await ctx.RespondAsync("Already connected in this guild.");
        //        return;
        //    }

        //    // get member's voice state
        //    var vstat = ctx.Member?.VoiceState;
        //    if (vstat?.Channel == null && chn == null)
        //    {
        //        // they did not specify a channel and are not in one
        //        await ctx.RespondAsync("You are not in a voice channel.");
        //        return;
        //    }

        //    // channel not specified, use user's
        //    if (chn == null)
        //        chn = vstat.Channel;

        //    // connect
        //    vnc = await vnext.ConnectAsync(chn);
        //    await ctx.RespondAsync($"Connected to `{chn.Name}`");
        //}

        //[Command("leave"), Description("Leaves a voice channel.")]
        //public async Task Leave(CommandContext ctx)
        //{
        //    // check whether VNext is enabled
        //    var vnext = ctx.Client.GetVoiceNextClient();
        //    if (vnext == null)
        //    {
        //        // not enabled
        //        await ctx.RespondAsync("VNext is not enabled or configured.");
        //        return;
        //    }

        //    // check whether we are connected
        //    var vnc = vnext.GetConnection(ctx.Guild);
        //    if (vnc == null)
        //    {
        //        // not connected
        //        await ctx.RespondAsync("Not connected in this guild.");
        //        return;
        //    }

        //    // disconnect
        //    vnc.Disconnect();
        //    await ctx.RespondAsync("Disconnected");
        //}

        //[Command("play"), Description("Plays an audio file.")]
        //public async Task Play(CommandContext ctx, [RemainingText, Description("Full path to the file to play.")] string filename)
        //{
        //    // check whether VNext is enabled
        //    var vnext = ctx.Client.GetVoiceNextClient();
        //    if (vnext == null)
        //    {
        //        // not enabled
        //        await ctx.RespondAsync("VNext is not enabled or configured.");
        //        return;
        //    }

        //    // check whether we aren't already connected
        //    var vnc = vnext.GetConnection(ctx.Guild);
        //    if (vnc == null)
        //    {
        //        // already connected
        //        await ctx.RespondAsync("Not connected in this guild.");
        //        return;
        //    }
        //    // check if file exists
        //    if (!File.Exists(filename))
        //    {
        //        // file does not exist
        //        await ctx.RespondAsync($"File `{filename}` does not exist.");
        //        return;
        //    }

        //    // wait for current playback to finish
        //    while (vnc.IsPlaying)
        //        await vnc.WaitForPlaybackFinishAsync();

        //    // play
        //    Exception exc = null;
        //    await ctx.Message.RespondAsync($"Playing `{filename}`");
        //    await vnc.SendSpeakingAsync(true);
        //    try
        //    {
        //        // borrowed from
        //        // https://github.com/RogueException/Discord.Net/blob/5ade1e387bb8ea808a9d858328e2d3db23fe0663/docs/guides/voice/samples/audio_create_ffmpeg.cs

        //        var ffmpeg_inf = new ProcessStartInfo
        //        {
        //            FileName = "ffmpeg",
        //            Arguments = $"-i \"{filename}\" -ac 2 -f s16le -ar 48000 pipe:1",
        //            UseShellExecute = false,
        //            RedirectStandardOutput = true,
        //            RedirectStandardError = true
        //        };
        //        var ffmpeg = Process.Start(ffmpeg_inf);
        //        var ffout = ffmpeg.StandardOutput.BaseStream;

        //        // let's buffer ffmpeg output
        //        using (var ms = new MemoryStream())
        //        {
        //            await ffout.CopyToAsync(ms);
        //            ms.Position = 0;

        //            var buff = new byte[3840]; // buffer to hold the PCM data
        //            var br = 0;
        //            while ((br = ms.Read(buff, 0, buff.Length)) > 0)
        //            {
        //                if (br < buff.Length) // it's possible we got less than expected, let's null the remaining part of the buffer
        //                    for (var i = br; i < buff.Length; i++)
        //                        buff[i] = 0;

        //                await vnc.SendAsync(buff, 20); // we're sending 20ms of data
        //            }
        //        }
        //    }
        //    catch (Exception ex) { exc = ex; }
        //    finally
        //    {
        //        await vnc.SendSpeakingAsync(false);
        //    }


        //    if (exc != null)
        //        await ctx.RespondAsync($"An exception occured during playback: `{exc.GetType()}: {exc.Message}`");
        //}

        [Command("r"), Description("Send random picture from string dir declared in Program.cs.")]
        public async Task RandomImage(CommandContext ctx)
        {
            string @imageDir = Program.GetRandomImageDir();
            Console.WriteLine(imageDir);

            string messageChannelID = ctx.Channel.Id.ToString();

            // if channel is #preston-nsfw or #squidwardscreamertest (by ID), then send image
            if (messageChannelID == AQPrestonNSFWChannelID || messageChannelID == TestPrestonNSFWChannelID)
            {
                switch (imageDir)
                {
                    case Program.HeadFilePath: //Head
                        // if the part has already been found
                        if (Program.CheckCollectionStatus("169JJ3a6ox.png.bin") == true)
                        {
                            // normal anime image
                            await ctx.RespondWithFileAsync(@imageDir);
                            Console.WriteLine(@imageDir.ToString());
                            break;
                        }
                        // newfound Fabio part
                        else
                        {
                            string message;

                            Part headpart = new Part();
                            headpart.PartFileName = "169JJ3a6ox.png";
                            headpart.PartName = "Fabio the Forbidden One";
                            headpart.PartCollected = true;
                            headpart.PartCollector = ctx.Member.Id.ToString();
                            headpart.FileName = "169JJ3a6ox.png.bin";
                            Serializer.Serialize(headpart.FileName, headpart);
                            Console.WriteLine("Fabio the Forbidden One has been collected!");

                            imageDir = Program.HeadFilePath;

                            message = "<@&" + FabioRoleIDToTag + ">! " + ctx.Member.Mention.ToString() + " found the Head of Fabio!";
                            await ctx.RespondWithFileAsync(@imageDir, message);
                            break;
                        }

                    case Program.LArmFilePath: // LArm
                        // if the part has already been found
                        if (Program.CheckCollectionStatus("iTXOIYQEKw.png.bin") == true)
                        {
                            // normal anime image
                            await ctx.RespondWithFileAsync(@imageDir);
                            Console.WriteLine(@imageDir.ToString());
                            break;
                        }
                        // newfound Fabio part
                        else
                        {
                            string message;

                            Part larmpart = new Part();
                            larmpart.PartFileName = "iTXOIYQEKw.png";
                            larmpart.PartName = "Left Arm of the Forbidden One";
                            larmpart.PartCollected = true;
                            larmpart.PartCollector = ctx.Member.Id.ToString();
                            larmpart.FileName = "iTXOIYQEKw.png.bin";
                            Serializer.Serialize(larmpart.FileName, larmpart);
                            Console.WriteLine("Left Arm of the Forbidden One has been collected!");

                            imageDir = Program.LArmFilePath;

                            message = "<@&" + FabioRoleIDToTag + ">! " + ctx.Member.Mention.ToString() + " found the Left Arm of Fabio!";
                            await ctx.RespondWithFileAsync(@imageDir, message);
                            break;
                        }

                    case Program.RArmFilePath: // RArm
                        // if the part has already been found
                        if (Program.CheckCollectionStatus("3TdTd0exXm.png.bin") == true)
                        {
                            // normal anime image
                            await ctx.RespondWithFileAsync(@imageDir);
                            Console.WriteLine(@imageDir.ToString());
                            break;
                        }
                        // newfound Fabio part
                        else
                        {
                            string message;

                            Part rarmpart = new Part();
                            rarmpart.PartFileName = "3TdTd0exXm.png";
                            rarmpart.PartName = "Right Arm of the Forbidden One";
                            rarmpart.PartCollected = true;
                            rarmpart.PartCollector = ctx.Member.Id.ToString();
                            rarmpart.FileName = "3TdTd0exXm.png.bin";
                            Serializer.Serialize(rarmpart.FileName, rarmpart);
                            Console.WriteLine("Right Arm of the Forbidden One has been collected!");

                            imageDir = Program.RArmFilePath;

                            message = "<@&" + FabioRoleIDToTag + ">! " + ctx.Member.Mention.ToString() + " found the Right Arm of Fabio!";
                            await ctx.RespondWithFileAsync(@imageDir, message);
                            break;
                        }

                    case Program.LLegFilePath: // LLeg
                        // if the part has already been found
                        if (Program.CheckCollectionStatus("68q1rY0LoT.png.bin") == true)
                        {
                            // normal anime image
                            await ctx.RespondWithFileAsync(@imageDir);
                            Console.WriteLine(@imageDir.ToString());
                            break;
                        }
                        // newfound Fabio part
                        else
                        {
                            string message;

                            Part llegpart = new Part();
                            llegpart.PartFileName = "68q1rY0LoT.png";
                            llegpart.PartName = "Left Leg of the Forbidden One";
                            llegpart.PartCollected = true;
                            llegpart.PartCollector = ctx.Member.Id.ToString();
                            llegpart.FileName = "68q1rY0LoT.png.bin";
                            Serializer.Serialize(llegpart.FileName, llegpart);
                            Console.WriteLine("Right Leg of the Forbidden One has been collected!");

                            imageDir = Program.LLegFilePath;

                            message = "<@&" + FabioRoleIDToTag + ">! " + ctx.Member.Mention.ToString() + " found the Left Leg of Fabio!";
                            await ctx.RespondWithFileAsync(@imageDir, message);
                            break;
                        }

                    case Program.RLegFilePath: // RLeg
                        // if the part has already been found
                        if (Program.CheckCollectionStatus("tcSBqq1BlX.png.bin") == true)
                        {
                            // normal anime image
                            await ctx.RespondWithFileAsync(@imageDir);
                            Console.WriteLine(@imageDir.ToString());
                            break;
                        }
                        // newfound Fabio part
                        else
                        {
                            string message;

                            Part rlegpart = new Part();
                            rlegpart.PartFileName = "tcSBqq1BlX.png";
                            rlegpart.PartName = "Right Leg of the Forbidden One";
                            rlegpart.PartCollected = true;
                            rlegpart.PartCollector = ctx.Member.Id.ToString();
                            rlegpart.FileName = "tcSBqq1BlX.png.bin";
                            Serializer.Serialize(rlegpart.FileName, rlegpart);
                            Console.WriteLine("Right Leg of the Forbidden One has been collected!");

                            imageDir = Program.RLegFilePath;

                            message = "<@&" + FabioRoleIDToTag + ">! " + ctx.Member.Mention.ToString() + " found the Right Leg of Fabio!";
                            await ctx.RespondWithFileAsync(@imageDir, message);
                            break;
                        }

                    default: // normal anime image
                        await ctx.RespondWithFileAsync(@imageDir);
                        break;
                }
            }
            else // it is not one of those channels. give link to #preston-nsfw)
            {
                await ctx.Channel.SendMessageAsync("<#452047052612960267>");
            }
        }

        #region admin commands

        //[Command("epiccheatcommandgethead48162"), Description("Gets Head.")]
        //public async Task GetHead(CommandContext ctx)
        //{
        //    string @imageDir = Program.GetRandomImageDir();

        //    // if the part has already been found
        //    if (Program.CheckCollectionStatus("169JJ3a6ox.png.bin") == true)
        //    {
        //        // normal anime image
        //        await ctx.RespondWithFileAsync(@imageDir);
        //        Console.WriteLine(@imageDir.ToString());
        //    }
        //    // newfound Fabio part
        //    else
        //    {
        //        string message;

        //        Part headpart = new Part();
        //        headpart.PartFileName = "169JJ3a6ox.png";
        //        headpart.PartName = "Fabio the Forbidden One";
        //        headpart.PartCollected = true;
        //        headpart.PartCollector = ctx.Member.Id.ToString();
        //        headpart.FileName = "169JJ3a6ox.png.bin";
        //        Serializer.Serialize(headpart.FileName, headpart);

        //        @imageDir = Program.HeadFilePath;

        //        message = "<@&" + FabioRoleIDToTag + ">! " + ctx.Member.Mention.ToString() + " found the Head of Fabio!";
        //        await ctx.RespondWithFileAsync(@imageDir, message);
        //    }
        //}

        //[Command("epiccheatcommandgetlarm58473"), Description("Gets LArm.")]
        //public async Task GetLArm(CommandContext ctx)
        //{
        //    string @imageDir = Program.GetRandomImageDir();
        //    // if the part has already been found
        //    if (Program.CheckCollectionStatus("iTXOIYQEKw.png.bin") == true)
        //    {
        //        // normal anime image
        //        await ctx.RespondWithFileAsync(@imageDir);
        //        Console.WriteLine(@imageDir.ToString());
        //    }
        //    // newfound Fabio part
        //    else
        //    {
        //        string message;

        //        Part larmpart = new Part();
        //        larmpart.PartFileName = "iTXOIYQEKw.png";
        //        larmpart.PartName = "Left Arm of the Forbidden One";
        //        larmpart.PartCollected = true;
        //        larmpart.PartCollector = ctx.Member.Id.ToString();
        //        larmpart.FileName = "iTXOIYQEKw.png.bin";
        //        Serializer.Serialize(larmpart.FileName, larmpart);
        //        Console.WriteLine("Left Arm of the Forbidden One has been collected!");

        //        imageDir = Program.LArmFilePath;

        //        message = "<@&" + FabioRoleIDToTag + ">! " + ctx.Member.Mention.ToString() + " found the Left Arm of Fabio!";
        //        await ctx.RespondWithFileAsync(@imageDir, message);
        //    }
        //}

        //[Command("epiccheatcommandgetrarm57319"), Description("Gets RArm.")]
        //public async Task GetRArm(CommandContext ctx)
        //{
        //    string @imageDir = Program.GetRandomImageDir();
        //    // if the part has already been found
        //    if (Program.CheckCollectionStatus("3TdTd0exXm.png.bin") == true)
        //    {
        //        // normal anime image
        //        await ctx.RespondWithFileAsync(@imageDir);
        //        Console.WriteLine(@imageDir.ToString());
        //    }
        //    // newfound Fabio part
        //    else
        //    {
        //        string message;

        //        Part rarmpart = new Part();
        //        rarmpart.PartFileName = "3TdTd0exXm.png";
        //        rarmpart.PartName = "Right Arm of the Forbidden One";
        //        rarmpart.PartCollected = true;
        //        rarmpart.PartCollector = ctx.Member.Id.ToString();
        //        rarmpart.FileName = "3TdTd0exXm.png.bin";
        //        Serializer.Serialize(rarmpart.FileName, rarmpart);
        //        Console.WriteLine("Right Arm of the Forbidden One has been collected!");

        //        imageDir = Program.RArmFilePath;

        //        message = "<@&" + FabioRoleIDToTag + ">! " + ctx.Member.Mention.ToString() + " found the Right Arm of Fabio!";
        //        await ctx.RespondWithFileAsync(@imageDir, message);
        //    }
        //}

        //[Command("epiccheatcommandgetlleg75138"), Description("Gets LLeg.")]
        //public async Task GetLLeg(CommandContext ctx)
        //{
        //    string @imageDir = Program.GetRandomImageDir();
        //    // if the part has already been found
        //    if (Program.CheckCollectionStatus("68q1rY0LoT.png.bin") == true)
        //    {
        //        // normal anime image
        //        await ctx.RespondWithFileAsync(@imageDir);
        //        Console.WriteLine(@imageDir.ToString());
        //    }
        //    // newfound Fabio part
        //    else
        //    {
        //        string message;

        //        Part llegpart = new Part();
        //        llegpart.PartFileName = "68q1rY0LoT.png";
        //        llegpart.PartName = "Left Leg of the Forbidden One";
        //        llegpart.PartCollected = true;
        //        llegpart.PartCollector = ctx.Member.Id.ToString();
        //        llegpart.FileName = "68q1rY0LoT.png.bin";
        //        Serializer.Serialize(llegpart.FileName, llegpart);
        //        Console.WriteLine("Right Leg of the Forbidden One has been collected!");

        //        imageDir = Program.LLegFilePath;

        //        message = "<@&" + FabioRoleIDToTag + ">! " + ctx.Member.Mention.ToString() + " found the Left Leg of Fabio!";
        //        await ctx.RespondWithFileAsync(@imageDir, message);
        //    }
        //}

        //[Command("epiccheatcommandgetrleg12497"), Description("Gets RLeg.")]
        //public async Task GetRLeg(CommandContext ctx)
        //{
        //    string @imageDir = Program.GetRandomImageDir();
        //    // if the part has already been found
        //    if (Program.CheckCollectionStatus("tcSBqq1BlX.png.bin") == true)
        //    {
        //        // normal anime image
        //        await ctx.RespondWithFileAsync(@imageDir);
        //        Console.WriteLine(@imageDir.ToString());
        //    }
        //    // newfound Fabio part
        //    else
        //    {
        //        string message;

        //        Part rlegpart = new Part();
        //        rlegpart.PartFileName = "tcSBqq1BlX.png";
        //        rlegpart.PartName = "Right Leg of the Forbidden One";
        //        rlegpart.PartCollected = true;
        //        rlegpart.PartCollector = ctx.Member.Id.ToString();
        //        rlegpart.FileName = "tcSBqq1BlX.png.bin";
        //        Serializer.Serialize(rlegpart.FileName, rlegpart);
        //        Console.WriteLine("Right Leg of the Forbidden One has been collected!");

        //        imageDir = Program.RLegFilePath;

        //        message = "<@&" + FabioRoleIDToTag + ">! " + ctx.Member.Mention.ToString() + " found the Right Leg of Fabio!";
        //        await ctx.RespondWithFileAsync(@imageDir, message);
        //    }
        //}

        //[Command("rb"), Description("Resets all bin files.")]
        //public async Task ResetBins(CommandContext ctx)
        //{
        //    Program.ResetBins();
        //}

        [Command("fi"), Description("Shows all parts collection status.")]
        public async Task FabioInfo(CommandContext ctx)
        {
            string message = "";

            bool headStatus;
            bool lArmStatus;
            bool rArmStatus;
            bool lLegStatus;
            bool rLegStatus;

            headStatus = Program.CheckCollectionStatus("169JJ3a6ox.png.bin");
            lArmStatus = Program.CheckCollectionStatus("iTXOIYQEKw.png.bin");
            rArmStatus = Program.CheckCollectionStatus("3TdTd0exXm.png.bin");
            lLegStatus = Program.CheckCollectionStatus("68q1rY0LoT.png.bin");
            rLegStatus = Program.CheckCollectionStatus("tcSBqq1BlX.png.bin");

            message = "Head: " + headStatus + "\r\n" +
                      "Left Arm: " + lArmStatus + "\r\n" +
                      "Right Arm: " + rArmStatus + "\r\n" +
                      "Left Leg: " + lLegStatus + "\r\n" +
                      "Right Leg: " + rLegStatus;

            await ctx.Channel.SendMessageAsync(message);
        }

        [Command("di"), Description("Dumps all bin info.")]
        public async Task DumpInfo(CommandContext ctx)
        {
            StringBuilder sb = new StringBuilder(500);

            string[] fabioPartPaths = new string[5];
            fabioPartPaths[0] = "169JJ3a6ox.png.bin";
            fabioPartPaths[1] = "iTXOIYQEKw.png.bin";
            fabioPartPaths[2] = "3TdTd0exXm.png.bin";
            fabioPartPaths[3] = "68q1rY0LoT.png.bin";
            fabioPartPaths[4] = "tcSBqq1BlX.png.bin";

            Console.WriteLine(fabioPartPaths);

            foreach (string fabioPiece in fabioPartPaths)
            {
                Part part = new Part();
                part = (Part)Serializer.Deserialize(fabioPiece);
                sb.Append(part.PartFileName + ", " + part.PartName + ", " + part.PartCollected + ", " + part.PartCollector + ", " + part.FileName + "\r\n");
            }

            await ctx.Channel.SendMessageAsync(sb.ToString());
        }

        #endregion admin commands
    }
}
using Discord;
using Discord.Commands;
using Discord.Net;
using Discord.Rest;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.IO;
using System.Numerics;



// See https://aka.ms/new-console-template for more information
// Captain hook webook https://discord.com/api/webhooks/1091435254780268674/26Oh14QmvZhTcYYsGDg4VVAdyl491cnFlkKNxS1TvlxG1GAJe_oSM4sNoMbIfJMsRMW5

public class Program
{
    public static Task Main(string[] args) => new Program().MainAsync();
    private DiscordSocketClient _client;
    string path  = @"C:\Users\fuzin\source\repos\Discord tracking bot\Discord tracking bot\Players Log.txt";


    public async Task MainAsync()
    {
        var config = new DiscordSocketConfig()
        {
            GatewayIntents = GatewayIntents.All
        };

        _client = new DiscordSocketClient(config);

        _client.Log += Log;

        

        var token = "MTA5MTQ0OTM4MjQ0NjA0NzI5NA.GC1_Mf.WvJw_LevRKut70_pKSZOaMIVQqvdakG6gt_DgA";

        await _client.LoginAsync(TokenType.Bot, token);



        await _client.StartAsync();

        _client.MessageReceived += ClientOnMessageReceived;

        await Task.Delay(-1);

    }
    private async Task ClientOnMessageReceived(SocketMessage socketMessage)
    {
        await Task.Run(async () =>
        {
            //Activity is not from a Bot.
            if (!socketMessage.Author.IsBot)
            {
                var channelId = socketMessage.Channel.Id.ToString();
                var channel = _client.GetChannel(Convert.ToUInt64(channelId));
                var socketChannel = (ISocketMessageChannel)channel;
                List<string> workingList = new List<string>();
                //Do Something and send a response here.
                //socketChannel.SendMessageAsync("YOUR RESPONSE");
                Console.WriteLine("User: "+ socketMessage.Author + " has Said: " + socketMessage.Content);
                if (socketMessage.Content.ToString().IndexOf("Track") != -1)
                {
                    await socketChannel.SendMessageAsync($"New Miles from {socketMessage.Author}!");
                    string[] Message = socketMessage.Content.ToString().Split(" ");

                    using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite , FileShare.ReadWrite))
                    {
                        List<string> FileRead = new List<string>();
                        using (StreamReader sr = new StreamReader(fs))
                        { 
                            while (sr.Peek() >= 0)
                            {
                                FileRead.Add(sr.ReadLine());
 
                            }

                            for (int i = 0; i < FileRead.Count; i++)
                            {
                                Console.WriteLine(FileRead[i] + "Before change");
                                string[] lineArray = FileRead[i].Split(':');
                                if (lineArray[0] == socketMessage.Author.ToString())
                                {
                                    Console.WriteLine($"Current Distance is {lineArray[1]}");
                                    int result = Int32.Parse(lineArray[1]);
                                    result += Int32.Parse(Message[1]);
                                    lineArray[1] = result.ToString();
                                    FileRead[i] = lineArray[0] + ":" + lineArray[1];
                                    await socketChannel.SendMessageAsync($"You're new distance is {FileRead[1]}.");
                                }
                                Console.WriteLine(FileRead[i] + "after change");
                            }
                            workingList = FileRead;
                            sr.Close();
                            
                        }
                        using (StreamWriter sw = new StreamWriter(path)) 
                        {
                            for (int i = 0; i < workingList.Count; i++)
                            {
                                sw.WriteLine(workingList[i]);
                            }
                        }

                    }
                }


            }
        });
    }
    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}
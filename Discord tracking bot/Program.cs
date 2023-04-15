using Discord;
using Discord.Commands;
using Discord.Net;
using Discord.Rest;
using Discord.WebSocket;
using System.Text.Json;
using System;
using System.Reflection;
using System.IO;
using System.Numerics;
using System.Threading.Channels;


// See https://aka.ms/new-console-template for more information
// Captain hook webook https://discord.com/api/webhooks/1091435254780268674/26Oh14QmvZhTcYYsGDg4VVAdyl491cnFlkKNxS1TvlxG1GAJe_oSM4sNoMbIfJMsRMW5

public class Program
{
    public static Task Main(string[] args) => new Program().MainAsync();
    private DiscordSocketClient? _client;
    private string path = System.IO.Directory.GetCurrentDirectory().ToString() + @"\Players Log.txt";



    public async Task MainAsync()
    {

            string text = File.ReadAllText(@"./Token.json");
            var token = JsonSerializer.Deserialize<Token>(text);

        var config = new DiscordSocketConfig()
        {
            GatewayIntents = GatewayIntents.All
        };

        if (!File.Exists(path))
            File.Create(path).Close();
        _client = new DiscordSocketClient(config);

        _client.Log += Log;

        await _client.LoginAsync(TokenType.Bot, token.token);
        await _client.StartAsync();
        var channel = await _client.GetChannelAsync(1091435186241146995) as IMessageChannel;
        await channel!.SendMessageAsync("Welcome to the Milage Tracker Bot!");

        _client.MessageReceived += ClientOnMessageReceived;
        


        await Task.Delay(-1);

    }

    private async Task ClientOnMessageReceived(SocketMessage socketMessage)
    {
        await Task.Run(async () =>
        {
            
            //Activity is not from a Bot.
            if (!socketMessage.Author.IsBot && socketMessage.Content.StartsWith("!"))
            {
                var channelId = socketMessage.Channel.Id.ToString();
                var channel = _client.GetChannel(Convert.ToUInt64(channelId));
                var socketChannel = (ISocketMessageChannel)channel;
                List<string> workingList = new List<string>();
                //Do Something and send a response here.
                //socketChannel.SendMessageAsync("YOUR RESPONSE");
                Console.WriteLine("User: "+ socketMessage.Author + " has Said: " + socketMessage.Content);

                if (socketMessage.Content.ToString().ToUpper().Contains("ADDME"))
                {
                    string report = AddPlayer(socketMessage.Author.ToString());
                    await socketChannel.SendMessageAsync(report);
                }

                if (socketMessage.Content.ToString().ToUpper().Contains("TRACK"))
                {
                    string[] Message = socketMessage.Content.ToString().Split(" ");
                    string report = AddMiles(Message, socketMessage.Author.ToString());
                    await socketChannel.SendMessageAsync(report);
                }

                if (socketMessage.Content.ToString().ToUpper().Contains("LEADER"))
                {
                    List<string> report = PostScores();
                    await socketChannel.SendMessageAsync("Here is the Current Leaderboard");
                    await socketChannel.SendMessageAsync("===============================");
                    for(int i=0;i<report.Count;i++) 
                    await socketChannel.SendMessageAsync(report[i]);
                }
                if (socketMessage.Content.ToString().ToUpper().Contains("KILO"))
                {
                    string[] Message = socketMessage.Content.ToString().Split(" ");
                    string report = calcKilo(Message);
                    await socketChannel.SendMessageAsync($"{Message[1]} Kilometers is {report} in Miles.");  
                }



            }
        });
    }

    public string calcKilo(string[] Message)
    {

        float kilo = float.Parse(Message[1]);
        float result = kilo * 0.621371F;
        return result.ToString();
    }
    public string AddPlayer(string Player)
    {
            List<string> FileRead = new List<string>();
            bool Check = false;

            using (StreamReader sr = new StreamReader(path))
            {
                while (sr.Peek() >= 0)
                {
                    FileRead.Add(sr.ReadLine());
                }

                for (int i = 0; i < FileRead.Count; i++)
                {
                    if (FileRead[i].Contains(Player))
                        Check = true;
                }
                sr.Close();
            }


            if (!Check)
            {
                FileRead.Add(Player + ":  " + "0");

                using (StreamWriter sw = new StreamWriter(path))
                {
                    for (int i = 0; i < FileRead.Count; i++)
                    {
                        sw.WriteLine(FileRead[i]);
                    }
                    sw.Close();
                }
                return $"Adding {Player} to the Challenge!";
            }
            else
                return $"{Player} is already Registered for this Challenge";
    }

    public  string  AddMiles(string[] Message, string Player)
    {
        try
        {
            float result = 0;
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
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

                        string[] lineArray = FileRead[i].Split(':');
                        if (lineArray[0] == Player)
                        {
                            result = float.Parse(Message[1]) + float.Parse(lineArray[1]);
                            lineArray[1] = result.ToString();
                            FileRead[i] = lineArray[0] + ":  " + lineArray[1];
                            Console.WriteLine($"Current Distance is {lineArray[1]} For user {lineArray[0]} current user message {Message[0]}");
                            Console.WriteLine($"is adding {Message[1]} to their total!");
                            if (result > 250)
                            {
                                sr.Close();
                                return $" @everyone That'll do it folks! {lineArray[0]} Has Won the Challenge with {result} Miles!";
                            }
                        }
                    }
                    sr.Close();

                }
                using (StreamWriter sw = new StreamWriter(path))
                {
                    for (int i = 0; i < FileRead.Count; i++)
                    {
                        sw.WriteLine(FileRead[i]);
                    }
                }
                return $"Your new distance is {result} Miles!";

            }
        }
        catch (IndexOutOfRangeException ex)
        {
            return "You did not give me your milage!";
        }
        catch (FormatException ex)
        {
            return "Hold up, Thats not a number!";
        }
    }

    public List<string> PostScores()
    {
        List<string> FileRead = new List<string>();
        using (StreamReader sr = new StreamReader(path))
        {
            while (sr.Peek() >= 0)
            {
                FileRead.Add(sr.ReadLine());
            }
            sr.Close();
            return FileRead;
        }
    }
    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
    public class Token
    {
        public string token { get; set; }
    }
}
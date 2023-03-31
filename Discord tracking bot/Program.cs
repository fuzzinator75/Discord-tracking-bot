using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Reflection;

// See https://aka.ms/new-console-template for more information
// Captain hook webook https://discord.com/api/webhooks/1091435254780268674/26Oh14QmvZhTcYYsGDg4VVAdyl491cnFlkKNxS1TvlxG1GAJe_oSM4sNoMbIfJMsRMW5

public class Program
{
    public static Task Main(string[] args) => new Program().MainAsync();
    private DiscordSocketClient _client;

    public async Task MainAsync()
    {
        _client = new DiscordSocketClient();
        _client.Log += Log;
        _client.MessageReceived += ClientOnMessageReceived;

        var token = "MTA5MTQ0OTM4MjQ0NjA0NzI5NA.GC1_Mf.WvJw_LevRKut70_pKSZOaMIVQqvdakG6gt_DgA";

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        await Task.Delay(-1);

    }
    private async Task ClientOnMessageReceived(SocketMessage socketMessage)
    {
        await Task.Run(() =>
        {
            //Activity is not from a Bot.
            if (!socketMessage.Author.IsBot)
            {

                var authorId = socketMessage.Author.Id;
                var channelId = socketMessage.Channel.Id.ToString();
                var messageId = socketMessage.Id.ToString();
                var message = socketMessage.Content;

                var channel = _client.GetChannel(Convert.ToUInt64(channelId));
                var socketChannel = (ISocketMessageChannel)channel;

                //Do Something and send a response here.
                Console.WriteLine(authorId + "   " + message + "   " + channelId + "   " + messageId);
                //socketChannel.SendMessageAsync("YOUR RESPONSE");
            }
        });
    }

    private async Task OnReady() 
    { 

    }
    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}
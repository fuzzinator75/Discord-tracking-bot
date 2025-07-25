using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;

public class BotService
{
    private readonly DiscordSocketClient _client;
    private readonly MessageServices _playerService;
    
    public class Token
    {
        [JsonProperty("token")]
        public string key { get; set; }
    }
    public BotService()
    {
        _client = new DiscordSocketClient(new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.All
        });
        _playerService = new MessageServices();
    }

    public async Task RunAsync()
    {
        string dataFromJson = File.ReadAllText("./Token.json");
        var token = JsonConvert.DeserializeObject<Token>(dataFromJson);
        _client.Log += Log;
        _client.MessageReceived += OnMessageReceived;
        await _client.LoginAsync(TokenType.Bot, token.key);
        await _client.StartAsync();
        await Task.Delay(-1);
    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }

    private async Task OnMessageReceived(SocketMessage message)
    {
        if (message.Author.IsBot || !message.Content.StartsWith("!")) return;
        var response = await _playerService.ProcessCommandAsync(message);

        if (!string.IsNullOrEmpty(response))
        {
            var channel = message.Channel as ISocketMessageChannel;
            await channel.SendMessageAsync(response);
        }
    }
}
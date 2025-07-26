using Discord;
using Discord.WebSocket;
using Discord.Interactions;
using Newtonsoft.Json;

public class BotService
{
    private readonly DiscordSocketClient _client;
    private readonly InteractionService _interactionService;
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
            GatewayIntents = GatewayIntents.All,
            AlwaysDownloadUsers = true
        });
        _interactionService = new InteractionService(_client.Rest);
        _playerService = new MessageServices();
    }

    public async Task RunAsync(IServiceProvider services)
    {
        string dataFromJson = File.ReadAllText("./Token.json");
        var token = JsonConvert.DeserializeObject<Token>(dataFromJson);
        _client.Log += Log;
        _client.Ready += () => ReadyAsync(services);
        _client.InteractionCreated += interaction => HandleInteraction(interaction,services);
        await _client.LoginAsync(TokenType.Bot, token.key);
        await _client.StartAsync();
        await Task.Delay(-1);
    }

    private async Task ReadyAsync(IServiceProvider services)
    {
        await _interactionService.AddModulesAsync(typeof(SlashModule).Assembly, services);
        await _interactionService.RegisterCommandsToGuildAsync(1398406997937885194);
       // await _interactionService.RegisterCommandsGloballyAsync();
    }

    private async Task HandleInteraction(SocketInteraction interaction, IServiceProvider services)
    {
        var ctx = new SocketInteractionContext(_client, interaction);
        await _interactionService.ExecuteCommandAsync(ctx, services);
    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}
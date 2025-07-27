using Microsoft.Extensions.DependencyInjection;
using Discord.Interactions;

// See https://aka.ms/new-console-template for more information
// Captain hook webook https://discord.com/api/webhooks/1091435254780268674/26Oh14QmvZhTcYYsGDg4VVAdyl491cnFlkKNxS1TvlxG1GAJe_oSM4sNoMbIfJMsRMW5

public class Program
{
    public static async Task Main(string[] args)
    {
        var services = new ServiceCollection()
            .AddSingleton<InteractionService>()
            .AddSingleton<BotService>()
            .AddSingleton<FileService>(new FileService("Suggestions.txt"))
            .AddSingleton<SlashService>()
            .AddSingleton<SheetService>()
            .BuildServiceProvider();

        var botService = services.GetRequiredService<BotService>();
        await botService.RunAsync(services);
    }
}
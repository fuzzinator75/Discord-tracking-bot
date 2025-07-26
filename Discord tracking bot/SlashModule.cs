using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System.Threading.Tasks;

public class SlashModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly SlashService _slashService;
    private ulong _adminChannel;


    public SlashModule(SlashService slashService)
    {
        _slashService = slashService;
        _adminChannel = 1398459684012425267; //test
        //_adminChannel = 1346502611125145600; //live
    }
    [SlashCommand("suggest", "Submit a suggestion for admins to review")]
    public async Task Suggest(string suggestion)
    {
        _slashService.AddSuggestion(Context.User.ToString(), suggestion);
        await RespondAsync("Thank you for your suggestion!", ephemeral: true);
    }
    [EnabledInDm(false)]
    [RequireUserPermission(GuildPermission.Administrator)]
    [SlashCommand("review-suggestions", "Review all submitted suggestions")]
    public async Task ReviewSuggestions()
    {
        var channel = Context.Guild.GetTextChannel(_adminChannel);
        var suggestions = _slashService.GetAllSuggestions();
        await channel.SendMessageAsync(string.IsNullOrWhiteSpace(suggestions)
            ? "No suggestions have been submitted yet."
            : suggestions);
        await RespondAsync("Suggestions have been sent to the admin channel.", ephemeral: true);
    }
    [EnabledInDm(false)]
    [RequireUserPermission(GuildPermission.Administrator)]
    [SlashCommand("delete-suggestion", "Delete a suggestion by its index")]
    public async Task DeleteSuggestion(int index)
    {
        try
        {
            var channel = Context.Guild.GetTextChannel(_adminChannel);
            var deletedSuggestion = _slashService.DeleteSuggestion(index);
            await RespondAsync($"Suggestion #{deletedSuggestion} has been deleted.");
        }
        catch
        {
            await RespondAsync("Invalid suggestion index.");
        }
    }
}
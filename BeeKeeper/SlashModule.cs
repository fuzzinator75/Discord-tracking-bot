using Discord.Interactions;
using Discord.WebSocket;
using System.Threading.Tasks;

public class SlashModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly SlashService _slashService;


    public SlashModule(SlashService slashService)
    {
        _slashService = slashService;
    }

    [SlashCommand("suggest1", "Submit a suggestion for admins to review")]
    public async Task Suggest(string suggestion)
    {
        _slashService.AddSuggestion(Context.User.ToString(), suggestion);
        await RespondAsync("Thank you for your suggestion!");
    }

    [SlashCommand("review-suggestions1", "Review all submitted suggestions")]
    public async Task ReviewSuggestions()
    {
        var suggestions = _slashService.GetAllSuggestions();
        await RespondAsync(string.IsNullOrWhiteSpace(suggestions)
            ? "No suggestions have been submitted yet."
            : suggestions);
    }

    [SlashCommand("delete-suggestion1", "Delete a suggestion by its index")]
    public async Task DeleteSuggestion(int index)
    {
        try
        {
            _slashService.DeleteSuggestion(index);
            await RespondAsync($"Suggestion #{index} has been deleted.");
        }
        catch
        {
            await RespondAsync("Invalid suggestion index.");
        }
    }
}
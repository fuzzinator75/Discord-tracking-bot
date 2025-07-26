using Discord;
using Discord.Interactions;


public class SlashModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly FileService _fileService;
     

    public SlashModule(FileService fileService)
    {
        _fileService = fileService;
    }

    [SlashCommand("suggest", "Submit a suggestion for admins to review")]
    public async Task Suggest(string suggestion)
    {
        _fileService.AddSuggestion(Context.User.ToString(), suggestion);
        await RespondAsync("Thank you for your suggestion!",ephemeral:true);
        _fileService.SortAndReindexSuggestions();
    }

    [SlashCommand("review-suggestions", "Review all submitted suggestions")]
    public async Task ReviewSuggestions()
    {
        var adminChannel = Context.Client.GetChannel(1398459684012425267) as ITextChannel;
        var suggestions = _fileService.GetAllSuggestions();
        await adminChannel.SendMessageAsync(string.IsNullOrWhiteSpace(suggestions)
            ? "No suggestions have been submitted yet."
            : suggestions);
        await RespondAsync($"Please check {adminChannel.Name}",ephemeral:true);
    }

    [SlashCommand("delete-suggestion", "Delete a suggestion by its index")]
    public async Task DeleteSuggestion(int index)
    {
        try
        {
            var adminChannel = Context.Client.GetChannel(1398459684012425267) as ITextChannel;
            _fileService.DeleteSuggestion(index);
            await adminChannel.SendMessageAsync($"Suggestion #{index} has been deleted.");
            _fileService.SortAndReindexSuggestions();
            await RespondAsync($"Please check {adminChannel.Name}", ephemeral: true);
        }
        catch
        {
            await RespondAsync("Invalid suggestion index.", ephemeral:true);
        }
    }
}
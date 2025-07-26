using Discord.WebSocket;
using System.Threading.Tasks;
public class MessageServices
{
    private readonly FileService _suggestionService;
    private readonly SlashService _slashService;

    public MessageServices()
    {
        _suggestionService = new FileService("Suggestions.txt");
    }

    public async Task<string> ProcessCommandAsync(SocketMessage message)
    {
        var content = message.Content.Trim();
        var upperContent = content.ToUpper();
        var User = message.Author as SocketGuildUser;
        

        if (upperContent.StartsWith("!SUGGEST "))
        {
            var suggestion = content.Substring("!suggest ".Length).Trim();
            if (string.IsNullOrWhiteSpace(suggestion))
                return "Please provide a suggestion after !suggest.";
            _slashService.AddSuggestion(message.Author.ToString(), suggestion);
            return "Thank you for your suggestion!";
        }

        if (upperContent.StartsWith("!REVIEWSUGGESTIONS") && User.GuildPermissions.Administrator)
        {
            // Optionally, check if the user is an admin here
            var suggestions = _slashService.GetAllSuggestions();
            return string.IsNullOrWhiteSpace(suggestions)
                ? "No suggestions have been submitted yet."
                : suggestions;
        }

        if (upperContent.StartsWith("!DELETESUGGESTION ") && User.GuildPermissions.Administrator)
        {
            var parts = content.Split(' ', 2);
            if (parts.Length < 2 || !int.TryParse(parts[1], out int index))
                return "Please provide a valid suggestion index to delete. Example: !deletesuggestion 2";

            try
            {
                _slashService.DeleteSuggestion(index); // User sees 1-based, file is 0-based
                _suggestionService.OrganizeSuggestions(); 
                return $"Suggestion #{index} has been deleted.";
            }
            catch (ArgumentOutOfRangeException)
            {
                return "Invalid suggestion index.";
            }
        }

        // ...existing command handling...
        return null;
    }
}
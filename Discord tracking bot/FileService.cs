public class FileService
{
    private readonly string _path;
    private int _suggestionCount = 0;
    public FileService(string fileName)
    {
        _path = Path.Combine(Directory.GetCurrentDirectory(), fileName);
        if (!File.Exists(_path))
            File.Create(_path).Close();
        else
        {
            _suggestionCount = File.ReadAllLines(_path).Length;
        }
    }

    public void AddSuggestion(string user, string suggestion)
    {
        var entry = $"{_suggestionCount} | {DateTime.Now.ToString("yyyy-dd-MM HH:mm"):u} | {user} | {suggestion}";
        File.AppendAllText(_path, entry + Environment.NewLine);
        _suggestionCount = File.ReadAllLines(_path).Length;
    }

    public string GetAllSuggestions()
    {
        return File.ReadAllText(_path);
    }

    public void DeleteSuggestion(int index)
    {
        var lines = File.ReadAllLines(_path).ToList();
        if (index < 0 || index > lines.Count)
            throw new ArgumentOutOfRangeException(nameof(index), "Invalid suggestion index.");

        lines.RemoveAt(index - 1);
        File.WriteAllLines(_path, lines);
        _suggestionCount = File.ReadAllLines(_path).Length;
    }

    public void SortAndReindexSuggestions()
    {
        var lines = File.ReadAllLines(_path)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .ToList();

        var sortedLines = lines
            .Select(line =>
            {
                var parts = line.Split('|');
                return new
                {
                    Original = line,
                    User = parts.Length > 2 ? parts[2].Trim() : string.Empty,
                    Suggestion = parts.Length > 3 ? parts[3].Trim() : string.Empty,
                    Date = parts.Length > 1 ? parts[1].Trim() : string.Empty,
                };
            })
            .OrderBy(entry => entry.User)
            .ThenBy(entry => entry.Date)
            .ToList();

        for (int i = 0; i < sortedLines.Count; i++)
        {
            var newLine = $"{i + 1} | {sortedLines[i].Date} | {sortedLines[i].User} | {sortedLines[i].Suggestion}";
            sortedLines[i] = new { Original = newLine, User = sortedLines[i].User, Suggestion = sortedLines[i].Suggestion, Date = sortedLines[i].Date };
        }

        File.WriteAllLines(_path, sortedLines.Select(x => x.Original));
        _suggestionCount = sortedLines.Count + 1;
        Console.WriteLine("Sort And ReIndexing");
    }
}
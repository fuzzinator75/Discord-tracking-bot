public class FileService
{
    private readonly string _path;
    private int _suggestionCount = 1;
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
        _suggestionCount++;
    }

    public string GetAllSuggestions()
    {
        return File.ReadAllText(_path);
    }

    public void DeleteSuggestion(int index)
    {
        var lines = File.ReadAllLines(_path).ToList();
        if (index < 0 || index >= lines.Count)
            throw new ArgumentOutOfRangeException(nameof(index), "Invalid suggestion index.");

        lines.RemoveAt(index);
        File.WriteAllLines(_path, lines);
    }

    public void OrganizeSuggestions()
    {
        string[] lines = File.ReadAllLines(_path);
        var sortedLines = lines
            .Select(line => new
            {
                Original = line,
                User = line.Split('|')[1].Trim()
            })
            .OrderBy(entry => entry.User)
            .Select(entry => entry.Original)
            .ToArray();
        File.WriteAllLines(_path, sortedLines);
        Console.WriteLine("Lines sorted by Submitter and written to Sorted.txt");
    }
}
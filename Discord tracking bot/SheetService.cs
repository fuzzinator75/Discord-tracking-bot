using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.IO;
using System.Collections.Generic;

public class SheetService
{
    private readonly SheetsService _service;
    private readonly string _spreadsheetID;
    private readonly string _range;
    private IList<IList<object>> _values;

    public SheetService()
    {
        _spreadsheetID = "1-7gZfCjB-MH2le2Ge_ojArMKjdM4cyzCGDm-dXDsz5A";
        _range = "'Invite List'!A1:Z1000";
        GoogleCredential credential;
        using (var stream = new FileStream("./Jsons/GoogleAPI.json", FileMode.Open, FileAccess.Read))
        {
            credential = GoogleCredential.FromStream(stream)
                .CreateScoped("https://www.googleapis.com/auth/spreadsheets");
        }
        _service = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = "Discord Tracking Bot"
        });

        var request = _service.Spreadsheets.Values.Get(_spreadsheetID, _range);
        var response = request.Execute();
        _values = response.Values;
    }

    public List<string> ReccomendedPeople(string user)
    {
        string UPPERUSER = user.ToUpper();
        IList<IList<object>> spreadSheet = _values;
        List<string> reccomendedPeople = new List<string>();
        if (spreadSheet == null || spreadSheet.Count == 0)
        {
            reccomendedPeople.Add("No Reccomendations given.");
            return reccomendedPeople; // Return empty list if no data is found
        }
        else
        {
            foreach (var row in spreadSheet)
            {
                if (row.Count > 0 && row[1].ToString().ToUpper().Contains(UPPERUSER))
                {
                    for (int i = 2; i < row.Count; i++)
                    {
                        string value = row[i]?.ToString();
                        if (!string.IsNullOrWhiteSpace(value))
                            reccomendedPeople.Add(value);
                    }
                }
            }
            return reccomendedPeople;
        }

    }

    public string AddReccomendedPerson(string user, string reccomendedPerson)
    {
        string UPPERUSER = user.ToUpper();
        if (_values == null || _values.Count == 0)
            return "No data found.";

        for (int rowIndex = 0; rowIndex < _values.Count; rowIndex++)
        {

            var row = _values[rowIndex];

            bool alreadyAdded = row.Any(cell => cell.ToString().Equals(reccomendedPerson, StringComparison.OrdinalIgnoreCase));
            if (alreadyAdded)
            {
                return $"{reccomendedPerson} is already listed for {user}.";
            }
            // Check if user is in column 2 (index 1)
            if (row.Count > 1 && row[1].ToString().ToUpper() == UPPERUSER)
            {
                // Append the new person to the end of the row
                int appendCol = row.Count + 1; // 1-based index for Sheets API
                string cell = $"'{_range.Split('!')[0].Trim('\'')}'!{GetColumnLetter(appendCol)}{rowIndex + 1}";

                var valueRange = new ValueRange
                {
                    Values = new List<IList<object>> { new List<object> { reccomendedPerson } }
                };

                var updateRequest = _service.Spreadsheets.Values.Update(valueRange, _spreadsheetID, cell);
                updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
                updateRequest.Execute();

                return $"Added {reccomendedPerson} to {user}'s row.";
            }
        }
        return "User not found.";
    }

    // Helper to convert column number to letter (e.g., 3 -> "C")
    private string GetColumnLetter(int col)
    {
        string colLetter = "";
        while (col > 0)
        {
            int rem = (col - 1) % 26;
            colLetter = (char)(rem + 'A') + colLetter;
            col = (col - 1) / 26;
        }
        return colLetter;
    }
}

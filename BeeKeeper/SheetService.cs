using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;
using System.Windows.Markup;


namespace Discord_tracking_bot
{
    public class SheetService
    {
        private IList<IList<object>> _values;
        public SheetService()
        {
            string spreadsheetID = "1-7gZfCjB-MH2le2Ge_ojArMKjdM4cyzCGDm-dXDsz5A";
            string range = "Sheet1!A1:Z1000";
            GoogleCredential credential;
            using (var stream = new FileStream("./Jsons/GoogleAPI.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped("https://www.googleapis.com/auth/spreadsheets");
            }
            var service = new Google.Apis.Sheets.v4.SheetsService(new Google.Apis.Services.BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "Discord Tracking Bot"
            });

            var request = service.Spreadsheets.Values.Get(spreadsheetID,range);
            var response = request.Execute();
            _values = response.Values;
        }

        public List<string> ReccomendedPeople(string user)
        {
            IList<IList<object>> spreadSheet = new List<IList<object>>();
            spreadSheet = _values;
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
                    if (row.Count > 0 && row[0].ToString().Contains(user))
                    {
                        reccomendedPeople.Add(row[1].ToString());
                    }
                }
                return reccomendedPeople;
            }

        }

    }
}

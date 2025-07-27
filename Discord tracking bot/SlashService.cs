

public class SlashService
    {
        private readonly FileService _fileService;  
        private readonly SheetService _sheetService;
    public SlashService(FileService fileService, SheetService sheetService)
        {
            _fileService = fileService;
            _sheetService = sheetService;
            _sheetService = sheetService;
    }
    public void AddSuggestion(string user, string suggestion)
        {
            _fileService.AddSuggestion(user, suggestion);
        }

        public string GetAllSuggestions()
        {
           
            return _fileService.GetAllSuggestions();
        }

        public string DeleteSuggestion(int index)
        {
            return _fileService.DeleteSuggestion(index);
        }
        
        public List<string> GetReccomendedPeople(string user)
        {
        return _sheetService.ReccomendedPeople(user);
        }

        public string AddReccomendedPerson(string user, string reccomendedPerson)
        {
            return _sheetService.AddReccomendedPerson(user, reccomendedPerson);
        }

}


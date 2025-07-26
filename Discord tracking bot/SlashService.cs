
    public class SlashService
    {
        private readonly FileService _fileService;  
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


    }


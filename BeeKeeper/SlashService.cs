using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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

        public void DeleteSuggestion(int index)
        {
            _fileService.DeleteSuggestion(index);
        }


    }


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowershellGUI.Models
    {
    class FileReader : ObservableObject
        {
        private string _fileURI;
        private string _fileContents;

        private void SetFileToRead(string ModulePath)
            {
            // håndterer logikk rundt hvordan filen som skal leses, velges
            // radio knapp valg etc
            }

        /// <summary>
        /// Constructor
        /// </summary>
        public FileReader(string modulePath)
            {
            SetFileToRead(modulePath);
            }

        public string FileURI
            {
            get
                {
                return _fileURI;
                }
            set
                {
                _fileURI = value;
                OnPropertyChanged("FileURI");
                }
            }


        public void ReadFile()
            {
/*
              <#
              Description = "beskrivelse"
              Header = "Funksjonsnavn"
              Output = "True"
              [string]Username = "beskrivelse av CLI"
              [int]SomeNumber = "beskrivelse av somenumber"
              [bool]SomeBool = "beskrivelse av someBool"
              #>
*/
            string[] lines = System.IO.File.ReadAllLines(_fileURI);
            string desc, header, output;
            List<string> psArgumentList = new List<string>();
            int ii = 1;
            bool varSectionEnd = false;
            foreach (string line in lines)
                {
                // if line starts with #> break
                if(ii == 1) { desc = line; }
                if(ii == 2) { header = line; }
                if(ii == 3) { output = line; }
                if(ii > 3 && varSectionEnd == false)
                    {
                    psArgumentList.Add(line);
                    }
                ii++;
                }

            }

        }
    }

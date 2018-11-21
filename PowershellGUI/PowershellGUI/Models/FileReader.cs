using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PowershellGUI.Models
    {
    class FileReader : ObservableObject
        {
        public string FileURI { get; set; }

        private string scriptDescription;
        private string scriptHeader;
        private string scriptOutput;
        private List<string> scriptVariables;

        private string _fileContent;

        public string FileContent
            {
            get
                {
                return _fileContent;
                }
            set
                {
                _fileContent = value;
                OnPropertyChanged("FileContent");
                }
            }

        /// <summary>
        /// Constructor
        /// </summary>
        public FileReader()
            {
            scriptVariables   = new List<string>();
            _fileContent      = "";
            scriptDescription = "";
            scriptHeader      = "";
            scriptOutput      = "";
            }

        /// <summary>
        /// Parses the powershell script header's top area.
        /// </summary>
        /// <param name="lineNum"></param>
        /// <param name="line"></param>
        private void ParseScriptHeader(int lineNum, string line)
            {
            // logic to separate variable names from variable content
            const int description = 1;
            const int header      = 2;
            const int output      = 3;

            switch (lineNum)
                {
                case description:
                    {
                    scriptDescription = line;
                    break;
                    }
                case header:
                    {
                    scriptHeader = line;
                    break;
                    }
                case output:
                    {
                    scriptOutput = line;
                    break;
                    }
                }
            }

        /// <summary>
        /// Parses the Powershell script header's bottom area.
        /// </summary>
        /// <param name="line"></param>
        private void ParseScriptVariables(string line)
            {
            // logic to separate variablename from variable content

            string cleanOutput = ParseTextContents(line);
            scriptVariables.Add(cleanOutput);
            }

        private string ParseTextContents(string line)
            {
            Regex regx;

            return "";
            }


        /// <summary>
        /// Parses a powerscript file and saves the 
        /// header contents.
        /// </summary>
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

            foreach(Match match in Regex.Matches(inputString, "\"([^\"]*)\""))
    Console.WriteLine(match.ToString());
*/

            string[] lines         = System.IO.File.ReadAllLines(FileURI);
            string scriptHeaderEnd = "#>";
            int lineNum            = 0;
            int lastHeaderLine     = 3;

            foreach (string line in lines)
                {
                // if header is completely parsed
                if (line == scriptHeaderEnd)
                    {
                    break;
                    }
                // If description, header or output
                if (lineNum <= lastHeaderLine)
                    {
                    ParseScriptHeader(lineNum, line);
                    }
                // else if input variables to script
                else
                    {
                    ParseScriptVariables(line);
                    }
                lineNum++;
               // FileContent += line;
                }
            // output test
            /*
            foreach(string line in scriptVariables)
                {
                FileContent += line;
                }
            */
            }


        }
    }

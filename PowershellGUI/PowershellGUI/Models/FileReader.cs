using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowershellGUI.Models
    {
    class FileReader : ObservableObject
        {
        public string FileURI { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public FileReader()
            {
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

            // les fil der filsti er definert i FileURI
            // parse fil metadata
*/
            string[] lines = System.IO.File.ReadAllLines(FileURI);
            System.Console.WriteLine("test");
            /*
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
                */

            }

        }
    }

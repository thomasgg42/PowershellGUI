using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsGui.Models
    {
    public class DirectoryReader
        {
        private bool   _isScriptSelected;
        private string _scriptSelected;

        public bool IsScriptSelected
            {
            get
                {
                return _isScriptSelected;
                }
            set
                {
                _isScriptSelected = value;
                }
            }

        }
    }

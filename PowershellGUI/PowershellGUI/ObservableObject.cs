using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowershellGUI
    {
    /// <summary>
    /// Implements INotifyPropertyChanged allowing
    /// child-classes to propagate events.
    /// </summary>
    class ObservableObject : INotifyPropertyChanged
        {
        public event PropertyChangedEventHandler PropertyChanged;

        /*
         *  Raises the event.
         *  @name - Name of the changed property.
         */
        protected void OnPropertyChanged(string name)
            {
            PropertyChangedEventHandler handler = PropertyChanged;
            if(handler != null)
                {
                handler(this, new PropertyChangedEventArgs(name));
                }
            }
        }
    }

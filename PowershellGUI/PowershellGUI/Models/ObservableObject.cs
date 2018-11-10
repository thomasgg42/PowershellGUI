using System.ComponentModel;

namespace PowershellGUI.Models
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

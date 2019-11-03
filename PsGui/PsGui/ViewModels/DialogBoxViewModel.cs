using System.Windows.Controls;

namespace PsGui.ViewModels
{
   public partial class DialogBoxViewModel : UserControl
    {
        public string Message { private set; get; }
        public string Title   { private set; get; }

        public DialogBoxViewModel(string title, string msg)
        {
            Title = title;
            Message = msg;
        }
    }
}

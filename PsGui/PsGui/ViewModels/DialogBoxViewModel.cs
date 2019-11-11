using System.Windows.Controls;

namespace PsGui.ViewModels
{
    /// <summary>
    /// View model or the Material Design message box replacement.
    /// </summary>
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

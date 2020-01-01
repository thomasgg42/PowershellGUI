using System.Windows.Controls;

namespace PsGui.ViewModels
{
    /// <summary>
    /// View model of the Material Design Dialog Box. Replaces
    /// the default Windows.System.MessageBox in a asynchronous
    /// way.
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

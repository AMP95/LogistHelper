using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace CustomDialog
{
    public class QuestionDialog : StatementDialog
    {
        public ICommand DenyCommand { get; set; }
        public QuestionDialog(string message, string title) :
            base(message, title, DialogType.Question)
        {
            DenyCommand = new RelayCommand(() =>
            {
                Close?.Invoke(false);
            });
        }

    }
}

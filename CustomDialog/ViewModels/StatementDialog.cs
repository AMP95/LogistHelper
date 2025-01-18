using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared;
using System.Windows.Input;

namespace CustomDialog
{
    public class StatementDialog : ObservableObject, IWindowCloser
    {
        #region Private

        private string _title;
        private string _message;
        private DialogType _type;

        #endregion Private

        #region Public

        public Action<bool?> Close { get; set; }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public DialogType Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        #endregion Public

        public ICommand AcceptCommand { get; set; }

        public StatementDialog(string message, string title, DialogType type)
        {
            Message = message;
            Title = title;
            Type = type;

            AcceptCommand = new RelayCommand(() =>
            {
                Close?.Invoke(true);
            });
        }
    }
}

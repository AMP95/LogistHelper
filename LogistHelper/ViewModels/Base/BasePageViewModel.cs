using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace LogistHelper.ViewModels.Base
{
    public class BasePageViewModel : ObservableObject
    {
        public ICommand BackCommand { get; }

        public BasePageViewModel()
        {
            #region CommandsInit

            BackCommand = new RelayCommand(BackCommandExecutor);

            #endregion CommandsInit
        }

        protected virtual void BackCommandExecutor(){}
    }
}

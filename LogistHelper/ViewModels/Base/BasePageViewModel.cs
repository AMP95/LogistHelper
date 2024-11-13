using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LogistHelper.Services;
using System.Windows.Input;

namespace LogistHelper.ViewModels.Base
{
    public class BasePageViewModel : ObservableObject
    {
        public ICommand BackCommand { get; }
        public ICommand LogOutCommand { get; }

        public BasePageViewModel()
        {
            #region CommandsInit

            BackCommand = new RelayCommand(BackCommandExecutor);

            LogOutCommand = new RelayCommand(() => 
            { 
                NavigationService.Navigate(Models.PageType.Enter);
            });

            #endregion CommandsInit
        }

        protected virtual void BackCommandExecutor(){}
    }
}

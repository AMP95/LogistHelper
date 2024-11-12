using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LogistHelper.Models;
using LogistHelper.Services;
using System.Windows.Input;

namespace LogistHelper.ViewModels.Pages
{
    public class EnterPageViewModel : ObservableObject
    {
        public ICommand EnterCommand { get; }

        public EnterPageViewModel()
        {
            #region CommandsInit

            EnterCommand = new RelayCommand(() => 
            {
                NavigationService.Navigate(PageType.Menu);
            });

            #endregion CommandsInit
        }
    }
}

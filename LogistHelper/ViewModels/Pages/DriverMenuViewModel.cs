using CommunityToolkit.Mvvm.ComponentModel;
using LogistHelper.ViewModels.Base;
using LogistHelper.ViewModels.Views;

namespace LogistHelper.ViewModels.Pages
{
    public class DriverMenuViewModel : BasePageViewModel
    {
        private ObservableObject _content;

        private DriverListViewModel _list;
        private EditDriverViewModel _edit;
    }
}

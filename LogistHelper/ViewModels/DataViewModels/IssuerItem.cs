using CommunityToolkit.Mvvm.ComponentModel;

namespace LogistHelper.ViewModels.DataViewModels
{
    public class IssuerItem : ObservableObject
    {
        private string _code;
        private string _name;

        public string Code 
        {
            get => _code;
            set => SetProperty(ref _code, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
    }
}

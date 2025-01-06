using CommunityToolkit.Mvvm.ComponentModel;

namespace LogistHelper.ViewModels.DataViewModels
{
    public class StringItem : ObservableObject
    {
        private Guid _id;
        private string _item;

        public Guid Id 
        { 
            get => _id;
        }

        public string Item 
        {
            get => _item;
            set => SetProperty(ref _item, value);
        }

        public StringItem(string item)
        {
            _id = Guid.NewGuid();
            Item = item;
        }
        public StringItem()
        {
            _id = Guid.NewGuid();
        }
    }
}

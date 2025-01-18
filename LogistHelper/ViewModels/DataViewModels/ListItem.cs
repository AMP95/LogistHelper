using CommunityToolkit.Mvvm.ComponentModel;

namespace LogistHelper.ViewModels.DataViewModels
{
    public class ListItem<T> : ObservableObject
    {
        private Guid _id;
        private T _item;

        public Guid Id
        {
            get => _id;
        }

        public T Item
        {
            get => _item;
            set => SetProperty(ref _item, value);
        }

        public ListItem(T item)
        {
            _id = Guid.NewGuid();
            Item = item;
        }
        public ListItem()
        {
            _id = Guid.NewGuid();
        }
    }
}

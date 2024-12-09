using CommunityToolkit.Mvvm.ComponentModel;

namespace DTO
{

    //https://dadata.ru/  - данные API

    public class CompanyViewModel : ObservableObject
    {
        #region Private

        private string _name;
        private long _inn;
        private long _kpp;
        private string _address;
        private string _phones;
        private string _emails;

        #endregion Private

        public Guid Id { get; set; }

        public string Name 
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        public long Inn
        {
            get => _inn;
            set => SetProperty(ref _inn, value);
        }
        public long Kpp
        {
            get => _kpp;
            set => SetProperty(ref _kpp, value);
        }

        public string Address 
        {
            get => _address;
            set => SetProperty(ref _address, value);    
        }
        public string Phones
        {
            get => _phones;
            set => SetProperty(ref _phones, value);
        }
        public string Emails
        {
            get => _emails;
            set => SetProperty(ref _emails, value);
        }
    }
}

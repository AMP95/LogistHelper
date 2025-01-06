using CommunityToolkit.Mvvm.ComponentModel;
using DTOs;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Packaging;
using Windows.Devices.Sensors;

namespace LogistHelper.ViewModels.DataViewModels
{
    public class CompanyViewModel : ObservableObject, IDataErrorInfo
    {
        #region Private

        private int _number;

        protected CompanyDto _company;

        private ObservableCollection<StringItem> _phones;
        private ObservableCollection<StringItem> _emails;

        private string _inn;
        private string _kpp;

        #endregion Private

        #region Public

        
        public int Number 
        {
            get => _number;
            set=> SetProperty(ref _number, value);
        }

        public Guid Id
        {
            get => _company.Id;
        }

        public string Name
        {
            get => _company.Name;
            set
            {
                _company.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string Address
        {
            get => _company.Address;
            set
            {
                _company.Address = value;
                OnPropertyChanged(nameof(Address));
            }
        }
        public string Inn
        {
            get => _inn;
            set
            {
                SetProperty(ref _inn, value);
                _company.InnKpp = $"{Inn}";
                if (string.IsNullOrWhiteSpace(Kpp))
                {
                    _company.InnKpp += $"/{Kpp}";
                }
            }
        }

        public string Kpp
        {
            get => _kpp;
            set
            {
                SetProperty(ref _kpp, value);
                _company.InnKpp = $"{Inn}";
                if (string.IsNullOrWhiteSpace(Kpp))
                {
                    _company.InnKpp += $"/{Kpp}";
                }
            }
        }

        public ObservableCollection<StringItem> Phones
        {
            get => _phones;
            set => SetProperty(ref _phones, value);
        }
        public ObservableCollection<StringItem> Emails
        {
            get => _emails;
            set => SetProperty(ref _emails, value);
        }

        #endregion Public

        #region Validation

        public string this[string columnName] => _company[columnName];

        public string Error => _company.Error;

        #endregion Validation

        public CompanyViewModel(CompanyDto company)
        {
            _company = company;
            string[] innkpp = _company.InnKpp.Split('/');
            _inn = innkpp[0];
            if (innkpp.Length > 1) 
            { 
                _kpp = innkpp[1];
            }

            Phones = new ObservableCollection<StringItem>(company.Phones.Select(s => new StringItem(s)));
            Emails = new ObservableCollection<StringItem>(company.Emails.Select(s => new StringItem(s)));

        }

        public CompanyViewModel()
        {
            _company = new CompanyDto();

            Phones = new ObservableCollection<StringItem>();
            Emails = new ObservableCollection<StringItem>();
        }

        public CompanyDto GetDto()
        {
            _company.Phones = _phones.Select(s => s.Item).ToList();
            _company.Emails = _emails.Select(s => s.Item).ToList();
            return _company;
        }
    }
}

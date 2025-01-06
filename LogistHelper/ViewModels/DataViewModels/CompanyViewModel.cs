using CommunityToolkit.Mvvm.ComponentModel;
using DTOs;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace LogistHelper.ViewModels.DataViewModels
{
    public class CompanyViewModel : ObservableObject, IDataErrorInfo
    {
        #region Private

        protected CompanyDto _company;

        private ObservableCollection<StringItem> _phones;
        private ObservableCollection<StringItem> _emails;

        #endregion Private

        #region Public

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
        public string InnKpp
        {
            get => _company.InnKpp;
            set
            {
                _company.InnKpp = value;
                OnPropertyChanged(nameof(InnKpp));
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

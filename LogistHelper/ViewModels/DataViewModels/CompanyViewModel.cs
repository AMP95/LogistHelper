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

        private ObservableCollection<string> _phones;
        private ObservableCollection<string> _emails;

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

        public ObservableCollection<string> Phones
        {
            get => _phones;
            set => SetProperty(ref _phones, value);
        }
        public ObservableCollection<string> Emails
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

            Phones = new ObservableCollection<string>(company.Phones);
            Emails = new ObservableCollection<string>(company.Emails);

            Phones.CollectionChanged += Phones_CollectionChanged;
            Emails.CollectionChanged += Emails_CollectionChanged;
        }

        public CompanyViewModel()
        {
            _company = new CompanyDto();

            Phones = new ObservableCollection<string>();
            Emails = new ObservableCollection<string>();

            Phones.CollectionChanged += Phones_CollectionChanged;
            Emails.CollectionChanged += Emails_CollectionChanged;
        }

        public CompanyDto GetDto()
        {
            _company.Phones = _phones.ToList();
            _company.Emails = _emails.ToList();
            return _company;
        }
    }
}

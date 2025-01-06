using CommunityToolkit.Mvvm.ComponentModel;
using DTOs;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace LogistHelper.ViewModels.DataViewModels
{
    public class CompanyViewModel<T> : ObservableObject, IDataErrorInfo where T : CompanyDto
    {
        #region Private

        private int _number;

        protected T _company;

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

        public CompanyViewModel(T company)
        {
            _company = company;
            if (_company.InnKpp != null)
            {
                string[] innkpp = _company.InnKpp.Split('/');
                _inn = innkpp[0];
                if (innkpp.Length > 1)
                {
                    _kpp = innkpp[1];
                }
            }
            if (company.Phones != null)
            {
                Phones = new ObservableCollection<StringItem>(company.Phones.Select(s => new StringItem(s)));
            }
            else 
            {
                Phones = new ObservableCollection<StringItem>();
            }

            if (company.Emails != null)
            {
                Emails = new ObservableCollection<StringItem>(company.Emails.Select(s => new StringItem(s)));
            }
            else 
            {
                Emails = new ObservableCollection<StringItem>();
            }
        }


        public T GetDto()
        {
            _company.Phones = _phones.Select(s => s.Item).ToList();
            _company.Emails = _emails.Select(s => s.Item).ToList();
            return _company;
        }
    }

    public class ClientViewModel : CompanyViewModel<CompanyDto> 
    {
        public ClientViewModel() : base(new CompanyDto())
        {
        }
        public ClientViewModel(CompanyDto dto) : base(dto) { }
    }

    public class CarrierViewModel : CompanyViewModel<CarrierDto> 
    {
        public CarrierViewModel() : base(new CarrierDto()) { }
        public CarrierViewModel(CarrierDto dto) : base(dto) { }
        public VAT Vat
        {
            get => _company.Vat;
            set
            {
                _company.Vat = value;
                OnPropertyChanged(nameof(Vat));
            }
        }
    }

    public interface ICompanyVmFactory<T> where T : CompanyDto 
    {
        public CompanyViewModel<T> GetViewModel(T dto, int number);
        public CompanyViewModel<T> GetViewModel(T dto);
        public CompanyViewModel<T> GetViewModel();
    }

    public class ClientVmFactory : ICompanyVmFactory<CompanyDto>
    {
        public CompanyViewModel<CompanyDto> GetViewModel(CompanyDto dto, int number)
        {
            return new ClientViewModel(dto) { Number = number };
        }

        public CompanyViewModel<CompanyDto> GetViewModel()
        {
            return new ClientViewModel();
        }

        public CompanyViewModel<CompanyDto> GetViewModel(CompanyDto dto)
        {
            return new ClientViewModel(dto);
        }
    }

    public class CarrierVmFactory : ICompanyVmFactory<CarrierDto>
    {
        public CompanyViewModel<CarrierDto> GetViewModel(CarrierDto dto, int number)
        {
            return new CarrierViewModel(dto) { Number = number };
        }

        public CompanyViewModel<CarrierDto> GetViewModel()
        {
            return new CarrierViewModel();
        }

        public CompanyViewModel<CarrierDto> GetViewModel(CarrierDto dto)
        {
            return new CarrierViewModel(dto);
        }
    }
}

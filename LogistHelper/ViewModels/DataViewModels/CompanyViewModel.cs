using DTOs;
using LogistHelper.ViewModels.Base;
using System.Collections.ObjectModel;

namespace LogistHelper.ViewModels.DataViewModels
{

    public abstract class CompanyViewModel<T> : DataViewModel<T> where T : CompanyDto
    {
        #region Private

        private ObservableCollection<ListItem<string>> _phones;
        private ObservableCollection<ListItem<string>> _emails;

        private string _inn;
        private string _kpp;

        #endregion Private

        #region Public

        public string Name
        {
            get => _dto.Name;
            set
            {
                _dto.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string Address
        {
            get => _dto.Address;
            set
            {
                _dto.Address = value;
                OnPropertyChanged(nameof(Address));
            }
        }
        public string Inn
        {
            get => _inn;
            set
            {
                SetProperty(ref _inn, value);
                _dto.InnKpp = $"{Inn}";
                if (!string.IsNullOrWhiteSpace(Kpp))
                {
                    _dto.InnKpp += $"/{Kpp}";
                }
            }
        }

        public string Kpp
        {
            get => _kpp;
            set
            {
                SetProperty(ref _kpp, value);
                _dto.InnKpp = $"{Inn}";
                if (!string.IsNullOrWhiteSpace(Kpp))
                {
                    _dto.InnKpp += $"/{Kpp}";
                }
            }
        }

        public ObservableCollection<ListItem<string>> Phones
        {
            get => _phones;
            set => SetProperty(ref _phones, value);
        }
        public ObservableCollection<ListItem<string>> Emails
        {
            get => _emails;
            set => SetProperty(ref _emails, value);
        }

        #endregion Public

        public CompanyViewModel(T dto, int counter) : base(dto, counter)
        {
            if (dto != null)
            {
                if (dto.InnKpp != null)
                {
                    string[] innkpp = dto.InnKpp.Split('/');
                    _inn = innkpp[0];
                    if (innkpp.Length > 1)
                    {
                        _kpp = innkpp[1];
                    }
                }

                if (dto.Phones != null)
                {
                    Phones = new ObservableCollection<ListItem<string>>(dto.Phones.Select(s => new ListItem<string>(s)));
                }

                if (dto.Emails != null)
                {
                    Emails = new ObservableCollection<ListItem<string>>(dto.Emails.Select(s => new ListItem<string>(s)));
                }
            }
        }

        public CompanyViewModel(T dto) : this(dto, 0) { }

        public CompanyViewModel() : base() { }


        public override T GetDto()
        {
            if (_phones != null)
            {
                _dto.Phones = _phones.Select(s => s.Item).ToList();
            }
            if (_emails != null)
            {
                _dto.Emails = _emails.Select(s => s.Item).ToList();
            }
            return base.GetDto();
        }

    }

    public class ClientViewModel : CompanyViewModel<ClientDto> 
    {
        public ClientViewModel() : base() { }
        public ClientViewModel(ClientDto dto) : this(dto, 0) { }
        public ClientViewModel(ClientDto dto, int number) : base(dto, number) { }

        protected override void DefaultInit()
        {
            _dto = new ClientDto();
            Phones = new ObservableCollection<ListItem<string>>() { new ListItem<string>() };
            Emails = new ObservableCollection<ListItem<string>>() { new ListItem<string>() };
        }
    }

    public class CarrierViewModel : CompanyViewModel<CarrierDto> 
    {
        public CarrierViewModel() : base() { }
        public CarrierViewModel(CarrierDto dto) : this(dto, 0) { }
        public CarrierViewModel(CarrierDto dto, int counter) : base(dto, counter) { }
        public VAT Vat
        {
            get => _dto.Vat;
            set
            {
                _dto.Vat = value;
                OnPropertyChanged(nameof(Vat));
            }
        }

        protected override void DefaultInit()
        {
            _dto = new CarrierDto();
            Phones = new ObservableCollection<ListItem<string>>() { new ListItem<string>() };
            Emails = new ObservableCollection<ListItem<string>>() { new ListItem<string>() };
        }
    }

    public class ClientViewModelFactory : IViewModelFactory<ClientDto>
    {
        public DataViewModel<ClientDto> GetViewModel(ClientDto dto, int number)
        {
            return new ClientViewModel(dto,  number);
        }

        public DataViewModel<ClientDto> GetViewModel(ClientDto dto)
        {
            return new ClientViewModel(dto);
        }

        public DataViewModel<ClientDto> GetViewModel()
        {
            return new ClientViewModel();
        }
    }

    public class CarrierViewModelFactory : IViewModelFactory<CarrierDto>
    {
        public DataViewModel<CarrierDto> GetViewModel(CarrierDto dto, int number)
        {
            return new CarrierViewModel(dto, number);
        }

        public DataViewModel<CarrierDto> GetViewModel(CarrierDto dto)
        {
            return new CarrierViewModel(dto);
        }

        public DataViewModel<CarrierDto> GetViewModel()
        {
            return new CarrierViewModel();
        }
    }
}

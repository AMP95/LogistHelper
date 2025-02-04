using DTOs;
using LogistHelper.ViewModels.Base;
using System.Collections.ObjectModel;

namespace LogistHelper.ViewModels.DataViewModels
{

    public abstract class CompanyBaseViewModel<T> : DataViewModel<T> where T : CompanyBaseDto
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

        public CompanyBaseViewModel(T dto, int counter) : base(dto, counter)
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

        public CompanyBaseViewModel(T dto) : this(dto, 0) { }

        public CompanyBaseViewModel() : base() { }


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

    public class CompanyViewModel : CompanyBaseViewModel<CompanyDto> 
    {
        public CompanyType Type
        { 
            get => _dto.Type;
            set 
            { 
                _dto.Type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        public CompanyViewModel() : base() { }
        public CompanyViewModel(CompanyDto dto) : this(dto, 0) { }
        public CompanyViewModel(CompanyDto dto, int number) : base(dto, number) { }

        protected override void DefaultInit()
        {
            _dto = new CompanyDto();
            Phones = new ObservableCollection<ListItem<string>>() { new ListItem<string>() };
            Emails = new ObservableCollection<ListItem<string>>() { new ListItem<string>() };
        }
    }

    public class CarrierViewModel : CompanyBaseViewModel<CarrierDto> 
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

    public class CompanyViewModelFactory : IViewModelFactory<CompanyDto>
    {
        public DataViewModel<CompanyDto> GetViewModel(CompanyDto dto, int number)
        {
            return new CompanyViewModel(dto,  number);
        }

        public DataViewModel<CompanyDto> GetViewModel(CompanyDto dto)
        {
            return new CompanyViewModel(dto);
        }

        public DataViewModel<CompanyDto> GetViewModel()
        {
            return new CompanyViewModel();
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

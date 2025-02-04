using DTOs.Dtos;
using LogistHelper.ViewModels.Base;
using System.Collections.ObjectModel;

namespace LogistHelper.ViewModels.DataViewModels
{
    public class TeplateViewModel : DataViewModel<TemplateDto>
    {
        private ObservableCollection<AdditionalsViewModel> _additionals;

        public string Name 
        { 
            get => _dto.Name;
            set 
            {
                _dto.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public ObservableCollection<AdditionalsViewModel> Additionals 
        {
            get => _additionals;
            set => SetProperty(ref _additionals, value);
        }



        public TeplateViewModel(TemplateDto dto, int counter) : base(dto, counter)
        {
            if (dto != null)
            {
                Additionals = new ObservableCollection<AdditionalsViewModel>(_dto.Additionals.Select(a => new AdditionalsViewModel(a)));
            }
        }

        public TeplateViewModel(TemplateDto dto) : this(dto, 0) { }

        public TeplateViewModel() : base() { }

        protected override void DefaultInit()
        {
            _dto = new TemplateDto();
            Additionals = new ObservableCollection<AdditionalsViewModel>();
        }

        public override TemplateDto GetDto()
        {
            _dto.Additionals = Additionals.Select(a => new AdditionalDto() { Id = a.Id, Name = a.Name, Description = a.Description }).ToList();
            return base.GetDto();
        }
    }

    public class TemplateViewModelFactory : IViewModelFactory<TemplateDto>
    {
        public DataViewModel<TemplateDto> GetViewModel(TemplateDto dto, int number)
        {
            return new TeplateViewModel(dto, number);
        }

        public DataViewModel<TemplateDto> GetViewModel(TemplateDto dto)
        {
            return new TeplateViewModel(dto);
        }

        public DataViewModel<TemplateDto> GetViewModel()
        {
            return new TeplateViewModel();
        }
    }


    public class AdditionalsViewModel : DataViewModel<AdditionalDto>
    {
        public string Name
        {
            get => _dto.Name;
            set
            {
                _dto.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Description
        {
            get => _dto.Description;
            set
            {
                _dto.Description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public AdditionalsViewModel(AdditionalDto dto, int counter) : base(dto, counter) { }

        public AdditionalsViewModel(AdditionalDto dto) : this(dto, 0) { }

        public AdditionalsViewModel() : base() { }

        protected override void DefaultInit()
        {
            _dto = new AdditionalDto();
        }
    }
}

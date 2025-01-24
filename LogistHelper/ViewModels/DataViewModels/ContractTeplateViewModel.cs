using DTOs.Dtos;
using LogistHelper.ViewModels.Base;

namespace LogistHelper.ViewModels.DataViewModels
{
    public class ContractTeplateViewModel : DataViewModel<ContractTemplateDto>
    {
        private FileViewModel _file;

        public string Name 
        { 
            get => _dto.Name;
            set 
            {
                _dto.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public FileViewModel File 
        {
            get => _file;
            set => SetProperty(ref _file, value);
        }

        public ContractTeplateViewModel(ContractTemplateDto dto, int counter) : base(dto, counter)
        {
            if (dto != null)
            {
                _file = new FileViewModel(_dto.File);
            }
        }

        public ContractTeplateViewModel(ContractTemplateDto dto) : this(dto, 0) { }

        public ContractTeplateViewModel() : base() { }

        protected override void DefaultInit()
        {
            _dto = new ContractTemplateDto();
            File = new FileViewModel();
        }

        public override ContractTemplateDto GetDto()
        {
            _dto.File = File.GetDto();
            return base.GetDto();
        }
    }

    public class TemplateViewModelFactory : IViewModelFactory<ContractTemplateDto>
    {
        public DataViewModel<ContractTemplateDto> GetViewModel(ContractTemplateDto dto, int number)
        {
            return new ContractTeplateViewModel(dto, number);
        }

        public DataViewModel<ContractTemplateDto> GetViewModel(ContractTemplateDto dto)
        {
            return new ContractTeplateViewModel(dto);
        }

        public DataViewModel<ContractTemplateDto> GetViewModel()
        {
            return new ContractTeplateViewModel();
        }
    }
}

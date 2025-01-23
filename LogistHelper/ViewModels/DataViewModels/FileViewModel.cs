using DTOs.Dtos;
using LogistHelper.ViewModels.Base;
using System.IO;

namespace LogistHelper.ViewModels.DataViewModels
{
    public class FileViewModel : DataViewModel<FileDto>
    {
        #region Private

        private string _localFullFilePath;
        private string _extencion;

        #endregion Private

        #region Public

        #endregion Public

        public string LocalFullFilePath 
        {
            get => _localFullFilePath;
            set 
            { 
                SetProperty(ref _localFullFilePath, value);
                if (!string.IsNullOrWhiteSpace(_localFullFilePath))
                {
                    LocalNameWithExtension = Path.GetFileName(value);
                }
            }
        }

        public string LocalNameWithExtension 
        {
            get => _dto.FileNameWithExtencion;
            set 
            { 
                _dto.FileNameWithExtencion = value;
                OnPropertyChanged(nameof(LocalNameWithExtension));
                if (!string.IsNullOrWhiteSpace(_dto.FileNameWithExtencion))
                {
                    Extension = Path.GetExtension(value);
                }
            }
        }

        public string Extension 
        {
            get => _extencion;
            set => SetProperty(ref _extencion, value);
        }

        public Type DtoType 
        { 
            get => _dto.DtoType;
            set 
            { 
                _dto.DtoType = value;
                OnPropertyChanged(nameof(DtoType));
            }
        }

        public string ServerCatalog 
        {
            get => _dto.Catalog;
            set
            {
                _dto.Catalog = value;
                OnPropertyChanged(nameof(ServerCatalog));
            }
        }

        protected override void DefaultInit()
        {
            _dto = new FileDto();
        }

        public FileViewModel(FileDto dto, int counter) : base(dto, counter) { }
        public FileViewModel(FileDto dto) : this(dto, 0) { }
        public FileViewModel() : base() { }
    }

    public class FileViewModelFactory : IViewModelFactory<FileDto>
    {
        public DataViewModel<FileDto> GetViewModel(FileDto dto, int number)
        {
            return new FileViewModel(dto, number);
        }

        public DataViewModel<FileDto> GetViewModel(FileDto dto)
        {
            return new FileViewModel(dto);
        }

        public DataViewModel<FileDto> GetViewModel()
        {
            return new FileViewModel();
        }
    }
}

using CommunityToolkit.Mvvm.Input;
using DTOs.Dtos;
using IronWord;
using LogistHelper.ViewModels.Base;
using LogistHelper.ViewModels.DataViewModels;
using Shared;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Utilities;

namespace LogistHelper.ViewModels.Views
{

    public class EditTemplateViewModel : MainEditViewModel<ContractTemplateDto>
    {
        #region Private

        private ContractTeplateViewModel _template;

        private IFileLoader<FileViewModel> _fileLoader;
        private ObservableCollection<ListItem<FileViewModel>> _files;

        private List<string> _allowedExtensions;

        private ObservableCollection<BookMarkDto> _requiredBookMarks;

        private bool _allowAddFile;

        #endregion Private

        #region Public


        public ObservableCollection<ListItem<FileViewModel>> Files
        {
            get => _files;
            set => SetProperty(ref _files, value);
        }
        public List<string> AllowedExtensions
        {
            get => _allowedExtensions;
            set => SetProperty(ref _allowedExtensions, value);
        }
        public ObservableCollection<BookMarkDto> RequiredBookMarks
        {
            get => _requiredBookMarks;
            set => SetProperty(ref _requiredBookMarks, value);
        }

        public bool IsAllowToAddFiles 
        {
            get => _allowAddFile;
            set => SetProperty(ref _allowAddFile, value);
        }

        #endregion Public

        public ICommand RemoveFileCommand { get; set; }

        public EditTemplateViewModel(IDataAccess dataAccess, 
                                     IViewModelFactory<ContractTemplateDto> factory, 
                                     IDialog dialog,
                                     IFileLoader<FileViewModel> fileLoader) : base(dataAccess, factory, dialog)
        {
            _fileLoader = fileLoader;
            Files = new ObservableCollection<ListItem<FileViewModel>>();
            IsAllowToAddFiles = true;

            AllowedExtensions = new List<string>()
            {
                ".doc", ".docx"
            };

            RemoveFileCommand = new RelayCommand<Guid>(async (id) =>
            {
                ListItem<FileViewModel> item = Files.FirstOrDefault(i => i.Id == id);

                Files.Remove(item);

                if (item.Item.Id != Guid.Empty)
                {
                    IAccessResult<bool> result = await _access.DeleteAsync<FileDto>(item.Item.Id);
                }

                if (Files.Count == 0) 
                {
                    IsAllowToAddFiles = true;
                }
            });
        }

        public override async Task Load(Guid id)
        {
            await base.Load(id);

            _template = EditedViewModel as ContractTeplateViewModel;

            IAccessResult<IEnumerable<BookMarkDto>> bookMarks = await _access.GetFilteredAsync<BookMarkDto>(nameof(BookMarkDto.Name), "name");
            if (bookMarks.IsSuccess) 
            {
                RequiredBookMarks = new ObservableCollection<BookMarkDto>(bookMarks.Result);
            }
        }

        public async override Task Save()
        {
            IsBlock = true;
            BlockText = "Сохранение";

            if (await SaveEntity())
            {
                FileViewModel file = Files.FirstOrDefault().Item;

                file.DtoId = EditedViewModel.Id;
                file.DtoType = nameof(ContractTemplateDto);
                file.ServerCatalog = $"Templates";
                await _fileLoader.UploadFile(EditedViewModel.Id, file);

                _dialog.ShowSuccess("ТС сохранено в базу данных");

                if (EditedViewModel.Id == Guid.Empty)
                {
                    Load(Guid.Empty);
                }
            }
            else
            {
                _dialog.ShowError("Не удалось сохранить изменения", "Сохранение");
            }

            IsBlock = false;
        }

        public override bool CheckSave()
        {
            if (string.IsNullOrWhiteSpace(_template.Name)) 
            {
                _dialog.ShowError($"Необходимо указать название шаблона");
                return false;
            }

            if (!Files.Any()) 
            {
                _dialog.ShowError($"Добавьте файл шаблона");
                return false;
            }

            FileViewModel file = Files.FirstOrDefault().Item;

            WordDocument doc = new WordDocument(file.LocalFullFilePath);

            foreach (BookMarkDto mark in RequiredBookMarks) 
            {
                if (!doc.Texts.Any(t => t.Text.Contains(mark.InsertView))) 
                {
                    _dialog.ShowError($"Отсутствует закладка для {mark.Name}");
                    return false;
                }
            }

            return true;
        }
    }
}

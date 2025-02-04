using CommunityToolkit.Mvvm.Input;
using DTOs;
using DTOs.Dtos;
using LogistHelper.Models;
using LogistHelper.Services;
using LogistHelper.ViewModels.Base;
using LogistHelper.ViewModels.DataViewModels;
using Models.Suggest;
using Shared;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Utilities;

namespace LogistHelper.ViewModels.Views
{
    public class EditCompanyViewModel<T> : MainEditViewModel<T> where T : CompanyBaseDto
    {
        private CompanyBaseViewModel<T> _company;

        private IDataSuggest<CompanySuggestItem> _dataSuggest;
        private IEnumerable<CompanySuggestItem> _companiesList;
        private CompanySuggestItem _selectedCompany;

        #region Public

        public IEnumerable<CompanySuggestItem> CompaniesList 
        {
            get => _companiesList;
            set => SetProperty(ref _companiesList, value);
        }
        public CompanySuggestItem SelectedCompany
        {
            get => _selectedCompany;
            set
            {
                SetProperty(ref _selectedCompany, value);
                if (SelectedCompany != null)
                {
                    _company.Name = value.Name;
                    _company.Inn = value.Inn;
                    _company.Kpp = value.Kpp;
                    _company.Address = value.Address;
                }
                else 
                {
                    _company.Name = string.Empty;
                    _company.Inn = string.Empty;
                    _company.Kpp = string.Empty;
                    _company.Address = string.Empty;
                }
            }
        }


        #endregion Public

        #region Commands

        public ICommand DeleteEmailCommand { get; set; }
        public ICommand AddEmailCommand { get; set; }
        public ICommand DeletePhoneCommand { get; set; }
        public ICommand AddPhoneCommand { get; set; }
        public ICommand SearchCompanyCommand { get; set; }

        #endregion Commands

        public EditCompanyViewModel(IDataAccess dataAccess,
                                    IViewModelFactory<T> factory, 
                                    IMessageDialog dialog,
                                    IDataSuggest<CompanySuggestItem> dataSuggest) : base(dataAccess, factory, dialog)
        {
            _dataSuggest = dataSuggest;

            #region CommandsInit

            AddPhoneCommand = new RelayCommand(() =>
            {
                _company.Phones.Add(new ListItem<string>());
            });

            AddEmailCommand = new RelayCommand(() =>
            {
                _company.Emails.Add(new ListItem<string>());
            });

            DeleteEmailCommand = new RelayCommand<Guid>((id) =>
            {
                ListItem<string> item = _company.Emails.FirstOrDefault(e => e.Id == id);
                if (item != null)
                {
                    _company.Emails.Remove(item);
                }
            });

            DeletePhoneCommand = new RelayCommand<Guid>((id) =>
            {
                ListItem<string> item = _company.Phones.FirstOrDefault(e => e.Id == id);
                if (item != null)
                {
                    _company.Phones.Remove(item);
                }
            });

            SearchCompanyCommand = new RelayCommand<string>(async (searchString) =>
            {
                CompaniesList = await _dataSuggest.SuggestAsync(searchString);
            });

            #endregion CommandsInit
        }

        public override async Task Load(Guid id)
        {
            await base.Load(id);
            _company = EditedViewModel as CompanyBaseViewModel<T>;

            var companies = await _dataSuggest.SuggestAsync(_company.Name);
            _selectedCompany = companies.FirstOrDefault();
            OnPropertyChanged(nameof(SelectedCompany));
        }

        public override bool CheckSave()
        {
            if (string.IsNullOrWhiteSpace(_company.Name)) 
            {
                _dialog.ShowError("Необходимо указать название организации");
                return false;
            }
            if (string.IsNullOrWhiteSpace(_company.Address))
            {
                _dialog.ShowError("Необходимо указать адрес организации");
                return false;
            }
            if (string.IsNullOrWhiteSpace(_company.Inn))
            {
                _dialog.ShowError("Необходимо указать ИНН организации");
                return false;
            }
            if (!_company.Phones.Any()) 
            {
                _dialog.ShowError("Необходимо указать номер телефона для связи");
                return false;
            }
            if (_company.Phones.Any(p=> !RegexService.CheckPhone(p.Item)))
            {
                _dialog.ShowError("Необходимо верно указать все номера телефонов");
                return false;
            }
            if (!_company.Emails.Any()) 
            {
                _dialog.ShowError("Необходимо указать электронный адрес");
                return false;
            }
            if (_company.Emails.Any(p => !RegexService.CheckEmail(p.Item)))
            {
                _dialog.ShowError("Необходимо верно указать все электронные адреса");
                return false;
            }
            return true;
        }
    }

    public class EditClientViewModel : EditCompanyViewModel<CompanyDto>
    {
        public EditClientViewModel(IDataAccess dataAccess, 
                                   IViewModelFactory<CompanyDto> factory, 
                                   IMessageDialog dialog, 
                                   IDataSuggest<CompanySuggestItem> dataSuggest) : base(dataAccess, factory, dialog, dataSuggest)
        {
        }

        public override Task Save()
        {
            (EditedViewModel as CompanyViewModel).Type = CompanyType.Client;
            return base.Save();
        }
    }

    public class EditCarrierViewModel : EditCompanyViewModel<CarrierDto>
    {
        private IFileLoader<FileViewModel> _fileLoader;
        private ObservableCollection<ListItem<FileViewModel>> _files;

        private CarrierViewModel _carrier;

        private bool _isWithVat;
        private bool _isWithoutVat;

        public bool IsWithVat 
        {
            get => _isWithVat;
            set => SetProperty(ref _isWithVat, value);
        }
        public bool IsWithoutVat
        {
            get => _isWithoutVat;
            set => SetProperty(ref _isWithoutVat, value);
        }

        public ObservableCollection<ListItem<FileViewModel>> Files
        {
            get => _files;
            set => SetProperty(ref _files, value);
        }

        public ICommand DownloadFileCommand { get; set; }
        public ICommand RemoveFileCommand { get; set; }

        public EditCarrierViewModel(IDataAccess dataAccess, 
                                    IViewModelFactory<CarrierDto> factory, 
                                    IMessageDialog dialog, 
                                    IDataSuggest<CompanySuggestItem> dataSuggest,
                                    IFileLoader<FileViewModel> fileLoader) : base(dataAccess, factory, dialog, dataSuggest)
        {
            _fileLoader = fileLoader;

            Files = new ObservableCollection<ListItem<FileViewModel>>();

            DownloadFileCommand = new RelayCommand<LoadPackage>(async (package) =>
            {
                if (package.FileToLoad.Any())
                {
                    if (await _fileLoader.DownloadFiles(package.SavePath, package.FileToLoad))
                    {
                        _dialog.ShowSuccess("Файлы сохранены");
                    }
                    else
                    {
                        _dialog.ShowError("Ошибка сохранения файлов");
                    }
                }
            });

            RemoveFileCommand = new RelayCommand<Guid>(async (id) =>
            {
                ListItem<FileViewModel> item = Files.FirstOrDefault(i => i.Id == id);
                Files.Remove(item);

                if (item.Item.Id != Guid.Empty)
                {
                    IAccessResult<bool> result = await _access.DeleteAsync<FileDto>(item.Item.Id);
                }
            });
        }

        public override async Task Load(Guid id)
        {
            await base.Load(id);

            if (EditedViewModel != null)
            {

                _carrier = EditedViewModel as CarrierViewModel;

                if (_carrier.Vat == VAT.With)
                {
                    IsWithVat = true;
                    IsWithoutVat = false;
                }
                else
                {
                    IsWithVat = false;
                    IsWithoutVat = true;
                }

                IAccessResult<IEnumerable<FileDto>> files = await _access.GetFilteredAsync<FileDto>(nameof(FileDto.DtoId), EditedViewModel.Id.ToString());

                Files = new ObservableCollection<ListItem<FileViewModel>>(files.Result.Select(f => new ListItem<FileViewModel>(new FileViewModel(f))));

            }

        }

        public async override Task Save()
        {
            IsBlock = true;
            BlockText = "Сохранение";

            if (await SaveEntity())
            {
                foreach (var file in Files)
                {
                    file.Item.DtoId = EditedViewModel.Id;
                    file.Item.DtoType = nameof(CarrierDto);
                    file.Item.ServerCatalog = $"{_carrier.Name}".Replace(" ", "_");
                }

                await _fileLoader.UploadFiles(EditedViewModel.Id, Files.Select(f => f.Item).Where(f => f.Id == Guid.Empty));

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


        protected override Task<bool> SaveEntity()
        {
            if (IsWithVat)
            {
                _carrier.Vat = VAT.With;
            }
            else
            {
                _carrier.Vat = VAT.Without;
            }

            return base.SaveEntity();
        }

    }
}

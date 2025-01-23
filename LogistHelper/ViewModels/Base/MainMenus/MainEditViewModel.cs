using CommunityToolkit.Mvvm.Input;
using DTOs.Dtos;
using Shared;
using System.Windows.Input;

namespace LogistHelper.ViewModels.Base
{
    public class MainEditViewModel<T> : BlockedViewModel, IMainEditView<T> where T : IDto
    {
        #region Private

        protected IDialog _dialog;
        protected IDataAccess _access;
        protected IViewModelFactory<T> _factory;

        private DataViewModel<T> _editedViewModel;

        #endregion Private

        public IMainMenuPage<T> MainParent { get; set; }

        public DataViewModel<T> EditedViewModel 
        { 
            get => _editedViewModel;
            set => SetProperty(ref _editedViewModel, value);
        }

        #region Commands

        public ICommand SaveCommand { get; set; }
        public ICommand SaveAndCloseCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand BackCommand { get; set; }

        #endregion Commands

        public MainEditViewModel(IDataAccess dataAccess, 
                                 IViewModelFactory<T> factory, 
                                 IDialog dialog)
        {
            _dialog = dialog;
            _factory = factory;
            _access = dataAccess;

            #region CommanstInit

            SaveCommand = new RelayCommand(async () =>
            {
                if (CheckSave() && _dialog.ShowSure("Сохранить изменения"))
                {
                    await Save();
                }
            });

            SaveAndCloseCommand = new RelayCommand(async() => 
            {
                if (CheckSave() && _dialog.ShowSure("Сохранить изменения"))
                {
                    await SaveAndClose();
                }
            });

            CancelCommand = new RelayCommand(async () =>
            {
                if (_dialog.ShowSure("Отменить изменения"))
                {
                    await Load(_editedViewModel.Id);
                }
            });

            BackCommand = new RelayCommand(() =>
            {
                MainParent.SwitchToList();
                Clear();
            });


            #endregion CommanstInit
        }

        public virtual void Clear() 
        {
            EditedViewModel = null;
        }

        public virtual async Task Load(Guid id) 
        {
            if (id == Guid.Empty)
            {
                EditedViewModel = _factory.GetViewModel();
            }
            else
            {
                IsBlock = true;
                BlockText = "Загрузка";

                IAccessResult<T> result = await _access.GetIdAsync<T>(id);

                if (result.IsSuccess)
                {
                    EditedViewModel = _factory.GetViewModel(result.Result);
                }

                IsBlock = false;
            }
        }

        protected virtual async Task<bool> SaveEntity() 
        {
            if (EditedViewModel.Id == Guid.Empty)
            {
                IAccessResult<Guid> addResult = await _access.AddAsync(EditedViewModel.GetDto());
                if (addResult.IsSuccess)
                {
                    EditedViewModel.Id = addResult.Result;
                }
                return addResult.IsSuccess;
            }
            else
            {
                IAccessResult<bool> updateResult = await _access.UpdateAsync(EditedViewModel.GetDto());
                return updateResult.IsSuccess;
            }
        }

        public virtual bool CheckSave() 
        {
            return true;
        }

        public virtual async Task Save() 
        {
            IsBlock = true;
            BlockText = "Сохранение";

            if (await SaveEntity())
            {
                _dialog.ShowSuccess("Сохранение");

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

        public virtual async Task SaveAndClose()
        {
            IsBlock = true;
            BlockText = "Сохранение";

            if (await SaveEntity())
            {
                _dialog.ShowSuccess("Сохранение");
                MainParent.SwitchToList();
            }
            else
            {
                _dialog.ShowError("Не удалось сохранить изменения", "Сохранение");
            }

            IsBlock = false;
        }
    }
}

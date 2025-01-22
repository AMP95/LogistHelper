using CommunityToolkit.Mvvm.Input;
using DTOs.Dtos;
using LogistHelper.Models.Settings;
using Shared;

namespace LogistHelper.ViewModels.Base
{
    public class SubEditViewModel<T> : MainEditViewModel<T>, ISubEditView<T> where T : IDto
    {
        public SubEditViewModel(ISettingsRepository<Settings> repository, 
                                IViewModelFactory<T> factory, 
                                IDialog dialog) : base(repository, factory, dialog)
        {

            #region CommandInit

            BackCommand = new RelayCommand(() =>
            {
                SubParent?.SwitchToList(MainId);
                Clear();
            });

            #endregion CommandInit
        }

        public Guid MainId { get ; set ; }

        public ISubMenuPage<T> SubParent { get ; set ; }

        public async Task Load(Guid subId, Guid mainId)
        {
            if (mainId == Guid.Empty) 
            { 
                return;
            }
            MainId = mainId;

            await Load(subId);
        }

        public async override Task SaveAndClose()
        {
            IsBlock = true;
            BlockText = "Сохранение";

            if (await SaveEntity())
            {
                _dialog.ShowSuccess("Сохранение");
                SubParent?.SwitchToList(MainId);
            }
            else
            {
                _dialog.ShowError("Не удалось сохранить изменения", "Сохранение");
            }

            IsBlock = false;
        }
    }
}

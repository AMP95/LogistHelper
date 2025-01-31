using CommunityToolkit.Mvvm.Input;
using DTOs.Dtos;
using LogistHelper.ViewModels.Base;
using LogistHelper.ViewModels.DataViewModels;
using Shared;
using System.Windows.Input;
using Utilities;

namespace LogistHelper.ViewModels.Views
{
    public class EditLogistViewModel : MainEditViewModel<LogistDto>
    {
        private LogistViewModel _viewModel;
        private IAuthDialog<LogistDto> _authDialog;
        private IHashService _hashService;


        #region Commands

        public ICommand ChangePasswordCommand { get; set; }

        #endregion Commands

        public EditLogistViewModel(IDataAccess dataAccess, 
                                   IViewModelFactory<LogistDto> factory, 
                                   IMessageDialog dialog,
                                   IAuthDialog<LogistDto> authDialog,
                                   IHashService hashService) : base(dataAccess, factory, dialog)
        {
            _authDialog = authDialog;
            _hashService = hashService;

            ChangePasswordCommand = new RelayCommand(async () => 
            {
                if (EditedViewModel.Id != Guid.Empty) 
                {
                    if (!_dialog.ShowSure("Изменение пароля пользователя")) 
                    {
                        return;
                    }
                }

                _viewModel.PasswordState = DTOs.PasswordState.OnReset;
                _authDialog.ShowPasswordChange(_viewModel.GetDto(), _access, _hashService);
            });
        }


        public override async Task Load(Guid id)
        {
            await base.Load(id);

            _viewModel = EditedViewModel as LogistViewModel;
        }
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DTOs.Dtos;
using LogistHelper.Models;
using LogistHelper.Services;
using Shared;
using System.Windows.Controls;
using System.Windows.Input;
using Utilities;

namespace LogistHelper.ViewModels.Pages
{
    public class EnterPageViewModel : ObservableObject
    {
        private IMessageDialog _dialog;
        private IDataAccess _access;
        private IHashService _hashService;
        private IAuthDialog<LogistDto> _authDialog;

        private string _login;


        public string Login 
        {
            get => _login;
            set => SetProperty(ref _login, value);
        }

        public ICommand EnterCommand { get; }

        public EnterPageViewModel(IMessageDialog dialog, IAuthDialog<LogistDto> authDialog, IDataAccess dataAccess, IHashService hashService)
        {
            _access = dataAccess;
            _dialog = dialog;
            _hashService = hashService;
            _authDialog = authDialog;

            _access.Logout();
            Login = LogistService.EnteredLogist?.Login;

            #region CommandsInit

            EnterCommand = new RelayCommand<PasswordBox>(async (box) => 
            {
                IAccessResult<LogistDto> loginResult = await _access.Login(new LogistDto() { Login = Login, Password = _hashService.GetHash(box.Password) });

                if (loginResult.IsSuccess) 
                {
                    LogistDto dto = loginResult.Result;

                    if (loginResult.Result.PasswordState == DTOs.PasswordState.OnReset)
                    {
                        if (_dialog.ShowQuestion("Вы авторизовались по временному паролю. Необходимо установить постоянный пароль."))
                        {

                            dto.PasswordState = DTOs.PasswordState.Active;
                            dto.Password = box.Password;

                            _authDialog.ShowPasswordChange(dto, _access, _hashService);
                        }
                    }
                    else 
                    {
                        LogistService.EnteredLogist = dto;
                        NavigationService.Navigate(PageType.MainMenu);
                    }
                }
                else
                {
                    _dialog.ShowError(loginResult.ErrorMessage);
                }
            });

            #endregion CommandsInit
        }
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DTOs.Dtos;
using Shared;
using System.Windows.Controls;
using System.Windows.Input;
using Utilities;

namespace CustomDialog
{
    public class PasswordChangeViewModel : ObservableObject, IWindowCloser
    {
        #region Private

        private IMessageDialog _dialog;
        private IDataAccess _access;
        private IHashService _hashService;
        private LogistDto _user;
        private string _title;

        #endregion Private

        public string Title 
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public Action<bool?> Close { get; set; }


        public ICommand CancelCommand { get; }
        public ICommand AcceptCommand { get; }

        public PasswordChangeViewModel(LogistDto dto,
                                       IMessageDialog dialog, 
                                       IDataAccess access,
                                       IHashService hashService)
        {
            _dialog = dialog;
            _access = access;
            _hashService = hashService;
            _user = dto;

            #region CommandInit

            CancelCommand = new RelayCommand(() => 
            {
                if (_dialog.ShowSure("Отмена установки пароля")) 
                {
                    Close?.Invoke(false);
                }
            });

            AcceptCommand = new RelayCommand<List<object>>(async (objects) => 
            {
                if (objects != null && objects.Any()) 
                {
                    List<PasswordBox> pwds = objects.Cast<PasswordBox>().ToList();

                    string first = pwds[0].Password;
                    string second = pwds[1].Password;

                    if (first.Equals(_user.Password)) 
                    {
                        _dialog.ShowError("Пароль не может совпадать с временным паролем");
                        return;

                    }
                    if (!first.Equals(second)) 
                    {
                        _dialog.ShowError("Пароль и его подтверждение не совпадают");
                        return;
                    }

                    _user.Password = _hashService.GetHash(first);

                    bool boolResult = true;

                    if (_user.Id != Guid.Empty)
                    {
                        IAccessResult<bool> result = await _access.UpdateAsync<LogistDto>(_user);
                        boolResult = result.IsSuccess;
                    }

                    if (!boolResult)
                    {
                        _dialog.ShowError("Не удалось сохранить пароль");
                        
                    }
                    else 
                    {
                        Close?.Invoke(true);
                    }
                }
            });

            #endregion CommandInit
        }
    }
}

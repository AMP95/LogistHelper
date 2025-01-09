﻿using CommunityToolkit.Mvvm.Input;
using DTOs.Dtos;
using LogistHelper.Models.Settings;
using ServerClient;
using Shared;
using System.Windows.Input;

namespace LogistHelper.ViewModels.Base
{
    public class EditViewModel<T> : BlockedViewModel where T : IDto
    {
        #region Private

        protected IDialog _dialog;
        protected Settings _settings;
        protected ApiClient _client;

        protected IViewModelFactory<T> _factory;
        private DataViewModel<T> _editedViewModel;

        #endregion Private

        public MenuPageViewModel<T> Parent { get; set; }

        public DataViewModel<T> EditedViewModel 
        { 
            get => _editedViewModel;
            set => SetProperty(ref _editedViewModel, value);
        }

        #region Commands

        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand BackCommand { get; set; }

        #endregion Commands

        public EditViewModel(ISettingsRepository<Settings> repository, 
                             IViewModelFactory<T> factory, 
                             IDialog dialog)
        {
            _settings = repository.GetSettings();
            _dialog = dialog;
            _factory = factory;

            _client = new ApiClient(_settings.ServerUri);

            #region CommanstInit

            SaveCommand = new RelayCommand(async () =>
            {
                if (_dialog.ShowSure("Сохранить изменения"))
                {
                    await Save();
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
                Parent.SwitchToList();
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

                RequestResult<T> result = await _client.GetId<T>(id);

                if (result.IsSuccess)
                {
                    EditedViewModel = _factory.GetViewModel(result.Result);
                }

                IsBlock = false;
            }
        }

        public virtual async Task Save() 
        {
            IsBlock = true;
            BlockText = "Сохранение";

            RequestResult<bool> result;

            if (EditedViewModel.Id == Guid.Empty)
            {
                result = await _client.Add(EditedViewModel.GetDto());
            }
            else
            {
                result = await _client.Update(EditedViewModel.GetDto());
            }

            if (result.IsSuccess)
            {
                _dialog.ShowSuccess("Сохранение");
            }
            else
            {
                _dialog.ShowError("Не удалось сохранить изменения", "Сохранение");
            }

            IsBlock = false;
        }
    }
}

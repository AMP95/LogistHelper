﻿using CommunityToolkit.Mvvm.ComponentModel;
using DTOs.Dtos;
using System.ComponentModel;

namespace LogistHelper.ViewModels.Base
{
    public abstract class DataViewModel<TDto> : ObservableObject where TDto : IDto, IDataErrorInfo
    {
        private int _counter;

        protected TDto _dto;

        public int Counter
        {
            get => _counter;
            set => SetProperty(ref _counter, value);
        }

        public Guid Id
        {
            get => _dto.Id;
        }

        #region Validation

        public string this[string columnName] => _dto[columnName];

        public string Error => _dto.Error;

        #endregion Validation

        public DataViewModel(TDto dto, int counter)
        {
            if (dto == null)
            {
                DefaultInit();
            }
            else 
            { 
                _dto = dto;
            }
            Counter = counter;
        }

        public DataViewModel(TDto dto) : this(dto, 0) { }
        
        public DataViewModel()
        {
            DefaultInit();
        }

        public virtual TDto GetDto()
        {
            return _dto;
        }

        protected abstract void DefaultInit();
    }

    public interface IViewModelFactory<TDto> where TDto : IDto 
    {
        public DataViewModel<TDto> GetViewModel(TDto dto, int number);
        public DataViewModel<TDto> GetViewModel(TDto dto);
        public DataViewModel<TDto> GetViewModel();
    }
}

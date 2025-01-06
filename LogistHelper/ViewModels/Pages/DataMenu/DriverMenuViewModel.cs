﻿using DTOs;
using LogistHelper.ViewModels.Base;

namespace LogistHelper.ViewModels.Pages
{
    public class DriverMenuViewModel : MenuPageViewModel<DriverDto>
    {
        public DriverMenuViewModel(ListViewModel<DriverDto> list, EditViewModel<DriverDto> edit) : base(list, edit)
        {
        }
    }
}

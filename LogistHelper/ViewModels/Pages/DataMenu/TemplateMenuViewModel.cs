﻿using DTOs.Dtos;
using LogistHelper.Services;
using LogistHelper.ViewModels.Base;

namespace LogistHelper.ViewModels.Pages
{
    internal class TemplateMenuViewModel : MainMenuPageViewModel<TemplateDto>
    {
        public TemplateMenuViewModel(IMainListView<TemplateDto> list, IMainEditView<TemplateDto> edit) : base(list, edit)
        {
        }

        protected override void BackCommandExecutor()
        {
            NavigationService.Navigate(Models.PageType.DatabaseMenu);
        }
    }
}

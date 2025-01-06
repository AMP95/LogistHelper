namespace LogistHelper.ViewModels.Base
{
    public class EditViewModel<T> : BlockedViewModel where T : class
    {
        public MenuPageViewModel<T> Parent { get; set; }

        public async Task Load(Guid id) 
        { 
            
        }
    }
}

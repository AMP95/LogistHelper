using DTOs.Dtos;

namespace LogistHelper.ViewModels.Base
{
    public interface ISubMenuPage<T> where T : IDto 
    {
        Task SwitchToSubList(Guid mainId);
        Task SwitchToSubEdit(Guid id);
        Task SwitchToMainList();
    }
}

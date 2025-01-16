using DTOs.Dtos;

namespace LogistHelper.ViewModels.Base
{
    public interface ISubMenuPage<T> where T : IDto
    {
        Task SwitchToList(Guid mainId);
        Task SwitchToEdit(Guid id, Guid mainId);
    }

    public interface ISubListView<T> where T : IDto
    {
        ISubMenuPage<T> SubParent { get; set; }
        Guid MainId { get; set; }
        Task Load(Guid mainId);
    }

    public interface ISubEditView<T> where T : IDto
    {
        ISubMenuPage<T> SubParent { get; set; }
        Guid MainId { get; set; }
        Task Load(Guid subId, Guid mainId);
    }
}

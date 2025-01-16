namespace LogistHelper.ViewModels.Base
{
    public interface ISubParent 
    {
        Task SwitchToSub(Guid mainid);
        Task SwitchToMain();
    }
}

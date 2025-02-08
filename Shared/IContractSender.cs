namespace Utilities
{
    public interface IContractSender
    {
        Task SendContract(string to, string subject, string contactPath);
    }
}

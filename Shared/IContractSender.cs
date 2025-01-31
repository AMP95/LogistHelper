namespace Utilities
{
    public interface IContractSender
    {
        Task<bool> SendContract(string to, string subject, string contactPath);
    }
}

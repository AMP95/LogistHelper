namespace Utilities
{
    public interface IPrintService
    {
        Task Print(string printer, string filePath);
    }
}

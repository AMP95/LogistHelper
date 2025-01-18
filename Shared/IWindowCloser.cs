namespace Shared
{
    public interface IWindowCloser
    {
        Action<bool?> Close { get; set; }
    }
}

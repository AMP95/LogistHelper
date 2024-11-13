namespace CustomDialog
{
    public interface IWindowCloser
    {
        Action<bool?> Close { get; set; }
    }
}

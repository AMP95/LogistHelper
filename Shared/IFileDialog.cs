namespace Utilities
{
    public interface IFileDialog
    {
        bool ShowSaveDialog(out string path);
        bool ShowOpenDialog(out string[] paths);
        bool ShowFolderDialog(out string folder);
    }
}

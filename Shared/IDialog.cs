namespace Shared
{
    public interface IDialog
    {
        public void ShowError(string message, string title = "Ошибка");
        public void ShowWarning(string message, string title = "Внимание");
        public void ShowInfo(string message, string title = "Информация");
        public bool ShowQuestion(string message, string title = "Вопрос");
        public bool ShowSure(string title);
        public void ShowSuccess(string title);
    }
}

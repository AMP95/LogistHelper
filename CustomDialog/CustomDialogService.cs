using Shared;
using System.Windows;

namespace CustomDialog
{
    public class CustomDialogService : IDialog
    {
        private void ShowStatementWindow(string message, string title, DialogType type)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                StatementDialog dialogViewModel = new StatementDialog(message, title, type);
                Dialog window = new Dialog();
                window.DataContext = dialogViewModel;
                window.ShowDialog();
            }));
        }

        private bool ShowQuestionWindow(string message, string title)
        {
            return Application.Current.Dispatcher.Invoke(new Func<bool>(() =>
            {
                QuestionDialog dialogViewModel = new QuestionDialog(message, title);
                Dialog window = new Dialog();
                window.DataContext = dialogViewModel;
                bool? result = window.ShowDialog();
                return result.HasValue ? result.Value : false;
            }));
        }

        public void ShowError(string message, string title = "Ошибка")
        {
            ShowStatementWindow(message, title, DialogType.Error);
        }

        public void ShowInformation(string message, string title = "Инфо")
        {
            ShowStatementWindow(message, title, DialogType.Info);
        }

        public bool ShowQuestion(string message, string title = "Вопрос")
        {
            return ShowQuestionWindow(message, title);
        }

        public void ShowSuccess(string title)
        {
            ShowStatementWindow("Успех!", title, DialogType.Success);
        }

        public bool ShowSure(string title)
        {
            return ShowQuestionWindow("Вы уверены?", title);
        }

        public void ShowWarning(string message, string title = "Внимание")
        {
            ShowStatementWindow(message, title, DialogType.Warning);
        }

        public void ShowInfo(string message, string title = "Информация")
        {
            throw new NotImplementedException();
        }
    }
}

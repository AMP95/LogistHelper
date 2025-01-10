using System.Windows;
using System.Windows.Data;

namespace LogistHelper.UI.CustomControls
{
    public static class BindingEditor
    {
        public static void AddBinding(object viewModel, string propertyName, DependencyObject target, DependencyProperty dependencyProperty, BindingMode mode)
        {
            Binding myBinding = new Binding(propertyName)
            {
                Source = viewModel,
                Mode = mode,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            };

            BindingOperations.SetBinding(target, dependencyProperty, myBinding);
        }

        public static void RemoveBinndings(DependencyObject target)
        {
            BindingOperations.ClearAllBindings(target);
        }
    }
}

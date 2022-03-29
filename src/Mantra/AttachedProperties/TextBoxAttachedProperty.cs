using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal class IsOnlyNumber : BaseAttachedProperty<IsOnlyNumber, bool>
{
    protected override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            textBox.SetValue(InputMethod.IsInputMethodEnabledProperty, !(bool) e.NewValue);
            textBox.PreviewTextInput -= TxtInput;
            if ((bool) e.NewValue)
            {
                textBox.PreviewTextInput += TxtInput;
            }
        }
    }

    private static void TxtInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
    }
}

internal class MaxNumber : BaseAttachedProperty<MaxNumber, int>
{
    private int _maxNumber;

    protected override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            textBox.PreviewTextInput -= TxtInput;
            if ((int) e.NewValue != 0)
            {
                textBox.PreviewTextInput += TxtInput;
                _maxNumber = (int) e.NewValue;
            }
        }
    }

    private void TxtInput(object sender, TextCompositionEventArgs e)
    {
        if (int.TryParse(((TextBox)e.OriginalSource).Text + e.Text, out var number))
        {
            e.Handled = number > _maxNumber;
        }
    }
}
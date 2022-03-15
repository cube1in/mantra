// using System.Windows;
// using System.Windows.Controls;
// using System.Windows.Markup;
// using System.Windows.Media;
//
// // ReSharper disable once CheckNamespace
// namespace Mantra;
//
// internal class Resizable : BaseAttachedProperty<Resizable, bool>
// {
//     protected override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
//     {
//         if (sender is not UIElement uiElement) return;
//
//         if ((bool) e.NewValue)
//         {
//             var parent = VisualTreeHelper.GetParent(sender);
//             
//             // Content Control
//             if (parent is ContentControl contentControl)
//             {
//                 var container = new DesignerContainer
//                 {
//                     DataContext = contentControl.DataContext,
//                     Content = uiElement
//                 };
//
//                 contentControl.Content = container;
//             }
//             
//             // Grid StackPanel ...
//             if (parent is Panel panel)
//             {
//                 var container = new DesignerContainer
//                 {
//                     DataContext = panel.DataContext
//                 };
//                 
//                 ((IAddChild)container).AddChild(uiElement);
//
//                 panel.Children.Remove(uiElement);
//                 panel.Children.Add(container);
//             }
//         }
//         else
//         {
//             var parent = VisualTreeHelper.GetParent(sender);
//             if (parent is DesignerContainer {Parent: ContentControl contentControl} container)
//             {
//                 contentControl.Content = container.Content;
//             }
//         }
//     }
// }
//
// internal class StrokeColor : BaseAttachedProperty<StrokeColor, string>
// {
//     protected override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
//     {
//         // Validate color value
//         var color = (string) e.NewValue;
//         ColorConverter.ConvertFromString(color);
//
//         // Has Resizable Property
//         if (sender is UIElement uiElement && uiElement.GetValue(Resizable.ValueProperty) != null)
//         {
//             var parent = VisualTreeHelper.GetParent(uiElement);
//             if (parent is DesignerContainer container)
//             {
//                 container.StrokeColor = color;
//             }
//         }
//     }
// }
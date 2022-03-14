﻿using System;
using System.Globalization;
using System.Windows;

namespace Mantra;

internal class GroupVisibilityConverter : BaseValueConverter<GroupVisibilityConverter>
{
    public override object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is 0 ? Visibility.Hidden : Visibility.Visible;
    }

    public override object ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
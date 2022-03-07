using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Windows;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal static class DevicePixels
{
    /// <summary>
    /// Get screen scaling factor 
    /// </summary>
    /// <returns></returns>
    public static double GetScreenScalingFactor()
    {
        var desktop = GetDC(IntPtr.Zero);
        var physicalScreenHeight = GetDeviceCaps(desktop, (int) DeviceCap.DESKTOPVERTRES);

        return physicalScreenHeight / SystemParameters.PrimaryScreenHeight;
    }

    #region Private Methods

    [DllImport("gdi32.dll")]
    private static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

    [DllImport("user32.dll")]
    private static extern IntPtr GetDC(IntPtr ptr);

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    private enum DeviceCap
    {
        VERTRES = 10,
        PHYSICALWIDTH = 110,
        SCALINGFACTORX = 114,
        DESKTOPVERTRES = 117,
    }

    #endregion
}
using System;
using System.Drawing;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal static class Colors
{
    /// <summary>
    /// 获取暗色
    /// </summary>
    /// <param name="borderLine">默认低于180</param>
    /// <returns></returns>
    public static Color MakeDarkColor(int borderLine = 180)
    {
        return MakeColorByDefine(0, 180);
    }

    /// <summary>
    /// 获取暗色的十六进制值
    /// </summary>
    /// <returns></returns>
    public static string MakeDarkColorAsString()
    {
        return ColorTranslator.ToHtml(MakeDarkColor());
    }

    /// <summary>
    /// 获取亮色
    /// </summary>
    /// <param name="borderLine">默认高于180</param>
    /// <returns></returns>
    public static Color MakeLightColor(int borderLine = 180)
    {
        return MakeColorByDefine(180, 255);
    }

    /// <summary>
    /// 获取亮色的十六进制值
    /// </summary>
    /// <returns></returns>
    public static string MakeLightColorAsString()
    {
        return ColorTranslator.ToHtml(MakeLightColor());
    }

    /// <summary>
    /// 获取所有颜色
    /// </summary>
    /// <returns></returns>
    public static Color MakeAllColor()
    {
        return MakeColorByDefine(0, 255);
    }

    /// <summary>
    /// 获取所有颜色的十六进制值
    /// </summary>
    /// <returns></returns>
    public static string MakeAllColorAsString()
    {
        return ColorTranslator.ToHtml(MakeColorByDefine(0, 255));
    }

    /// <summary>
    /// 根据范围获取颜色
    /// </summary>
    /// <param name="start">起始数值 0-255</param>
    /// <param name="end">结束数值 0-255</param>
    /// <returns></returns>
    private static Color MakeColorByDefine(int start, int end)
    {
        if (start > 255 || end > 255 || start < 0 || end < 0 || start > end)
        {
            throw new InvalidOperationException();
        }

        var ran = new Random(Guid.NewGuid().GetHashCode());

        int r, g, b;
        bool result;

        do
        {
            r = ran.Next(0, 255);
            g = ran.Next(0, 255);
            b = ran.Next(0, 255);

            var y = 0.299 * r + 0.587 * g + 0.114 * b;

            result = y >= start && y <= end;
        } while (!result);

        return Color.FromArgb(r, g, b);
    }

    /// <summary>
    /// 根据范围获取颜色的十六进制值
    /// </summary>
    /// <param name="start">起始数值 0-255</param>
    /// <param name="end">结束数值 0-255</param>
    /// <returns></returns>
    public static string MakeColorByDefineAsString(int start, int end)
    {
        return ColorTranslator.ToHtml(MakeColorByDefine(start, end));
    }
}
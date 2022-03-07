using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal class PictureTransResponse
{
    /// <summary>
    /// 返回错误码
    /// 0 成功 详情查看错误码列表
    /// </summary>
    public string ErrorCode { get; set; } = null!;

    /// <summary>
    /// 错误信息
    /// success
    /// </summary>
    public string ErrorMsg { get; set; } = null!;

    /// <summary>
    /// 返回数据
    /// </summary>
    public TransContext Data { get; set; } = null!;
}

internal class TransContext
{
    /// <summary>
    /// 源语种方向
    /// </summary>
    public string From { get; set; } = null!;

    /// <summary>
    /// 译文语种方向
    /// </summary>
    public string To { get; set; } = null!;

    /// <summary>
    /// 内容
    /// </summary>
    public IEnumerable<TransContent> Content { get; set; } = null!;

    /// <summary>
    /// 组合识别原文
    /// </summary>
    public string SumSrc { get; set; } = null!;

    /// <summary>
    /// 组合翻译译文
    /// </summary>
    public string SumDst { get; set; } = null!;

    /// <summary>
    /// 组合贴合图片
    /// paste=2有效，base64格式
    /// </summary>
    public string PasteImg { get; set; } = null!;
}

internal class TransContent
{
    /// <summary>
    /// 识别原文
    /// </summary>
    public string Src { get; set; } = null!;

    /// <summary>
    /// 翻译译文
    /// </summary>
    public string Dst { get; set; } = null!;

    /// <summary>
    /// 原文擦除矩形位置
    /// 格式："rect":"0 0 321 199"矩形的位置信息，依次顺序left, top, wide, high (以图片左上角顶点为坐标原点)
    /// </summary>
    public string Rect { get; set; } = null!;

    /// <summary>
    /// 合并行数
    /// 表示该分段信息是原文的多少行合并在一起
    /// </summary>
    public int LineCount { get; set; }

    /// <summary>
    /// 分段贴合图片
    /// paste=2有效，base64格式
    /// </summary>
    public string PasteImg { get; set; } = null!;

    /// <summary>
    /// 译文贴合矩形位置
    /// （坐标0点为左上角），坐标顺序左上，右上，右下，左下
    /// </summary>
    public IEnumerable<Axis> Points { get; set; } = null!;
}

internal class Axis
{
    /// <summary>
    /// 横坐标
    /// </summary>
    public int X { get; set; }

    /// <summary>
    /// 纵坐标
    /// </summary>
    public int Y { get; set; }
}
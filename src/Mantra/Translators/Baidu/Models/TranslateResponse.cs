using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Mantra.Translators.Baidu;

internal class TranslateResponse
{
    public string LogId { get; set; } = null!;

    public TranslateResult Result { get; set; } = null!;
}

internal class TranslateResult
{
    public IEnumerable<TranslateContext> TransResult { get; set; } = null!;

    public string From { get; set; } = null!;

    public string To { get; set; } = null!;
}

internal class TranslateContext
{
    public string Dst { get; set; } = null!;

    public string Src { get; set; } = null!;
}
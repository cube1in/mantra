using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

// ReSharper disable once CheckNamespace
namespace Mantra;

// ReSharper disable once InconsistentNaming
internal class OCRResponse
{
    public long LogId { get; set; }

    public int WordsResultNum { get; set; }

    public IEnumerable<WordContext> WordsResult { get; set; } = null!;
}

internal class WordContext
{
    public string Words { get; set; } = null!;

    public WordLocation Location { get; set; } = null!;
}


[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
internal class WordLocation
{
    public int Top { get; set; }

    public int Left { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }
}

using System.Collections.Generic;

namespace Mantra;

internal class OCRResponse
{
    public int LogId { get; set; }

    public int WordsResultNum { get; set; }

    public IEnumerable<WordContext> WordsResult { get; set; } = null!;
}

using System.Collections.Generic;

namespace Mantra;

internal class WordContext
{
    public IEnumerable<string> Words { get; set; } = null!;

    public WordLocation Location { get; set; } = null!;
}

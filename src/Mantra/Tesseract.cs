using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Threading.Tasks;
using Tesseract;

namespace Mantra;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "IdentifierTypo")]
internal static class Tesseact
{
    public static async Task<IEnumerable<Rectangle>> DoOCRAsync(byte[] bytes, string language,PageIteratorLevel pageIteratorLevel = PageIteratorLevel.Para)
    {
        return await Task.Run(() =>
        {
            // ReSharper disable once StringLiteralTypo
            using var engine = new TesseractEngine(@".\trained_data", language, EngineMode.Default);
            using var pix = Pix.LoadFromMemory(bytes);
            using var page = engine.Process(pix);

            return page.GetSegmentedRegions(pageIteratorLevel);
        });
    }
}
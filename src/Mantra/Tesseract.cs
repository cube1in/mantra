using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Threading.Tasks;
using Tesseract;

namespace Mantra;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "IdentifierTypo")]
internal class Tesseact
{
    public static async Task<IEnumerable<Rectangle>> DoOCRAsync(byte[] bytes, string language,
        PageIteratorLevel pageIteratorLevel = PageIteratorLevel.Block)
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

    // public static Blocks DoOCR1(byte[] bytes, Language language = Language.English)
    // {
    //     try
    //     {
    //         // ReSharper disable once StringLiteralTypo
    //         using var engine = new TesseractOCR.Engine(@".\trained_data", language, TesseractOCR.Enums.EngineMode.Default);
    //         using var pix = TesseractOCR.Pix.Image.LoadFromMemory(bytes);
    //         using var page = engine.Process(pix);
    //
    //         return page.Layout;
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine(e);
    //         throw;
    //     }
    // }
}
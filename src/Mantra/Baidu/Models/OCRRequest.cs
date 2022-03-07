// ReSharper disable once CheckNamespace
namespace Mantra;

// ReSharper disable once InconsistentNaming
internal class OCRRequest
{
    public string Image { get; set; } = null!;

    public LanguageType LanguageType { get; set; } = LanguageType.ChnEng;
}

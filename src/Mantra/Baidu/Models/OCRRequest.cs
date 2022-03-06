namespace Mantra;

internal class OCRRequest
{
    public string Image { get; set; } = null!;

    public LanguageType LanguageType { get; set; } = LanguageType.ChnEng;
}

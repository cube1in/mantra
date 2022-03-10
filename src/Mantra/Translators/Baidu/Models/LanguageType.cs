using System.Runtime.Serialization;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal enum LanguageType
{
    None,

    [EnumMember(Value = "auto_detect")]
    AutoDetect,

    [EnumMember(Value = "CHN_ENG")]
    ChnEng,

    [EnumMember(Value = "ENG")]
    Eng,

    [EnumMember(Value = "JAP")]
    Jap
}

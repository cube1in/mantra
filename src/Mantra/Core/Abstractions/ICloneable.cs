namespace Mantra.Core.Abstractions;

internal interface ICloneable<out T>
{
    T Clone();
}
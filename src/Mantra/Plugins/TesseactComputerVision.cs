using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Mantra.Core.Abstractions;
using Mantra.Core.Models;
using TesseractOCR;
using TesseractOCR.Enums;
using TesseractOCR.Layout;

namespace Mantra.Plugins;

[SuppressMessage("ReSharper", "IdentifierTypo")]
internal class TesseactComputerVision : IComputerVision
{
    private static readonly HttpClient Client = new();

    public async Task<IEnumerable<BoundingBox>> ReadFileLocalAsync(string localFile, string language)
    {
        var blocks = await Task.Run(() => Tesseact.GetBlocks(localFile));

        return from block in blocks
            where block.BoundingBox != null
            let box = block.BoundingBox!.Value
            select new BoundingBox
            {
                Left = box.X1,
                Top = box.Y1,
                Height = box.Height,
                Width = box.Width,
                Text = block.Text
            };
    }

    public async Task<IEnumerable<BoundingBox>> ReadFileUrlAsync(string urlFile, string language)
    {
        var bytes = await Client.GetByteArrayAsync(urlFile);
        var blocks = await Task.Run(() => Tesseact.GetBlocks(bytes));

        return from block in blocks
            where block.BoundingBox != null
            let box = block.BoundingBox!.Value
            select new BoundingBox
            {
                Left = box.X1,
                Top = box.Y1,
                Height = box.Height,
                Width = box.Width,
                Text = block.Text
            };
    }

    public async Task<IEnumerable<BoundingBox>> ReadFileStreamAsync(byte[] bytes, string language)
    {
        var blocks = await Task.Run(() => Tesseact.GetBlocks(bytes));

        return from block in blocks
            where block.BoundingBox != null
            let box = block.BoundingBox!.Value
            select new BoundingBox
            {
                Left = box.X1,
                Top = box.Y1,
                Height = box.Height,
                Width = box.Width,
                Text = block.Text
            };
    }
}

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "IdentifierTypo")]
internal static class Tesseact
{
    private static Page GetPage(string path, Language language = Language.English)
    {
        var engine = new Engine(@".\trained_data", language, EngineMode.LstmOnly);
        var pix = TesseractOCR.Pix.Image.LoadFromFile(path);
        return engine.Process(pix);
    }

    private static Page GetPage(byte[] bytes, Language language = Language.English)
    {
        var engine = new Engine(@".\trained_data", language, EngineMode.LstmOnly);
        var pix = TesseractOCR.Pix.Image.LoadFromMemory(bytes);
        return engine.Process(pix);
    }

    public static string GetText(byte[] bytes, Language language = Language.English)
        => GetPage(bytes, language).Text;

    public static IEnumerable<Block> GetBlocks(string path, Language language = Language.English)
        => GetPage(path, language).Layout;

    public static IEnumerable<Block> GetBlocks(byte[] bytes, Language language = Language.English)
        => GetPage(bytes, language).Layout;

    public static IEnumerable<Paragraph> GetParagraphs(string path, Language language = Language.English)
    {
        var blocks = GetBlocks(path, language);
        return from block in blocks from paragraph in block.Paragraphs select paragraph;
    }

    public static IEnumerable<Paragraph> GetParagraphs(byte[] bytes, Language language = Language.English)
    {
        var blocks = GetBlocks(bytes, language);
        return from block in blocks from paragraph in block.Paragraphs select paragraph;
    }

    public static IEnumerable<TextLine> GetTextLines(string path, Language language = Language.English)
    {
        var paragraphs = GetParagraphs(path, language);
        return from paragraph in paragraphs from textLine in paragraph.TextLines select textLine;
    }

    public static IEnumerable<TextLine> GetTextLines(byte[] bytes, Language language = Language.English)
    {
        var paragraphs = GetParagraphs(bytes, language);
        return from paragraph in paragraphs from textLine in paragraph.TextLines select textLine;
    }

    public static IEnumerable<Word> GetWords(string path, Language language = Language.English)
    {
        var textLines = GetTextLines(path, language);
        return from textLine in textLines from word in textLine.Words select word;
    }

    public static IEnumerable<Word> GetWords(byte[] bytes, Language language = Language.English)
    {
        var textLines = GetTextLines(bytes, language);
        return from textLine in textLines from word in textLine.Words select word;
    }

    public static IEnumerable<Symbol> GetSymbols(string path, Language language = Language.English)
    {
        var words = GetWords(path, language);
        return from word in words from symbol in word.Symbols select symbol;
    }

    public static IEnumerable<Symbol> GetSymbols(byte[] bytes, Language language = Language.English)
    {
        var words = GetWords(bytes, language);
        return from word in words from symbol in word.Symbols select symbol;
    }
}
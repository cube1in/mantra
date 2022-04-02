using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Mantra.Core.Abstractions;
using Mantra.Core.Models;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Newtonsoft.Json.Linq;

namespace Mantra.Plugins;

internal class AzureComputerVision : IComputerVision
{
    private static readonly ComputerVisionClient Client;

    static AzureComputerVision()
    {
        const string path = @"C:\Users\sou1m\Documents\azure.json";
        if (!File.Exists(path)) throw new FileNotFoundException();
        var jo = JObject.Parse(File.ReadAllText(path));
        var subscriptionKey = jo["ComputerVision"]!["SubscriptionKey"]!.Value<string>()!;
        var endpoint = jo["ComputerVision"]!["Endpoint"]!.Value<string>()!;

        Client = Authenticate(endpoint, subscriptionKey);
    }

    private static ComputerVisionClient Authenticate(string endpoint, string key)
    {
        var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(key)) {Endpoint = endpoint};
        return client;
    }

    public async Task<IEnumerable<BoundingBox>> ReadFileStreamAsync(byte[] bytes, string language)
    {
        // Read text from URL
        var textHeaders = await Client.ReadInStreamAsync(new MemoryStream(bytes));
        // After the request, get the operation location (operation ID)
        var operationLocation = textHeaders.OperationLocation;

        // Retrieve the URI where the recognized text will be stored from the Operation-Location header.
        // We only need the ID and not the full URL
        const int numberOfCharsInOperationId = 36;
        var operationId = operationLocation[^numberOfCharsInOperationId..];

        // Extract the text
        ReadOperationResult results;
        do
        {
            results = await Client.GetReadResultAsync(Guid.Parse(operationId));
        } while (results.Status is OperationStatusCodes.Running or OperationStatusCodes.NotStarted);

        var textUrlFileResults = results.AnalyzeResult.ReadResults;

        return from result in textUrlFileResults
            from line in result.Lines
            select new BoundingBox
            {
                Left = line.BoundingBox[0]!.Value,
                Top = line.BoundingBox[1]!.Value,
                Width = line.BoundingBox[2]!.Value,
                Height = line.BoundingBox[3]!.Value,
                Text = line.Text
            };
    }

    public async Task<IEnumerable<BoundingBox>> ReadFileUrlAsync(string urlFile, string language)
    {
        // Read text from URL
        var textHeaders = await Client.ReadAsync(urlFile);
        // After the request, get the operation location (operation ID)
        var operationLocation = textHeaders.OperationLocation;

        // Retrieve the URI where the extracted text will be stored from the Operation-Location header.
        // We only need the ID and not the full URL
        const int numberOfCharsInOperationId = 36;
        var operationId = operationLocation[^numberOfCharsInOperationId..];

        // Extract the text
        ReadOperationResult results;
        do
        {
            results = await Client.GetReadResultAsync(Guid.Parse(operationId));
        } while (results.Status is OperationStatusCodes.Running or OperationStatusCodes.NotStarted);

        var textUrlFileResults = results.AnalyzeResult.ReadResults;

        return from result in textUrlFileResults
            from line in result.Lines
            select new BoundingBox
            {
                Left = line.BoundingBox[0]!.Value,
                Top = line.BoundingBox[1]!.Value,
                Width = line.BoundingBox[2]!.Value,
                Height = line.BoundingBox[3]!.Value,
                Text = line.Text
            };
    }

    public async Task<IEnumerable<BoundingBox>> ReadFileLocalAsync(string localFile, string language)
    {
        // Read text from URL
        var textHeaders = await Client.ReadInStreamAsync(File.OpenRead(localFile));
        // After the request, get the operation location (operation ID)
        var operationLocation = textHeaders.OperationLocation;

        // Retrieve the URI where the recognized text will be stored from the Operation-Location header.
        // We only need the ID and not the full URL
        const int numberOfCharsInOperationId = 36;
        var operationId = operationLocation[^numberOfCharsInOperationId..];

        // Extract the text
        ReadOperationResult results;
        do
        {
            results = await Client.GetReadResultAsync(Guid.Parse(operationId));
        } while (results.Status is OperationStatusCodes.Running or OperationStatusCodes.NotStarted);

        var textUrlFileResults = results.AnalyzeResult.ReadResults;

        return from result in textUrlFileResults
            from line in result.Lines
            select new BoundingBox
            {
                Left = line.BoundingBox[0]!.Value,
                Top = line.BoundingBox[1]!.Value,
                Width = line.BoundingBox[2]!.Value,
                Height = line.BoundingBox[3]!.Value,
                Text = line.Text
            };
    }
}
using Microsoft.KernelMemory;
using Microsoft.SemanticKernel;

public static class MemoryUsage
{
    public static async Task Run(string key, string endpoint, Kernel kernel)
    {
        var memory = new KernelMemoryBuilder()
        .WithAzureOpenAITextEmbeddingGeneration(new AzureOpenAIConfig
        {
            APIType = AzureOpenAIConfig.APITypes.EmbeddingGeneration,
            Auth = AzureOpenAIConfig.AuthTypes.APIKey,
            APIKey = key,
            MaxTokenTotal = 8191,
            MaxEmbeddingBatchSize = 1,
            Endpoint = endpoint,
            Deployment = "embedding",
        })
        .WithAzureOpenAITextGeneration(new AzureOpenAIConfig
        {
            APIType = AzureOpenAIConfig.APITypes.ChatCompletion,
            Auth = AzureOpenAIConfig.AuthTypes.APIKey,
            APIKey = key,
            Endpoint = endpoint,
            Deployment = "4omni",
            MaxTokenTotal = 12048,
        })
        .WithAzureAISearchMemoryDb(new AzureAISearchConfig
        {
            Auth = AzureAISearchConfig.AuthTypes.APIKey,
            APIKey = "oai-key",
            Endpoint = "https://some-fancy-demo.search.windows.net",
        })
        .WithSearchClientConfig(new SearchClientConfig { MaxMatchesCount = 2, Temperature = 0.7 })
        .Build();
        //.Build<MemoryServerless>();  

        if (!await memory.IsDocumentReadyAsync("doc001"))
        {
            await CreateIndexAndImportFile(memory);
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Ask a question about this document: ");
        Console.ForegroundColor = ConsoleColor.White;

        var question = Console.ReadLine();
        var answer = await memory.AskAsync(question);
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Question: {question}\n\nAnswer: {answer.Result}");
        Console.ForegroundColor = ConsoleColor.White;

        Console.WriteLine("Ask a question about this document with prompt: ");
        question = Console.ReadLine();
        var promptAnswer = await kernel.InvokePromptAsync(question);
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\nResult: \n\n{promptAnswer}");
        Console.ForegroundColor = ConsoleColor.White;
    }

    private static async Task CreateIndexAndImportFile(IKernelMemory memory)
    {
        await memory.DeleteIndexAsync("doc001");
        await memory.ImportDocumentAsync("usergroupinfor.pdf", documentId: "doc001");
        while (!await memory.IsDocumentReadyAsync("doc001"))
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1500));
        }

        Console.WriteLine("Waiting for memory ingestion to complete...");
    }
}

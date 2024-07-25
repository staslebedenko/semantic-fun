using Azure.AI.OpenAI;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System;
using System.Threading.Tasks;

public static class ChatWithIndex
{
    public static async Task Run(Kernel kernel)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Question for doc + LLM > ");
        Console.ForegroundColor = ConsoleColor.White;

        var request = Console.ReadLine();
        var azureSearchExtensionConfiguration = new AzureSearchChatExtensionConfiguration
        {
            SearchEndpoint = new Uri("https://some-fancy-demo.search.windows.net"),
            Authentication = new OnYourDataApiKeyAuthenticationOptions("oai-key"),
            IndexName = "msugodua"
        };
        var chatExtensionsOptions = new AzureChatExtensionsOptions { Extensions = { azureSearchExtensionConfiguration } };

#pragma warning disable SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.  
        var executionSettings = new OpenAIPromptExecutionSettings { AzureChatExtensionsOptions = chatExtensionsOptions };
        var result = await kernel.InvokePromptAsync(request, new(executionSettings));
#pragma warning restore SKEXP0010

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\nResult: \n\n{result}");
        Console.ForegroundColor = ConsoleColor.White;
    }
}

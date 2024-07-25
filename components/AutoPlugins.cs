using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System;
using System.Threading.Tasks;

public static class AutoPlugins
{
    public static async Task Run(Kernel kernel, OpenAIPromptExecutionSettings? settings)
    {
        ChatHistory history = new ChatHistory();
        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("your question > ");
        Console.ForegroundColor = ConsoleColor.White;

        var request = Console.ReadLine();
        history.AddUserMessage(request!);
        var result = chatCompletionService.GetStreamingChatMessageContentsAsync(history, settings, kernel);
        string fullMessage = "";

        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("LLM response > ");

        await foreach (var content in result)
        {
            Console.Write(content.Content);
            fullMessage += content.Content;
        }
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.White;

        history.AddAssistantMessage(fullMessage);
    }
}

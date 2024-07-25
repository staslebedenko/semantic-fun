using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

public static class ImageExplanation
{
    public static async Task Run(Kernel kernel)
    {
        var chat = kernel.GetRequiredService<IChatCompletionService>();
        var history = new ChatHistory();
        
        history.AddSystemMessage("You are a not friendly but helpful assistant that responds to questions directly");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Question: Can you tell me what's in this image?");
        Console.ForegroundColor = ConsoleColor.White;

        var message = new ChatMessageContentItemCollection
        {
            new TextContent("Can you tell me what's in this image?"),
            new ImageContent(new Uri("https://pbs.twimg.com/media/FJGF4luXMAkpFaS?format=jpg&name=large"))  
            //new ImageContent(new Uri("https://i.ibb.co/DCWpt9m/Screenshot-2024-07-24-163934.png"))  
        };

        history.AddUserMessage(message);
        var result = await chat.GetChatMessageContentAsync(history);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Let me describe that image for you: {result}");
        Console.ForegroundColor = ConsoleColor.White;
    }
}

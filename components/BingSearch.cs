using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Plugins.Web;
using Microsoft.SemanticKernel.Plugins.Web.Bing;
using System;
using System.Threading.Tasks;

public static class BingSearch
{
    public static async Task Run(Kernel kernel, OpenAIPromptExecutionSettings? settings)
    {

        
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("your question > ");
        Console.ForegroundColor = ConsoleColor.White;

        var request = Console.ReadLine();
        Console.WriteLine("Searching...");
        await DirectSearchOutput(kernel, request);
        var result = await kernel.InvokePromptAsync(request, new KernelArguments(settings));

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\nResult: \n\n{result}");
        Console.ForegroundColor = ConsoleColor.White;
    }

    private static async Task DirectSearchOutput(Kernel kernel, string? request)
    {
        var function = kernel.Plugins["bing"]["search"];
        var response = await kernel.InvokeAsync(function, new()
        {
            ["query"] = request
        });

        Console.WriteLine($"\nResult: \n\n{response}");
    }
}

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Plugins.Web.Bing;
using Microsoft.SemanticKernel.Plugins.Web;
using SemanticKernel.NativeFunction;

class Program
{
    static async Task Main(string[] args)
    {
        var endpoint = "https://super-fancy.openai.azure.com/";
        var key = "oai-key";
        var builder = Kernel.CreateBuilder();
        builder.AddAzureOpenAIChatCompletion("4omni", endpoint, key);
        builder.Plugins.AddFromType<DateTimePlugin>();
        var kernel = builder.Build();

        var bingConnector = new BingConnector("bing-key");
        kernel.ImportPluginFromObject(new WebSearchEnginePlugin(bingConnector), "bing");

        var settings = new OpenAIPromptExecutionSettings
        {
            ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
            Temperature = 0.6f,
            MaxTokens = 700
        };

        while (true)
        {
            //await AutoPlugins.Run(kernel, settings);
            //await BingSearch.Run(kernel, settings);
            //await ImageExplanation.Run(kernel);
            //await MemoryUsage.Run(key, endpoint, kernel);
            await ChatWithIndex.Run(kernel);
        }
    }
}

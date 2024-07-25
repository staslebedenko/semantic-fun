using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace SemanticKernel.NativeFunction;

public class DateTimePlugin
{
    [KernelFunction]
    [Description("Gets the current time.")]
    public TimeSpan GetTime()
    {
        return TimeProvider.System.GetLocalNow().TimeOfDay;
    }

    [KernelFunction]
    [Description("Gets the current date.")]
    public DateTime GetDate()
    {
        return TimeProvider.System.GetLocalNow().Date;
    }
}
// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using Polly;

namespace tryouts;

public class Program
{
    // public static async Task Main(string[] args)
    // {
    //     var summary = BenchmarkRunner.Run<OneTryVsMultipleTry>();
    // }
    
    public static async Task Main(string[] args)
    {
        var urls = new[] {""};
        await AsyncLocalTest.Run();
        
        var bulkhead = Policy.BulkheadAsync(10, Int32.MaxValue);
        var tasks = new List<Task>();
        for(var i = 0; i < 10000; i++)
        {
            var t = bulkhead.ExecuteAsync(async () =>
            {
                await Task.Delay(500);
            });
            tasks.Add(t);
        }
        await Task.WhenAll(tasks);


        await using (new My())
        {
            
        }
        await using var file = File.OpenRead("").ConfigureAwait(false);
        // await using var file2 = Task.Delay(1000).ConfigureAwait(false);
    }
}

ref struct My
{
    public ValueTask DisposeAsync()
    {
        return default;
    }
}

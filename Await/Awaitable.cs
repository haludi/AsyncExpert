using System.Runtime.CompilerServices;

namespace Await;

public class SimpleAwaitable
{
    public SimpleAwaiter GetAwaiter() => new SimpleAwaiter();
}

public class SimpleAwaiter : INotifyCompletion
{
    public bool IsCompleted => false;

    public void OnCompleted(Action action)
    {
        action();
        Console.WriteLine("Finished");
    }

    public string GetResult() => "done!";
}
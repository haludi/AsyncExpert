using BenchmarkDotNet.Attributes;

namespace Benchmarks;

[MemoryDiagnoser]
// [NativeMemoryProfiler]
[ThreadingDiagnoser]
public class CancelWaitForUncancelableTask
{
    private Func<Task<string>> taskDelegate;

    public CancelWaitForUncancelableTask()
    {
        taskDelegate = Execute;
    }
    
    [Params(true, false)]
    public bool Timeout;
    
    async Task<string> Execute()
    {
        if (Timeout)
            await Task.Delay(int.MaxValue);

        return "";
    }

    // [Benchmark]
    // public async Task TaskNew()
    // {
    //     try
    //     {
    //         await CancelWithNewTask.TimeoutAfter(Task.Run(taskDelegate), Timeout ? 0 : int.MaxValue);
    //     }
    //     catch (TaskCanceledException e)
    //     {
    //     }
    // }
    //
    // [Benchmark]
    // public async Task  TaskDelayWithContinue()
    // {
    //     try
    //     {
    //         await CancelWithDelayWithContinue.TimeoutAfter(Task.Run(taskDelegate), Timeout ? 0 : int.MaxValue);
    //     }
    //     catch (TaskCanceledException e)
    //     {
    //     }
    // }
    
    [Benchmark(Baseline = true)]
    public async Task  TaskDelay()
    {
        try
        {
            await CancelWithDelay.TimeoutAfter(Task.Run(taskDelegate), Timeout ? 0 : int.MaxValue);
        }
        catch (TaskCanceledException e)
        {
        }
    }
    
    [Benchmark]
    public async Task  TaskCompilationSource()
    {
        try
        {
            await CancelWithTaskCompletionSource.TimeoutAfter(Task.Run(taskDelegate), Timeout ? 0 : int.MaxValue);
        }
        catch (TaskCanceledException e)
        {
        }
    }
}

public static class CancelWithNewTask
{
    private static T Dummy<T>() => default!;

    public static async Task<T> TimeoutAfter<T>(this Task<T> task, int millisecondsTimeout)
    {
        using var cts = new CancellationTokenSource();
        cts.CancelAfter(millisecondsTimeout);

        var firstTask = await Task.WhenAny(new Task<T>(Dummy<T>, cts.Token), task);
        cts.Cancel();
        return await firstTask;
    }
}

public static class CancelWithTaskCompletionSource
{
    private static T Dummy<T>() => default!;

    static void Callback<T>(object o) => ((TaskCompletionSource<T>) o).SetCanceled();
    
    public static async Task<T> TimeoutAfter<T>(this Task<T> task, int millisecondsTimeout)
    {
        using var cts = new CancellationTokenSource();
        var tcs = new TaskCompletionSource<T>();
        cts.CancelAfter(millisecondsTimeout);
        cts.Token.Register(Callback<T>, tcs);
        
        var firstTask = await Task.WhenAny(tcs.Task, task);
        cts.Cancel();
        return await firstTask;
    }
}

public static class CancelWithDelayWithContinue
{
    private static Task<T> Delay<T>(int millisecond, CancellationToken token)
    {
        return Task.Delay(millisecond, token).ContinueWith(CancelledTask<T>, "My massage", token);
    }
        
    private static T CancelledTask<T>(Task t, object? o) => throw new TaskCanceledException();
    
    // private static T CancelledTask<T>(Task t, object? o) => 
    //     o != null ? throw new TaskCanceledException(o.ToString()) : throw new TaskCanceledException();

    public static async Task<T> TimeoutAfter<T>(this Task<T> task, int millisecondsTimeout)
    {
        using var cts = new CancellationTokenSource();
        var firstTask = await Task.WhenAny(Delay<T>(millisecondsTimeout, cts.Token), task);
        cts.Cancel();
        return await firstTask;
    }
}
public static class CancelWithDelay
{
    public static async Task<T> TimeoutAfter<T>(this Task<T> task, int millisecondsTimeout)
    {
        using var cts = new CancellationTokenSource();
        
        var delayTask = Task.Delay(millisecondsTimeout, cts.Token);

        var resultTask = await Task.WhenAny(task, delayTask);
        if (resultTask == delayTask)
        {
            // Operation cancelled
            throw new TaskCanceledException();
        }

        // Cancel the timer task so that it does not fire
        cts.Cancel();

        return await task;
    }
}
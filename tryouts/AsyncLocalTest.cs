namespace tryouts;

public class AsyncLocalTest
{
    private static AsyncLocal<int> AsyncLocal = new AsyncLocal<int>();

    public static async Task Run()
    {
        // var tasks = Enumerable.Range(0, 2).Select(x => Task.Run( async () =>
        // {
        //     // await Task.Yield();
        //     AsyncLocal.Value = x + 1;
        //
        //     var guid = Guid.NewGuid();
        //     Console.WriteLine($"1 {AsyncLocal.Value} - {guid}");
        //     await AsyncFun(guid);
        //     Console.WriteLine($"0 AsyncLocal.Value:{AsyncLocal.Value} - guid:{guid}");
        //
        // }));
        //
        // await Task.WhenAll(tasks);

        AsyncLocal.Value = 1;
        var guid = Guid.NewGuid();
        await AsyncFun(guid, true);
        Console.WriteLine($"0 AsyncLocal.Value:{AsyncLocal.Value} - guid:{guid}");
    }

    private static async Task AsyncFun(Guid guid, bool b)
    {
        Console.WriteLine($"2 AsyncLocal.Value:{AsyncLocal.Value} - guid:{guid}");

        // await Task.Yield();
        AsyncLocal.Value = AsyncLocal.Value * 10 + 1;
        Console.WriteLine($"3 AsyncLocal.Value:{AsyncLocal.Value} - guid:{guid}");

        if(b)
            await AsyncFun(guid, false);
        // var tasks = Enumerable.Range(0, 2).Select(x => Task.Run(async () =>
        // {
        //     Console.WriteLine($"4 AsyncLocal.Value:{AsyncLocal.Value} - guid:{guid}");
        //     AsyncLocal.Value = AsyncLocal.Value * 10 + x;
        //     Console.WriteLine($"5 AsyncLocal.Value:{AsyncLocal.Value} - guid:{guid}");
        // }));
        // await Task.WhenAll(tasks);
        // await Task.Run(async () =>
        // {
        //     AsyncLocal.Value = AsyncLocal.Value * 10 + 1;
        //     Console.WriteLine($"5 AsyncLocal.Value:{AsyncLocal.Value} - guid:{guid}");
        // });

        Console.WriteLine($"6 AsyncLocal.Value:{AsyncLocal.Value} - guid:{guid}");

    }
}
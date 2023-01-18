using System.Buffers;
using BenchmarkDotNet.Attributes;

namespace tryouts;

public class OneTryVsMultipleTry
{
    private const int N = 10000;
    
    [Benchmark]
    public void OneTry()
    {
        var a = 0;
        try
        {
            for (int i = 0; i < 10000; i++)
            {
                a++;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    [Benchmark]
    public void TryCatchFinallyWithClass()
    {
        var a = 0;

        for (int i = 0; i < 10000; i++)
        {
            try
            {
                a++;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
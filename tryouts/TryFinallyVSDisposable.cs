using System.Buffers;
using BenchmarkDotNet.Attributes;

namespace tryouts;

public class TryFinallyVSDisposable
{
    private const int N = 10000;
    private readonly byte[] data;

    class ReturnRentedArray : IDisposable
    {
        private readonly int[] _array;

        public ReturnRentedArray()
        {
            _array = ArrayPool<int>.Shared.Rent(10);
        }

        public void Dispose()
        {
            ArrayPool<int>.Shared.Return(_array);
        }
    }
    struct ReturnRentedArray2 : IDisposable
    {
        private readonly int[] _array;

        public ReturnRentedArray2()
        {
            _array = ArrayPool<int>.Shared.Rent(10);
        }

        public void Dispose()
        {
            ArrayPool<int>.Shared.Return(_array);
        }
    }

    readonly struct ReturnRentedArrayReadOnly : IDisposable
    {
        private readonly int[] _array;

        public ReturnRentedArrayReadOnly()
        {
            _array = ArrayPool<int>.Shared.Rent(10);
        }

        public void Dispose()
        {
            ArrayPool<int>.Shared.Return(_array);
        }
    }
    
    [Benchmark]
    public void TryCatchFinallyWithout()
    {
        int[] array = null;
        try
        {
            array = ArrayPool<int>.Shared.Rent(10);
        }
        finally
        {
            ArrayPool<int>.Shared.Return(array);
        }
    }

    [Benchmark]
    public void UsingClass()
    {
        using var array = new ReturnRentedArray();
    }
    
    [Benchmark]
    public void TryCatchFinallyWithClass()
    {
        ReturnRentedArray array = null;
        try
        {
            array = new ReturnRentedArray();
        }
        finally
        {
            array.Dispose();
        }
    }
    
    [Benchmark]
    public void TryCatchFinallyWithStruct()
    {
        ReturnRentedArray2 array = default;
        try
        {
            array = new ReturnRentedArray2();
        }
        finally
        {
            array.Dispose();
        }
    }

    [Benchmark]
    public void UsingStruct()
    {
        using var array = new ReturnRentedArray2();
    }
    
    [Benchmark]
    public void UsingStructReadOnly()
    {
        using var array = new ReturnRentedArrayReadOnly();
    }
}
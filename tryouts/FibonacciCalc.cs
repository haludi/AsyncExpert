using BenchmarkDotNet.Attributes;

namespace tryouts
{
    [DisassemblyDiagnoser(exportCombinedDisassemblyReport: true)]
    [MemoryDiagnoser]
    public class FibonacciCalc
    {
        // HOMEWORK:
        // 1. Write implementations for RecursiveWithMemoization and Iterative solutions
        // 2. Add MemoryDiagnoser to the benchmark
        // 3. Run with release configuration and compare results
        // 4. Open disassembler report and compare machine code
        // 
        // You can use the discussion panel to compare your results with other students

        [Benchmark(Baseline = true)]
        [ArgumentsSource(nameof(Data))]
        public ulong Recursive(ulong n)
        {
            if (n == 1 || n == 2) return 1;
            return Recursive(n - 2) + Recursive(n - 1);
        }

        private readonly Dictionary<ulong, ulong> _cache = new Dictionary<ulong, ulong>
        {
            {1, 1},
            {2, 1}
        };

        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public ulong RecursiveWithMemoization(ulong n)
        {
            if (_cache.TryGetValue(n, out var result))
                return result;
            
            result = RecursiveWithMemoization(n - 2) + RecursiveWithMemoization(n - 1);
            _cache[n] = result;
            return result;
        }
        
        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public ulong Iterative(ulong n)
        {
            var index = 0;
            Span<ulong> result = stackalloc ulong[]{0, 1};
            for (ulong i = 1; i < n; i++)
            {
                result[index++ % 2] = result[0] + result[1];
            }

            return result[(int) (n % 2)];
        }

        public IEnumerable<ulong> Data()
        {
            yield return 15;
            yield return 35;
        }
    }
}
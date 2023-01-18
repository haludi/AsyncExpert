// See https://aka.ms/new-console-template for more information

using System.Reflection;
using BenchmarkDotNet.Running;
using Benchmarks;

// var testClass = new CancelWaitForUncancelableTask();
// await testClass.TaskDelay();

var r = BenchmarkSwitcher.FromAssembly(Assembly.GetExecutingAssembly()).Run(args);
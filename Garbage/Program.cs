using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using Garbage.TwoHeaps;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Garbage
{
	[SimpleJob(RuntimeMoniker.Net472, baseline: true)]
	[RPlotExporter]
	[MemoryDiagnoser]
	public class Program
	{
		public static void Main(string[] args)
		{
			//HappyNumbersDemo.Run();
			//ArrayListDemo.Run();
			//OneZeroDemo.Run();
			//new TwoHeapsDemo().Run();

			//Console.OutputEncoding = Encoding.Unicode;
			//Console.WindowWidth = Console.LargestWindowWidth;
			//Console.WindowHeight = Console.LargestWindowHeight;
			var summary = BenchmarkRunner.Run<Program>();
			Console.WriteLine(summary);
			//new Program().RunWithInMemoryBuffer();
		}

		[Benchmark]
		public void RunWithInMemoryBuffer()
		{
			var n = 15;
			for (int i = 1; i <= n; i++)
			{
				Console.ForegroundColor = (ConsoleColor)(i);
				using (var ms = new MemoryStream())
				using (var stream = new StreamWriter(ms))
				{
					for (int j = 0; j < i; j++)
					{
						for (int k = 0; k < (i << 1) - (j == 0 || j == i - 1 ? i / 3 : 0); k++)
							stream.Write('█');

						stream.Write(Environment.NewLine);
					}
					for (int j = 0; j < i; j++)
					{
						for (int k = 0; k < (i << 3) - (j == 0 || j == i - 1 ? i / 3 : 0); k++)
							stream.Write('█');

						stream.Write(Environment.NewLine);
					}
					for (int j = 0; j < i; j++)
					{
						for (int k = 0; k < (i << 1) - (j == 0 || j == i - 1 ? i / 3 : 0); k++)
							stream.Write('█');

						stream.Write(Environment.NewLine);
					}

					stream.Write(Environment.NewLine);
					stream.Flush();
					Console.WriteLine(Encoding.UTF8.GetString(ms.ToArray()));
				}
			}
		}

		[Benchmark]
		public void RunWithConsoleStream()
		{
			var n = 15;
			var typeInfo = Console.Out.GetType();
			var outFieldInfo = typeInfo.GetField("_out", BindingFlags.NonPublic | BindingFlags.Instance);
			var stream = (StreamWriter)outFieldInfo.GetValue(Console.Out);

			for (int i = 1; i <= n; i++)
			{
				Console.ForegroundColor = (ConsoleColor)(i);
				for (int j = 0; j < i; j++)
				{
					for (int k = 0; k < (i << 1) - (j == 0 || j == i - 1 ? i / 3 : 0); k++)
						stream.Write('█');

					stream.Write(Environment.NewLine);
				}
				for (int j = 0; j < i; j++)
				{
					for (int k = 0; k < (i << 3) - (j == 0 || j == i - 1 ? i / 3 : 0); k++)
						stream.Write('█');

					stream.Write(Environment.NewLine);
				}
				for (int j = 0; j < i; j++)
				{
					for (int k = 0; k < (i << 1) - (j == 0 || j == i - 1 ? i / 3 : 0); k++)
						stream.Write('█');

					stream.Write(Environment.NewLine);
				}

				stream.Write(Environment.NewLine);
			}
		}

		[Benchmark]
		public void RunWithConsole()
		{
			var n = 15;

			var w = Console.Out;

			for (int i = 1; i <= n; i++)
			{
				Console.ForegroundColor = (ConsoleColor)(i);
				for (int j = 0; j < i; j++)
				{
					for (int k = 0; k < (i << 1) - (j == 0 || j == i - 1 ? i / 3 : 0); k++)
						w.Write('█');

					w.Write(Environment.NewLine);
				}
				for (int j = 0; j < i; j++)
				{
					for (int k = 0; k < (i << 3) - (j == 0 || j == i - 1 ? i / 3 : 0); k++)
						w.Write('█');

					w.Write(Environment.NewLine);
				}
				for (int j = 0; j < i; j++)
				{
					for (int k = 0; k < (i << 1) - (j == 0 || j == i - 1 ? i / 3 : 0); k++)
						w.Write('█');

					w.Write(Environment.NewLine);
				}

				w.Write(Environment.NewLine);
			}
		}

		[Benchmark]
		public void RunWithStringBuilder()
		{
			var n = 15;
			StringBuilder sb = new StringBuilder(1 << 64);
			for (int i = 1; i <= n; i++)
			{
				sb.Clear();
				Console.ForegroundColor = (ConsoleColor)(i);
				for (int j = 0; j < i; j++)
				{
					for (int k = 0; k < (i << 1) - (j == 0 || j == i - 1 ? i / 3 : 0); k++)
						sb.Append('█');

					sb.Append(Environment.NewLine);
				}
				for (int j = 0; j < i; j++)
				{
					for (int k = 0; k < (i << 3) - (j == 0 || j == i - 1 ? i / 3 : 0); k++)
						sb.Append('█');

					sb.Append(Environment.NewLine);
				}
				for (int j = 0; j < i; j++)
				{
					for (int k = 0; k < (i << 1) - (j == 0 || j == i - 1 ? i / 3 : 0); k++)
						sb.Append('█');

					sb.Append(Environment.NewLine);
				}

				Console.WriteLine(sb);
			}
		}

		[Benchmark]
		public void RunWithString()
		{
			var n = 15;
			for (int i = 1; i <= n; i++)
			{
				Console.ForegroundColor = (ConsoleColor)(i);

				for (int j = 0; j < i; j++)
				{
					Console.WriteLine(new string('█', (i << 1) - (j == 0 || j == i - 1 ? i / 3 : 0)));
				}
				for (int j = 0; j < i; j++)
				{
					Console.WriteLine(new string('█', (i << 3) - (j == 0 || j == i - 1 ? i / 3 : 0)));
				}
				for (int j = 0; j < i; j++)
				{
					Console.WriteLine(new string('█', (i << 1) - (j == 0 || j == i - 1 ? i / 3 : 0)));
				}

				Console.WriteLine();
			}
		}
	}

}

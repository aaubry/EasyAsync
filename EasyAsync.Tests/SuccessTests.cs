// 	Copyright 2011 Antoine Aubry
// 
// 	This file is part of EasyAsync.
// 
// 	EasyAsync is free software: you can redistribute it and/or modify
// 	it under the terms of the GNU Lesser General Public License as published by
// 	the Free Software Foundation, either version 3 of the License, or
// 	(at your option) any later version.
// 
// 	Foobar is distributed in the hope that it will be useful,
// 	but WITHOUT ANY WARRANTY; without even the implied warranty of
// 	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// 	GNU General Public License for more details.
// 
// 	You should have received a copy of the GNU General Public License
// 	along with EasyAsync. If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using EasyAsync.Tests.AdslServiceReference;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EasyAsync.Tests
{
	[TestClass]
	public class SuccessTests
	{
		public TestContext TestContext { get; set; }

		[TestMethod]
		public void AsyncSumTest()
		{
			dynamic result = AsyncIterator.Run(c => AddAsync(c, 2, 3, 4));
			Assert.AreEqual(9, result.Sum, "The sum should be 9");
			Assert.AreNotEqual(Thread.CurrentThread.ManagedThreadId, result.ThreadId, "The sum should have been executed on another thread.");
		}

		#region Helpers
		private IEnumerable<IAsyncResult> AddAsync(IAsyncIteratorContext context, int a, int b, int c)
		{
			Func<int, int, int> add = Add;

			yield return add.BeginInvoke(a, b, context.Callback, null);
			var x = add.EndInvoke(context.LastAsyncResult);

			yield return add.BeginInvoke(x, c, context.Callback, null);
			var y = add.EndInvoke(context.LastAsyncResult);

			context.SetResult(new { Sum = y, ThreadId = Thread.CurrentThread.ManagedThreadId });
		}

		private int Add(int a, int b)
		{
			TestContext.WriteLine("{0}: Calculating {1} + {2}", Thread.CurrentThread.ManagedThreadId, a, b);
			Thread.Sleep(10);
			var result = a + b;
			TestContext.WriteLine("{0}: {1} + {2} = {3}", Thread.CurrentThread.ManagedThreadId, a, b, result);
			return result;
		}
		#endregion

		[TestMethod]
		public void AsyncServiceTest()
		{
			var result = (Dictionary<string, bool>)AsyncIterator.Run(c => HasCoverageAsync(c, "269906222", "210093556", "212345678"));
			Assert.AreEqual(3, result.Count, "There should have been 3 results");
		}

		#region Helpers
		private IEnumerable<IAsyncResult> HasCoverageAsync(IAsyncIteratorContext context, params string[] numbers)
		{
			var result = new Dictionary<string, bool>();
			using (var proxy = new ADSLSoapClient())
			{
				foreach (var number in numbers)
				{
					TestContext.WriteLine("{0}: Checking coverage of {1}", Thread.CurrentThread.ManagedThreadId, number);
					yield return proxy.BeginHasCoverage(number, context.Callback, null);
					bool hasCoverage = proxy.EndHasCoverage(context.LastAsyncResult);
					result.Add(number, hasCoverage);

					TestContext.WriteLine("{0}: {1} has coverage = {2}", Thread.CurrentThread.ManagedThreadId, number, hasCoverage);
				}
			}
			context.SetResult(result);
		}
		#endregion

		[TestMethod]
		public void NestedIteratorsTest()
		{
			dynamic result = AsyncIterator.Run(c => NestedAsync(c, 2, 3, 4));
			Assert.AreEqual(9, result.Sum, "The sum should be 9");
			Assert.AreNotEqual(Thread.CurrentThread.ManagedThreadId, result.ThreadId, "The sum should have been executed on another thread.");
			Assert.AreEqual(3, result.Coverage.Count, "There should have been 3 results");
		}

		#region Helpers
		private IEnumerable<IAsyncResult> NestedAsync(IAsyncIteratorContext context, int a, int b, int c)
		{
			Func<int, int, int> add = Add;

			yield return add.BeginInvoke(a, b, context.Callback, null);
			var x = add.EndInvoke(context.LastAsyncResult);

			yield return AsyncIterator.BeginRun(ctx => HasCoverageAsync(ctx, "269906222", "210093556", "212345678"), context.Callback);
			var coverage = (Dictionary<string, bool>)AsyncIterator.EndRun(context.LastAsyncResult);

			yield return add.BeginInvoke(x, c, context.Callback, null);
			var y = add.EndInvoke(context.LastAsyncResult);

			context.SetResult(new { Sum = y, ThreadId = Thread.CurrentThread.ManagedThreadId, Coverage = coverage });
		}
		#endregion
	}
}
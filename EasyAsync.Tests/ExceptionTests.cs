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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EasyAsync.Tests
{
	[TestClass]
	public class ExceptionTests
	{
		public TestContext TestContext { get; set; }

		[TestMethod]
		public void ExceptionsInAsyncCodeCanBeHandled()
		{
			var result = AsyncIterator.BeginRun(c => DoSomethingAsync(c, false));
			try
			{
				AsyncIterator.EndRun(result);
				Assert.Fail("No exception was thrown");
			}
			catch
			{
			}
		}

		[TestMethod]
		public void ExceptionsInSyncCodeCanBeHandled()
		{
			var result = AsyncIterator.BeginRun(c => DoSomethingAsync(c, true));
			try
			{
				AsyncIterator.EndRun(result);
				Assert.Fail("No exception was thrown");
			}
			catch
			{
			}
		}

		private IEnumerable<IAsyncResult> DoSomethingAsync(IAsyncIteratorContext context, bool failEarly)
		{
			Action<bool> del = DoSomething;

			if (failEarly)
			{
				throw new Exception();
			}

			yield return del.BeginInvoke(false, context.Callback, null);
			del.EndInvoke(context.LastAsyncResult);

			yield return del.BeginInvoke(true, context.Callback, null);
			try
			{
				del.EndInvoke(context.LastAsyncResult);
				Assert.Fail("No exception was thrown");
			}
			catch (Exception err)
			{
				context.SetResult(err);
			}

			yield return del.BeginInvoke(true, context.Callback, null);
			del.EndInvoke(context.LastAsyncResult);
			Assert.Fail("No exception was thrown");
		}

		private void DoSomething(bool fail)
		{
			if (fail)
			{
				Console.WriteLine("{0}: Throwing exception", Thread.CurrentThread.ManagedThreadId);
				throw new Exception();
			}
			else
			{
				TestContext.WriteLine("{0}: Doing something", Thread.CurrentThread.ManagedThreadId);
				Thread.Sleep(10);
				TestContext.WriteLine("{0}: Done something", Thread.CurrentThread.ManagedThreadId);
			}
		}
	}
}
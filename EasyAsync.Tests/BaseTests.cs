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
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EasyAsync.Tests
{
	[TestClass]
	public class BaseTests
	{
		public TestContext TestContext { get; set; }

		[TestMethod]
		public void CallbackIsInvoked()
		{
			var done = false;
			var timeout = DateTime.Now.AddSeconds(5);

			var result = AsyncIterator.BeginRun(DoSomethingAsync, iar => done = true);
			while(!done)
			{
				Assert.IsFalse(timeout < DateTime.Now, "Timeout");
			}
			AsyncIterator.EndRun(result);
		}

		#region Helpers
		private IEnumerable<IAsyncResult> DoSomethingAsync(IAsyncIteratorContext context)
		{
			Action del = DoSomething;

			yield return del.BeginInvoke(context.Callback, del);
			del.EndInvoke(context.LastAsyncResult);

			yield return del.BeginInvoke(context.Callback, del);
			del.EndInvoke(context.LastAsyncResult);
		}

		private void DoSomething()
		{
			TestContext.WriteLine("{0}: Doing something", Thread.CurrentThread.ManagedThreadId);
			Thread.Sleep(10);
			TestContext.WriteLine("{0}: Done something", Thread.CurrentThread.ManagedThreadId);
		}
		#endregion

		[TestMethod]
		public void EmptyIteratorCompletesSynchronously()
		{
			var result = AsyncIterator.BeginRun(c => new IAsyncResult[0]);

			Assert.IsTrue(result.IsCompleted, "The async result is not completed.");
			Assert.IsTrue(result.CompletedSynchronously, "The async result did not complete synchronously.");

			AsyncIterator.EndRun(result);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullIteratorFails()
		{
			AsyncIterator.BeginRun(c => null);
		}
	}
}
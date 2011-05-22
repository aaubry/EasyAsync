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

namespace EasyAsync
{
	/// <summary>
	/// Executes asynchronous iterators.
	/// </summary>
	public static class AsyncIterator
	{
		/// <summary>
		/// Runs the specified iterator synchronously.
		/// </summary>
		/// <param name="iterator">An iterator that executes asynchronous code.</param>
		/// <param name="state">An object containing state information for this asynchronous execution.</param>
		/// <returns></returns>
		public static object Run(Func<IAsyncIteratorContext, IEnumerable<IAsyncResult>> iterator, object state = null)
		{
			return EndRun(BeginRun(iterator, null, state));
		}

		/// <summary>
		/// Runs the specified iterator asynchronously.
		/// </summary>
		/// <param name="iterator">An iterator that executes asynchronous code.</param>
		/// <param name="callback">An optional <see cref="AsyncCallback"/> delegate that will be invoked when the asynchronous code completes.</param>
		/// <param name="state">An object containing state information for this asynchronous execution.</param>
		/// <returns></returns>
		public static IAsyncResult BeginRun(Func<IAsyncIteratorContext, IEnumerable<IAsyncResult>> iterator, AsyncCallback callback = null, object state = null)
		{
			if (iterator == null) throw new ArgumentNullException("iterator");

			var runner = new AsyncIteratorRunner();
			return runner.BeginRun(iterator(runner), callback, state);
		}

		/// <summary>
		/// Completes the execution of an asynchronous iterator execution.
		/// </summary>
		/// <param name="result">An <see cref="IAsyncResult"/> that references a pending execution.</param>
		/// <returns>Returns the result of the iterator, if any.</returns>
		/// <remarks>
		/// If the iterator has already finished executing, this method return immediately. Otherwise,
		/// it blocks until the iterator finishes executing.
		/// If an exception is thrown by the iterator, it will be rethrown when <see cref="EndRun"/> is invoked.
		/// </remarks>
		public static object EndRun(IAsyncResult result)
		{
			return ((AsyncIteratorAsyncResult)result).Runner.EndRun(result);
		}
	}
}
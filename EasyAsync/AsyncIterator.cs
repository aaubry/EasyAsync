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
		public static object Run<T>(AsyncIteratorDelegate<T> iterator, T state = default(T))
		{
			return EndRun(BeginRun(iterator, null, state));
		}

		/// <summary>
		/// Runs the specified iterator synchronously.
		/// </summary>
		/// <param name="iterator">An iterator that executes asynchronous code.</param>
		/// <returns></returns>
		public static object Run(AsyncIteratorDelegate iterator)
		{
			return EndRun(BeginRun(iterator, null));
		}

		/// <summary>
		/// Runs the specified iterator asynchronously.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="iterator">An iterator that executes asynchronous code.</param>
		/// <param name="callback">An optional <see cref="AsyncCallback"/> delegate that will be invoked when the asynchronous code completes.</param>
		/// <param name="state">An object containing state information for this asynchronous execution.</param>
		/// <param name="wrapExceptions">if set to <c>true</c> exceptions will be wrapped into <see cref="ApplicationException"/>.</param>
		/// <returns></returns>
		public static IAsyncResult BeginRun<T>(AsyncIteratorDelegate<T> iterator, AsyncCallback callback = null, T state = default(T), bool wrapExceptions = false)
		{
			if (iterator == null) throw new ArgumentNullException("iterator");

			var runner = new AsyncIteratorRunner<T>(state, wrapExceptions);
			return runner.BeginRun(iterator(runner), callback);
		}

		/// <summary>
		/// Runs the specified iterator asynchronously.
		/// </summary>
		/// <param name="iterator">An iterator that executes asynchronous code.</param>
		/// <param name="callback">An optional <see cref="AsyncCallback"/> delegate that will be invoked when the asynchronous code completes.</param>
		/// <param name="wrapExceptions">if set to <c>true</c> exceptions will be wrapped into <see cref="Exception"/>.</param>
		/// <returns></returns>
		public static IAsyncResult BeginRun(AsyncIteratorDelegate iterator, AsyncCallback callback = null, bool wrapExceptions = false)
		{
			return BeginRun<object>(c => iterator(c), callback, null, wrapExceptions);
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
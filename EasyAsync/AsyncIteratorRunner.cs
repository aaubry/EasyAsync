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
using System.Reflection;

namespace EasyAsync
{
	internal interface IAsyncIteratorRunner
	{
		object EndRun(IAsyncResult result);
	}

	internal class AsyncIteratorRunner<T> : IAsyncIteratorRunner, IAsyncIteratorContext<T>
	{
		private IEnumerator<IAsyncResult> _state;
		private AsyncCallback _callback;
		private readonly AsyncIteratorAsyncResult _result;
		private readonly bool _wrapExceptions;

		public AsyncIteratorRunner(T state, bool wrapExceptions)
		{
			_wrapExceptions = wrapExceptions;
			_result = new AsyncIteratorAsyncResult(state, this);
			Callback = ContinuationCallback;
		}

		public IAsyncResult BeginRun(IEnumerable<IAsyncResult> iterator, AsyncCallback callback)
		{
			if (iterator == null)
			{
				throw new ArgumentNullException("iterator");
			}

			_callback = callback;

			_state = iterator.GetEnumerator();

			bool hasNext;
			try
			{
				hasNext = _state.MoveNext();
			}
			catch (Exception err)
			{
				_result.Exception = err;
				hasNext = false;
			}

			if (!hasNext)
			{
				_result.CompletedSynchronously = true;
				CompleteIteration();
			}
			return _result;
		}

		public object EndRun(IAsyncResult result)
		{
			if (!ReferenceEquals(_result, result))
			{
				throw new ArgumentException("The IAsyncResult that was passed to EndRun() is not the one that was returned by BeginRun().", "result");
			}

			if (!_result.IsCompleted)
			{
				_result.AsyncWaitHandle.WaitOne();
			}

			if (_result.Exception != null)
			{
				if (_wrapExceptions)
				{
					throw new ApplicationException("Exception in async iterator. See InnerException for details.", _result.Exception);
				}
				else
				{
					var stackTrace = typeof(Exception).GetField("_remoteStackTraceString",
																 BindingFlags.Instance | BindingFlags.NonPublic);
					if (stackTrace != null)
					{
						stackTrace.SetValue(_result.Exception, _result.Exception.StackTrace + "\n");
					}

					throw _result.Exception;
				}
			}

			return _iteratorResult;
		}

		private void ContinuationCallback(IAsyncResult iar)
		{
			LastAsyncResult = iar;

			bool hasNext;
			try
			{
				hasNext = _state.MoveNext();
			}
			catch (Exception err)
			{
				_result.Exception = err;
				hasNext = false;
			}

			if (!hasNext)
			{
				CompleteIteration();
			}
		}

		private void CompleteIteration()
		{
			_result.Complete();
			_state.Dispose();
			if (_callback != null)
			{
				_callback(_result);
			}
		}

		private object _iteratorResult;

		#region IAsyncIteratorContext members
		public AsyncCallback Callback { get; private set; }
		public IAsyncResult LastAsyncResult { get; private set; }
		public T AsyncState { get { return (T)_result.AsyncState; } }

		public void SetResult(object result)
		{
			_iteratorResult = result;
		}
		#endregion
	}
}
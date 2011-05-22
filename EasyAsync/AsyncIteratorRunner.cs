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
	internal class AsyncIteratorRunner : IAsyncIteratorContext
	{
		private IEnumerator<IAsyncResult> _state;
		private AsyncCallback _callback;
		private AsyncIteratorAsyncResult _result;

		public AsyncIteratorRunner()
		{
			Callback = ContinuationCallback;
		}

		public IAsyncResult BeginRun(IEnumerable<IAsyncResult> iterator, AsyncCallback callback, object state)
		{
			if (iterator == null) throw new ArgumentNullException("iterator");

			_callback = callback;

			_state = iterator.GetEnumerator();

			_result = new AsyncIteratorAsyncResult(state, this);

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
			if(!ReferenceEquals(_result, result))
			{
				throw new ArgumentException("The IAsyncResult that was passed to EndRun() is not the one that was returned by BeginRun().", "result");
			}

			if(!_result.IsCompleted)
			{
				_result.AsyncWaitHandle.WaitOne();
			}
			
			if(_result.Exception != null)
			{
				throw _result.Exception;
			}

			return _ireratorResult;
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

		private object _ireratorResult;

		#region IAsyncIteratorContext members
		public AsyncCallback Callback { get; private set; }
		public IAsyncResult LastAsyncResult { get; private set; }
		public object AsyncState { get { return _result.AsyncState; } }

		public void SetResult(object result)
		{
			_ireratorResult = result;
		}
		#endregion
	}
}
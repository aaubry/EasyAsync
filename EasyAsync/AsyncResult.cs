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
using System.Threading;

namespace EasyAsync
{
	/// <summary>
	/// Implementation of <see cref="IAsyncResult"/> that does not rely on unmanaged resources.
	/// </summary>
	public class AsyncResult : IAsyncResult
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ExceptionAsyncResult"/> class.
		/// </summary>
		/// <param name="asyncState">State of the async.</param>
		public AsyncResult(object asyncState)
		{
			AsyncState = asyncState;
		}

		/// <summary>
		/// Signals the completion of the operation represented by this <see cref="ExceptionAsyncResult"/>.
		/// </summary>
		public void Complete()
		{
			IsCompleted = true;
			if (_waitHandle != null)
			{
				_waitHandle.Signal();
			}
		}

		private ManagedWaitHandle _waitHandle;

		/// <summary>
		/// Gets a <see cref="T:System.Threading.WaitHandle"/> that is used to wait for an asynchronous operation to complete.
		/// </summary>
		/// <value></value>
		/// <returns>
		/// A <see cref="T:System.Threading.WaitHandle"/> that is used to wait for an asynchronous operation to complete.
		/// </returns>
		public WaitHandle AsyncWaitHandle
		{
			get
			{
				if(_waitHandle == null)
				{
					var value = new ManagedWaitHandle(IsCompleted);
					if(Interlocked.CompareExchange(ref _waitHandle, value, null) != null)
					{
						((IDisposable)value).Dispose();
					}
				}
				
				return _waitHandle;
			}
		}

		/// <summary>
		/// Gets a user-defined object that qualifies or contains information about an asynchronous operation.
		/// </summary>
		/// <value></value>
		/// <returns>
		/// A user-defined object that qualifies or contains information about an asynchronous operation.
		/// </returns>
		public object AsyncState { get; private set; }

		/// <summary>
		/// Gets a value that indicates whether the asynchronous operation completed synchronously.
		/// </summary>
		/// <value></value>
		/// <returns>true if the asynchronous operation completed synchronously; otherwise, false.
		/// </returns>
		public bool CompletedSynchronously { get; set; }

		/// <summary>
		/// Gets a value that indicates whether the asynchronous operation has completed.
		/// </summary>
		/// <value></value>
		/// <returns>true if the operation is complete; otherwise, false.
		/// </returns>
		public bool IsCompleted { get; private set; }
	}
}
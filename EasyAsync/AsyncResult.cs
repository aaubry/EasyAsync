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
	internal class AsyncResult : IAsyncResult
	{
		public AsyncResult(object asyncState)
		{
			AsyncState = asyncState;
			_waitHandle = new Lazy<ManualResetEvent>(() => new ManualResetEvent(IsCompleted));
		}

		public void Complete()
		{
			IsCompleted = true;
			if(_waitHandle.IsValueCreated)
			{
				_waitHandle.Value.Set();
			}
		}

		private readonly Lazy<ManualResetEvent> _waitHandle;

		public object AsyncState { get; private set; }
		public WaitHandle AsyncWaitHandle { get { return _waitHandle.Value; } }
		public bool CompletedSynchronously { get; set; }
		public bool IsCompleted { get; set; }
		public Exception Exception { get; set; }
	}
}
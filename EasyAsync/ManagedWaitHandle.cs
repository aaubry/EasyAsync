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
using System.Threading;

namespace EasyAsync
{
	internal class ManagedWaitHandle : WaitHandle
	{
		private readonly object _monitor = new object();
		private bool _isSignaled;

		public ManagedWaitHandle(bool isSignaled)
		{
			_isSignaled = isSignaled;
		}

		public override bool WaitOne()
		{
			lock (_monitor)
			{
				return _isSignaled || Monitor.Wait(_monitor);
			}
		}

		public override bool WaitOne(int millisecondsTimeout, bool exitContext)
		{
			lock (_monitor)
			{
				return _isSignaled || Monitor.Wait(_monitor, millisecondsTimeout, exitContext);
			}
		}

		public void Signal()
		{
			lock (_monitor)
			{
				_isSignaled = true;
				Monitor.PulseAll(_monitor);
			}
		}
	}
}
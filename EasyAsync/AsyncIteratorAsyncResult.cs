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
namespace EasyAsync
{
	internal class AsyncIteratorAsyncResult : AsyncResult
	{
		public AsyncIteratorAsyncResult(object asyncState, AsyncIteratorRunner runner)
			: base(asyncState)
		{
			Runner = runner;
		}

		public AsyncIteratorRunner Runner { get; private set; }
	}
}
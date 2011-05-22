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
	/// Supports the implementation of asynchronous iterators.
	/// </summary>
	public interface IAsyncIteratorContext
	{
		/// <summary>
		/// Gets the <see cref="AsyncCallback"/> that should be passed to BeginXxx() methods.
		/// </summary>
		AsyncCallback Callback { get; }

		/// <summary>
		/// Gets the last <see cref="IAsyncResult"/> that was passed to the <see cref="Callback"/>.
		/// This <see cref="IAsyncResult"/> should be passed to the EndXxx() method.
		/// </summary>
		IAsyncResult LastAsyncResult { get; }

		/// <summary>
		/// Gets the state that was passed to <see cref="AsyncIterator.BeginRun"/>.
		/// </summary>
		object AsyncState { get; }

		/// <summary>
		/// Sets the result of the iterator.
		/// </summary>
		void SetResult(object result);
	}
}
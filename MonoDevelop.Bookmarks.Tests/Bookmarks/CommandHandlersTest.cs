//
// CommandHandlersTest.cs
//
// Author:
//       vmladenov <ventsislav.mladenov@gmail.com>
//
// Copyright (c) 2012 vmladenov
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using NUnit.Framework;
using System.Reflection;
using MonoDevelop.Bookmarks;

namespace MonoDevelop.Bookmarks.Tests.Bookmarks
{
	[TestFixture]
	public class CommandHandlersTest
	{
		public int GetBookmarkNumber(SetBookmarkHandler ch)
		{
			return 1;
		}

		[Test]
		public void SetCommandNumbers()
		{
			SetBookmarkHandler ch = new SetLocalBookmarkHandler0();
			Assert.AreEqual(0, ch.BookmarkNumber);
			ch = new SetLocalBookmarkHandler1();
			Assert.AreEqual(1, ch.BookmarkNumber);
			ch = new SetLocalBookmarkHandler2();
			Assert.AreEqual(2, ch.BookmarkNumber);
			ch = new SetLocalBookmarkHandler3();
			Assert.AreEqual(3, ch.BookmarkNumber);
			ch = new SetLocalBookmarkHandler4();
			Assert.AreEqual(4, ch.BookmarkNumber);
			ch = new SetLocalBookmarkHandler5();
			Assert.AreEqual(5, ch.BookmarkNumber);
			ch = new SetLocalBookmarkHandler6();
			Assert.AreEqual(6, ch.BookmarkNumber);
			ch = new SetLocalBookmarkHandler7();
			Assert.AreEqual(7, ch.BookmarkNumber);
			ch = new SetLocalBookmarkHandler8();
			Assert.AreEqual(8, ch.BookmarkNumber);
			ch = new SetLocalBookmarkHandler9();
			Assert.AreEqual(9, ch.BookmarkNumber);
		}
	}
}


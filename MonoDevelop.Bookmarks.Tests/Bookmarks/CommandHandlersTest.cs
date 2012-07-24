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
using MonoDevelop.Components.Commands;

namespace MonoDevelop.Bookmarks.Tests
{
	[TestFixture]
	public class CommandHandlersTest
	{
		[Test]
		public void SetLocalCommandNumbers()
		{
			SetBookmarkHandler ch = new SetLocalBookmarkHandler0();
			Assert.AreEqual(0, ch.BookmarkNumber);
			Assert.AreEqual(BookmarkType.Local, ch.BookmarkType);
			ch = new SetLocalBookmarkHandler1();
			Assert.AreEqual(1, ch.BookmarkNumber);
			Assert.AreEqual(BookmarkType.Local, ch.BookmarkType);
			ch = new SetLocalBookmarkHandler2();
			Assert.AreEqual(2, ch.BookmarkNumber);
			Assert.AreEqual(BookmarkType.Local, ch.BookmarkType);
			ch = new SetLocalBookmarkHandler3();
			Assert.AreEqual(3, ch.BookmarkNumber);
			Assert.AreEqual(BookmarkType.Local, ch.BookmarkType);
			ch = new SetLocalBookmarkHandler4();
			Assert.AreEqual(4, ch.BookmarkNumber);
			Assert.AreEqual(BookmarkType.Local, ch.BookmarkType);
			ch = new SetLocalBookmarkHandler5();
			Assert.AreEqual(5, ch.BookmarkNumber);
			Assert.AreEqual(BookmarkType.Local, ch.BookmarkType);
			ch = new SetLocalBookmarkHandler6();
			Assert.AreEqual(6, ch.BookmarkNumber);
			Assert.AreEqual(BookmarkType.Local, ch.BookmarkType);
			ch = new SetLocalBookmarkHandler7();
			Assert.AreEqual(7, ch.BookmarkNumber);
			Assert.AreEqual(BookmarkType.Local, ch.BookmarkType);
			ch = new SetLocalBookmarkHandler8();
			Assert.AreEqual(8, ch.BookmarkNumber);
			Assert.AreEqual(BookmarkType.Local, ch.BookmarkType);
			ch = new SetLocalBookmarkHandler9();
			Assert.AreEqual(9, ch.BookmarkNumber);
			Assert.AreEqual(BookmarkType.Local, ch.BookmarkType);
		}

		[Test]
		public void GoToLocalCommandNumbers()
		{
			GoToBookmarkHandler ch = new GoToLocalBookmarkHandler0();
			Assert.AreEqual(0, ch.BookmarkNumber);
			Assert.AreEqual(BookmarkType.Local, ch.BookmarkType);
			ch = new GoToLocalBookmarkHandler1();
			Assert.AreEqual(1, ch.BookmarkNumber);
			Assert.AreEqual(BookmarkType.Local, ch.BookmarkType);
			ch = new GoToLocalBookmarkHandler2();
			Assert.AreEqual(2, ch.BookmarkNumber);
			Assert.AreEqual(BookmarkType.Local, ch.BookmarkType);
			ch = new GoToLocalBookmarkHandler3();
			Assert.AreEqual(3, ch.BookmarkNumber);
			Assert.AreEqual(BookmarkType.Local, ch.BookmarkType);
			ch = new GoToLocalBookmarkHandler4();
			Assert.AreEqual(4, ch.BookmarkNumber);
			Assert.AreEqual(BookmarkType.Local, ch.BookmarkType);
			ch = new GoToLocalBookmarkHandler5();
			Assert.AreEqual(5, ch.BookmarkNumber);
			Assert.AreEqual(BookmarkType.Local, ch.BookmarkType);
			ch = new GoToLocalBookmarkHandler6();
			Assert.AreEqual(6, ch.BookmarkNumber);
			Assert.AreEqual(BookmarkType.Local, ch.BookmarkType);
			ch = new GoToLocalBookmarkHandler7();
			Assert.AreEqual(7, ch.BookmarkNumber);
			Assert.AreEqual(BookmarkType.Local, ch.BookmarkType);
			ch = new GoToLocalBookmarkHandler8();
			Assert.AreEqual(8, ch.BookmarkNumber);
			Assert.AreEqual(BookmarkType.Local, ch.BookmarkType);
			ch = new GoToLocalBookmarkHandler9();
			Assert.AreEqual(9, ch.BookmarkNumber);
			Assert.AreEqual(BookmarkType.Local, ch.BookmarkType);
		}

		[Test]
		public void ClearBookmarks()
		{
			ClearAllBookmarksHandler handler = new ClearAllBookmarksHandler();
			NumberBookmark bookmark = new NumberBookmark("TestPath.cs", 10, 4, 2, BookmarkType.Local);
			BookmarkService.Instance.AddBookmark(bookmark);

			bookmark = new NumberBookmark("TestPath.cs", 20, 4, 1, BookmarkType.Local);
			BookmarkService.Instance.AddBookmark(bookmark);

			bookmark = new NumberBookmark("TestPath1SAD.cs", 10, 4, 1, BookmarkType.Global);
			BookmarkService.Instance.AddBookmark(bookmark);


			Assert.AreEqual(3, BookmarkService.Instance.BookmarkCount);
			handler.RunTest();
			Assert.AreEqual(0, BookmarkService.Instance.BookmarkCount);
		}
	}
}


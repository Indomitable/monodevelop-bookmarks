//
// BookmarkStoreTest.cs
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
using MonoDevelop.Bookmarks;

namespace MonoDevelop.Bookmarks.Tests
{
	[TestFixture]
	public class BookmarkStoreTest
	{
		[SetUp]
		public void Init()
		{
			BookmarkService.Instance.Clear();
		}

		[Test]
		public void AddBookmark1()
		{
			NumberBookmark bookmark = new NumberBookmark("TestPath.cs", 10, 4, 2, BookmarkType.Local);
			BookmarkService.Instance.AddBookmark(bookmark);

			Assert.AreEqual(1, BookmarkService.Instance.BookmarkCount);

			bookmark = new NumberBookmark("TestPath.cs", 15, 20, 2, BookmarkType.Local);
			BookmarkService.Instance.AddBookmark(bookmark);
			Assert.AreEqual(1, BookmarkService.Instance.BookmarkCount);

			var bookmarkCheck = BookmarkService.Instance.GetBookmarkLocal("TestPath.cs", 2);
			Assert.AreEqual(15, bookmarkCheck.LineNumber);
			Assert.AreEqual(20, bookmarkCheck.Column);
		}

		[Test]
		public void AddBookmark2()
		{
			NumberBookmark bookmark = new NumberBookmark("TestPath.cs", 10, 4, 2, BookmarkType.Local);
			BookmarkService.Instance.AddBookmark(bookmark);
			
			Assert.AreEqual(1, BookmarkService.Instance.BookmarkCount);
			
			bookmark = new NumberBookmark("TestPath.cs", 10, 20, 2, BookmarkType.Global);
			BookmarkService.Instance.AddBookmark(bookmark);
			Assert.AreEqual(1, BookmarkService.Instance.BookmarkCount);
			
			var bookmarkCheck = BookmarkService.Instance.GetBookmarkLocal("TestPath.cs", 2);
			Assert.IsNull(bookmarkCheck);

			bookmarkCheck = BookmarkService.Instance.GetBookmarkGlobal(2);
			Assert.AreEqual(10, bookmarkCheck.LineNumber);
			Assert.AreEqual(20, bookmarkCheck.Column);
		}

		[Test]
		public void AddBookmark3()
		{
			NumberBookmark bookmark = new NumberBookmark("TestPath1SAD.cs", 10, 4, 3, BookmarkType.Global);
			BookmarkService.Instance.AddBookmark(bookmark);
			
			Assert.AreEqual(1, BookmarkService.Instance.BookmarkCount);
			
			bookmark = new NumberBookmark("TestPath.cs", 40, 20, 3, BookmarkType.Global);
			BookmarkService.Instance.AddBookmark(bookmark);

			Assert.AreEqual(1, BookmarkService.Instance.BookmarkCount);
			
			var bookmarkCheck = BookmarkService.Instance.GetBookmarkGlobal(3);
			Assert.AreEqual(40, bookmarkCheck.LineNumber);
			Assert.AreEqual(20, bookmarkCheck.Column);
			Assert.AreEqual("TestPath.cs", bookmark.FileName);
		}

		[Test]
		public void AddBookmark4()
		{
			NumberBookmark bookmark = new NumberBookmark("TestPath.cs", 15, 4, 2, BookmarkType.Local);
			BookmarkService.Instance.AddBookmark(bookmark);
			
			Assert.AreEqual(1, BookmarkService.Instance.BookmarkCount);
			
			bookmark = new NumberBookmark("TestPath.cs", 15, 20, 2, BookmarkType.Local);
			BookmarkService.Instance.AddBookmark(bookmark);
			Assert.AreEqual(0, BookmarkService.Instance.BookmarkCount);
		}

		[Test]
		public void AddBookmark5()
		{
			NumberBookmark bookmark = new NumberBookmark("TestPath.cs", 15, 4, 5, BookmarkType.Local);
			BookmarkService.Instance.AddBookmark(bookmark);
			
			Assert.AreEqual(1, BookmarkService.Instance.BookmarkCount);
			
			bookmark = new NumberBookmark("TestPath.cs", 15, 20, 5, BookmarkType.Global);
			BookmarkService.Instance.AddBookmark(bookmark);
			Assert.AreEqual(1, BookmarkService.Instance.BookmarkCount);

			var bookmarkCheck = BookmarkService.Instance.GetBookmarkGlobal(5);
			Assert.AreEqual(15, bookmarkCheck.LineNumber);
			Assert.AreEqual(20, bookmarkCheck.Column);
			Assert.AreEqual("TestPath.cs", bookmark.FileName);
		}

		[Test]
		public void AddBookmark6()
		{
			NumberBookmark bookmark = new NumberBookmark("TestPath.cs", 10, 4, 2, BookmarkType.Local);
			BookmarkService.Instance.AddBookmark(bookmark);
			
			Assert.AreEqual(1, BookmarkService.Instance.BookmarkCount);
			
			bookmark = new NumberBookmark("TestPath.cs", 15, 20, 3, BookmarkType.Local);
			BookmarkService.Instance.AddBookmark(bookmark);
			Assert.AreEqual(2, BookmarkService.Instance.BookmarkCount);
			
			var bookmarkCheck = BookmarkService.Instance.GetBookmarkLocal("TestPath.cs", 3);
			Assert.AreEqual(15, bookmarkCheck.LineNumber);
			Assert.AreEqual(20, bookmarkCheck.Column);
		}

		[Test]
		public void AddBookmark7()
		{
			NumberBookmark bookmark = new NumberBookmark("TestPath.cs", 20,17, 6, BookmarkType.Global);
			BookmarkService.Instance.AddBookmark(bookmark);
			
			Assert.AreEqual(1, BookmarkService.Instance.BookmarkCount);
			
			bookmark = new NumberBookmark("TestPath.cs", 20, 5, 6, BookmarkType.Global);
			BookmarkService.Instance.AddBookmark(bookmark);
			Assert.AreEqual(0, BookmarkService.Instance.BookmarkCount);
		}

	}
}


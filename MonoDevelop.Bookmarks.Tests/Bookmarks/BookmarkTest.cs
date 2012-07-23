//
// BookmarkTest.cs
//
// Author:
//       Ventsislav Mladenov <ventsislav.mladenov@gmail.com>
//
// Copyright (c) 2012 Ventsislav Mladenov
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
using System.Xml;
using System.Text;
using MonoDevelop.Bookmarks;

namespace MonoDevelop.Bookmarks.Tests
{
	[TestFixture]
	public class BookmarkTest
	{
		[Test]
		public void ToXml()
		{
			XmlDocument doc = new XmlDocument();
			var bookMark = new NumberBookmark();
			bookMark.FileName = "TestFile.cs";
			bookMark.LineNumber = 29;
			bookMark.Column = 10;
			bookMark.BookmarkType = BookmarkType.Global;
			bookMark.Number = 4;
			XmlElement element = bookMark.ToXml(doc);

			StringBuilder builder = new StringBuilder();
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = false;
			settings.Encoding = Encoding.UTF8;
			settings.OmitXmlDeclaration = true;
			XmlWriter writer = XmlWriter.Create(builder, settings);
			element.WriteTo(writer);
			writer.Flush();
			string value = "<NumberBookmark Type=\"1\" Number=\"4\" FileName=\"TestFile.cs\" LineNumber=\"29\" Column=\"10\" />";
			Assert.AreEqual(value, builder.ToString());
		}

		[Test]
		public void FromXml()
		{
			XmlDocument doc = new XmlDocument();
			XmlElement element = doc.CreateElement("NumberBookmark");
			element.SetAttribute("FileName", "TestFile12.cs");
			element.SetAttribute("LineNumber", "10");
			element.SetAttribute("Type", "0");
			element.SetAttribute("Number", "5");
			element.SetAttribute("Column", "24");
			var bookmark = NumberBookmark.FromXml(element);
			Assert.AreEqual("TestFile12.cs", bookmark.FileName);
			Assert.AreEqual(10, bookmark.LineNumber);
			Assert.AreEqual(BookmarkType.Local, bookmark.BookmarkType);
			Assert.AreEqual(5, bookmark.Number);
			Assert.AreEqual(24, bookmark.Column);
		}

		[Test]
		public void FromXmlNoValue()
		{
			XmlDocument doc = new XmlDocument();
			//Wrong Element Name
			XmlElement element = doc.CreateElement("NumberBookmark");
			element.SetAttribute("FileName", "");
			element.SetAttribute("LineNumber", "");
			element.SetAttribute("Type", "");
			element.SetAttribute("Number", "");
			element.SetAttribute("Column", "");
			var bookmark = NumberBookmark.FromXml(element);
			Assert.IsNull(bookmark);
		}
	}
}


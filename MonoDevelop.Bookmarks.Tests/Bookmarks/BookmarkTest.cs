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
			XmlElement element = bookMark.ToXml(doc);

			StringBuilder builder = new StringBuilder();
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = false;
			settings.Encoding = Encoding.UTF8;
			settings.OmitXmlDeclaration = true;
			XmlWriter writer = XmlWriter.Create(builder, settings);
			element.WriteTo(writer);
			writer.Flush();
			Assert.AreEqual("<NumberBookmark FileName=\"TestFile.cs\" LineNumber=\"29\" />", builder.ToString());
		}

		[Test]
		public void FromXml()
		{
			XmlDocument doc = new XmlDocument();
			XmlElement element = doc.CreateElement("NumberBookmark");
			element.SetAttribute("FileName", "TestFile12.cs");
			element.SetAttribute("LineNumber", "10");
			var bookmark = NumberBookmark.FromXml(element);
			Assert.AreEqual("TestFile12.cs", bookmark.FileName);
			//Assert.AreEqual(10, bookmark.LineNumber);
		}

		[Test]
		public void FromXmlWrongAttributes()
		{
			XmlDocument doc = new XmlDocument();
			//Wrong Element Name
			XmlElement element = doc.CreateElement("NumberBookmark1");
			element.SetAttribute("FileName", "TestFile12.cs");
			element.SetAttribute("LineNumber", "10");
			var bookmark = NumberBookmark.FromXml(element);
			Assert.IsNull(bookmark);

			element = doc.CreateElement("NumberBookmark");
			bookmark = NumberBookmark.FromXml(element);
			Assert.IsNull(bookmark);

			//Wrong FileName Name
			element.SetAttribute("FileName1", "TestFile12.cs");
			bookmark = NumberBookmark.FromXml(element);
			Assert.IsNull(bookmark);

			//FileName without LineNumber
			element.SetAttribute("FileName", "TestFile34.cs");
			bookmark = NumberBookmark.FromXml(element);
			Assert.IsNull(bookmark);

			//FileName with wrong LineNumber name
			element.SetAttribute("LineNumber2", "10");
			bookmark = NumberBookmark.FromXml(element);
			Assert.IsNull(bookmark);

			element.SetAttribute("LineNumber", "20");
			bookmark = NumberBookmark.FromXml(element);
			Assert.AreEqual("TestFile34.cs", bookmark.FileName);
			//Assert.AreEqual(20, bookmark.LineNumber);
		}

		[Test]
		public void FromXmlNoValue()
		{
			XmlDocument doc = new XmlDocument();
			//Wrong Element Name
			XmlElement element = doc.CreateElement("NumberBookmark");
			element.SetAttribute("FileName", "");
			element.SetAttribute("LineNumber", "10");
			var bookmark = NumberBookmark.FromXml(element);
			Assert.IsNull(bookmark);

			element.SetAttribute("FileName", "dsfs");
			element.SetAttribute("LineNumber", "");
			bookmark = NumberBookmark.FromXml(element);
			Assert.IsNull(bookmark);

			element.SetAttribute("FileName", "");
			element.SetAttribute("LineNumber", "");
			bookmark = NumberBookmark.FromXml(element);
			Assert.IsNull(bookmark);
		}
	}
}


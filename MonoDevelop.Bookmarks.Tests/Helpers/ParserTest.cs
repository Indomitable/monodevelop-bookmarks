//
// ParserTest.cs
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

namespace MonoDevelop.Bookmarks.Tests
{
	[TestFixture]
	public class ParserTest
	{
		[Test]
		public void Parse()
		{
			Assert.AreEqual(1, Parser.ParseInt("1"));
			Assert.AreEqual(2.5, Parser.ParseDouble("2.5"));
			Assert.AreEqual(1212, Parser.ParseLong("1212"));
			Assert.AreEqual(5, Parser.ParseShort("5"));
			Assert.AreEqual(5.212131231, Parser.ParseDecimal("5.212131231"));
			Assert.AreEqual(5.323, Parser.ParseDouble("5.323"));
		}

		[Test]
		public void EmptyString()
		{
			Assert.IsNull(Parser.ParseInt(""));
			Assert.IsNull(Parser.ParseLong(""));
			Assert.IsNull(Parser.ParseShort(""));
			Assert.IsNull(Parser.ParseDecimal(""));
			Assert.IsNull(Parser.ParseDouble(""));
		}

		[Test]
		public void NullParse()
		{
			Assert.IsNull(Parser.ParseInt(null));
			Assert.IsNull(Parser.ParseLong(null));
			Assert.IsNull(Parser.ParseShort(null));
			Assert.IsNull(Parser.ParseDecimal(null));
			Assert.IsNull(Parser.ParseDouble(null));
		}

		[Test]
		public void InvalidValue()
		{
			Assert.IsNull(Parser.ParseInt("32ada3"));
			Assert.IsNull(Parser.ParseLong("324ds5gf23"));
			Assert.IsNull(Parser.ParseShort("32s"));
			Assert.IsNull(Parser.ParseDecimal("3242.asd546"));
			Assert.IsNull(Parser.ParseDouble("456lfd54.fd"));
		}
	}
}


//
// NumberBookmark.cs
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
using System.Xml;
using Mono.TextEditor;
using System.Text;

namespace MonoDevelop.Bookmarks
{
    public class NumberBookmark
    {
        private const string BookmarkTag = "NumberBookmark";
        private const string BookmarkTypeTag = "Type";
        private const string BookmarkNumberTag = "Number";
        private const string FileNameTag = "FileName";
        private const string LineNumberTag = "LineNumber";
        private const string ColumnTag = "Column";

        public NumberBookmark()
        {
            
        }

        public NumberBookmark(MonoDevelop.Core.FilePath fileName, int lineNumber, int column, int bookmarkNumber, BookmarkType bookmarkType)
        {
            FileName = fileName;
            Column = column; 
            Number = bookmarkNumber;
            BookmarkType = bookmarkType;
            LineNumber = lineNumber;
        }

        public string FileName { get; set; }

        public int Number { get; set; }

        public BookmarkType BookmarkType { get; set; }

        public int Column { get; set; }

        public string LineContent { get; set; }

        public int LineNumber { get; set; }

        internal virtual XmlElement ToXml(XmlDocument doc)
        {
            XmlElement elem = doc.CreateElement(BookmarkTag);
            elem.SetAttribute(BookmarkTypeTag, Convert.ToString((int)this.BookmarkType));
            elem.SetAttribute(BookmarkNumberTag, Convert.ToString(this.Number));
            elem.SetAttribute(FileNameTag, this.FileName);
            elem.SetAttribute(LineNumberTag, Convert.ToString(this.LineNumber));
            elem.SetAttribute(ColumnTag, Convert.ToString(this.Column));
            return elem;
        }
        
        internal static NumberBookmark FromXml(XmlElement elem)
        {
            if (string.Equals(elem.Name, BookmarkTag, StringComparison.OrdinalIgnoreCase))
            {
                NumberBookmark bookMark = new NumberBookmark();

                var fileName = elem.GetAttribute(FileNameTag);
                if (string.IsNullOrEmpty(fileName))
                    return null;
                bookMark.FileName = fileName;

                var bookmarkType = Parser.ParseInt(elem.GetAttribute(BookmarkTypeTag));
                if (!bookmarkType.HasValue)
                    return null;
                bookMark.BookmarkType = (BookmarkType)bookmarkType.Value;

                var number = Parser.ParseInt(elem.GetAttribute(BookmarkNumberTag));
                if (!number.HasValue)
                    return null;
                bookMark.Number = number.Value;

                var lineNumber = Parser.ParseInt(elem.GetAttribute(LineNumberTag));
                if (!lineNumber.HasValue)
                    return null;
                bookMark.LineNumber = lineNumber.Value;

                var column = Parser.ParseInt(elem.GetAttribute(ColumnTag));
                if (!column.HasValue)
                    return null;
                bookMark.Column = column.Value;

                return bookMark;
            }
            return null;
        }
    }
}


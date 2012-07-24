//
// BookmarkCollection.cs
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
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using MonoDevelop.Ide.Gui;
using Mono.TextEditor;
using MonoDevelop.Projects.Text;
using System.IO;

namespace MonoDevelop.Bookmarks
{
    public class BookmarkCollection : List<NumberBookmark>
    {
        public BookmarkCollection()
        {
        }

        public XmlElement ToXml()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement elem = doc.CreateElement("BookmarkCollection");
            foreach (NumberBookmark bookmark in this)
            {
                elem.AppendChild(bookmark.ToXml(doc));
            }
            return elem;
        }

        public void Load(XmlElement root)
        {
            this.Clear();
            foreach (XmlElement child in root.ChildNodes)
            {
                var bookmark = NumberBookmark.FromXml(child);
                TryGetLineContent(bookmark);
                this.Add(bookmark);
            }
        }

        private void TryGetLineContent(NumberBookmark bookmark)
        {
            try
            {
                var fileName = Path.GetFullPath(bookmark.FileName);
                if (File.Exists(fileName))
                {
                    var lines = File.ReadAllLines(fileName);
                    if (lines.Length > bookmark.LineNumber)
                        bookmark.LineContent = lines[bookmark.LineNumber - 1].Trim();
                }
            } catch
            {
                bookmark.LineContent = string.Empty;
            }
        }

        public void InitBookmarksForDocument(Document document)
        {
            if (document == null)
                return;
            var textEditor = document.GetContent<ITextEditorDataProvider>();
            if (textEditor == null)
                return;
            var editor = textEditor.GetTextEditorData();
            if (editor == null)
                return;
            var fileName = document.FileName;
            if (this.Any(b => b.FileName == fileName))
            {
                foreach (var bookmark in this.Where(x => x.FileName == fileName))
                {
                    var line = editor.GetLine(bookmark.LineNumber);
                    if (line == null)
                        continue;
                    editor.Document.AddMarker(line, new NumberBookmarkMarker(bookmark));
                    editor.Document.RequestUpdate(new LineUpdate(bookmark.LineNumber));
                }
                editor.Document.CommitDocumentUpdate();
            }
        }

        public void FixLineNumbers(LineCountEventArgs arg)
        {
            if (arg.TextFile == null)
                return;
            var markedForRemove = new List<NumberBookmark>();
            foreach (var bookmark in this.Where(x => x.FileName == arg.TextFile.Name))
            {
                if (bookmark.LineNumber > arg.LineNumber)
                {
                    if (bookmark.LineNumber + arg.LineCount >= arg.LineNumber)
                    {
                        bookmark.LineNumber = bookmark.LineNumber + arg.LineCount;
                    } else
                        markedForRemove.Add(bookmark);
                }
            }
            foreach (var b in markedForRemove)
            {
                this.Remove(b);
            }
        }
    }
}


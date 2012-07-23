//
// BookmarkService.cs
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
using System.Linq;
using MonoDevelop.Projects.Text;
using MonoDevelop.Ide;
using System.Xml;
using Mono.Addins;
using Mono.TextEditor;

namespace MonoDevelop.Bookmarks
{
    public static class BookmarkService
    {
        static BookmarkCollection bookmarks = new BookmarkCollection();

        public static void Init()
        {
            TextFileService.LineCountChanged += OnLineCountChanged;
            IdeApp.Initialized += delegate
            {
                IdeApp.Workspace.StoringUserPreferences += OnStoreUserPrefs;
                IdeApp.Workspace.LoadingUserPreferences += OnLoadUserPrefs;
                IdeApp.Workspace.LastWorkspaceItemClosed += OnSolutionClosed;
                IdeApp.Workbench.DocumentOpened += OnDocumentOpened;
            };
        }  

        #region Events

        static void OnLineCountChanged(object ob, LineCountEventArgs a)
        {
            bookmarks.FixLineNumbers(a);
        }
        
        static void OnStoreUserPrefs(object s, UserPreferencesEventArgs args)
        {
            args.Properties.SetValue("MonoDevelop.Ide.BookmarkService.Bookmarks", bookmarks.ToXml());
        }
        
        static void OnLoadUserPrefs(object s, UserPreferencesEventArgs args)
        {
            XmlElement elem = args.Properties.GetValue<XmlElement>("MonoDevelop.Ide.BookmarkService.Bookmarks");
            if (elem == null)
                return;
            bookmarks.Load(elem);
            bookmarks.InitBookmarksForDocument(IdeApp.Workbench.ActiveDocument);
        }
        
        static void OnSolutionClosed(object s, EventArgs args)
        {
            Clear();
        }

        static void OnDocumentOpened(object sender, MonoDevelop.Ide.Gui.DocumentEventArgs e)
        {
            bookmarks.InitBookmarksForDocument(e.Document);
        }

        #endregion

        internal static int BookmarkCount { get { return bookmarks.Count; } }

        internal static void Clear()
        {
            foreach (var b in bookmarks)
            {
                RemoveMarkerFromDocument(b);
            }
            bookmarks.Clear();
        }

        internal static void AddBookmark(NumberBookmark bookmark)
        {
            NumberBookmark sameBookmark = null;
            if (bookmark.BookmarkType == BookmarkType.Local)
            {
                sameBookmark = bookmarks.FirstOrDefault(b => string.Equals(b.FileName, bookmark.FileName, StringComparison.OrdinalIgnoreCase) && 
                    b.BookmarkType == BookmarkType.Local && b.Number == bookmark.Number);
            } else
            {
                sameBookmark = bookmarks.FirstOrDefault(b => b.BookmarkType == BookmarkType.Global && b.Number == bookmark.Number);
            }
            if (sameBookmark != null)
            {
                bookmarks.Remove(sameBookmark); //Remove bookmark from store;
                RemoveMarkerFromDocument(sameBookmark); //Remove marker from document
            }
            var lineBookmark = bookmarks.FirstOrDefault(b => string.Equals(b.FileName, bookmark.FileName, StringComparison.OrdinalIgnoreCase) && 
                                                                           b.LineNumber == bookmark.LineNumber);
            if (lineBookmark != null)
            {
                bookmarks.Remove(lineBookmark);
                RemoveMarkerFromDocument(lineBookmark);
            }

            bookmarks.Add(bookmark);
            AddMarkerToLine(bookmark);
        }

        private static TextDocument GetActiveDocument()
        {
            if (IdeApp.Workbench == null)
                return null;
            var activeDocument = IdeApp.Workbench.ActiveDocument;
            if (activeDocument == null)
                return null;
            var textEditor = activeDocument.GetContent<ITextEditorDataProvider>();
            if (textEditor == null)
                return null;
            var editorData = textEditor.GetTextEditorData();
            if (editorData == null)
                return null;
            return editorData.Document;
        }

        private static void RemoveMarkerFromDocument(NumberBookmark bookmark)
        {
            var document = GetActiveDocument();
            if (document == null)
                return;

            if (string.Equals(bookmark.FileName, document.FileName))
            {
                var line = document.GetLine(bookmark.LineNumber);
                if (line == null)
                    return;
                document.RemoveMarker(line, typeof (NumberBookmarkMarker));
                document.RequestUpdate(new LineUpdate(line.LineNumber));
                document.CommitDocumentUpdate();
            }
        }

        private static void AddMarkerToLine(NumberBookmark bookmark)
        {
            var document = GetActiveDocument();
            if (document == null)
                return;
            if (string.Equals(bookmark.FileName, document.FileName))
            {
                var line = document.GetLine(bookmark.LineNumber);
                if (line == null)
                    return;
                document.AddMarker(line, new NumberBookmarkMarker(bookmark));
                document.RequestUpdate(new LineUpdate(line.LineNumber));
                document.CommitDocumentUpdate();
            }
        }

        internal static NumberBookmark GetBookmarkLocal(string fileName, int number)
        {
            return bookmarks.SingleOrDefault(b => string.Equals(b.FileName, fileName, StringComparison.OrdinalIgnoreCase) && 
                b.Number == number && b.BookmarkType == BookmarkType.Local);
        }

        internal static NumberBookmark GetBookmarkGlobal(int number)
        {
            return bookmarks.SingleOrDefault(b => b.Number == number && b.BookmarkType == BookmarkType.Global);
        }

        internal static bool CheckLineForBookmark(string fileName, int lineNumber)
        {
            var bookmark = bookmarks.SingleOrDefault(b => string.Equals(b.FileName, fileName, StringComparison.OrdinalIgnoreCase) && 
                b.LineNumber == lineNumber);
            return bookmark != null;
        }
    }
}


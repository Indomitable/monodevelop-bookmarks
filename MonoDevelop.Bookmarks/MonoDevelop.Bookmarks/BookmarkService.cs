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
using MonoDevelop.Ide;
using System.Xml;
using Mono.TextEditor;
using System.Collections.Generic;
using MonoDevelop.Ide.TextEditing;

namespace MonoDevelop.Bookmarks
{
    public class BookmarkService
    {
        private BookmarkCollection bookmarks = new BookmarkCollection();
        private static BookmarkService _instance;

        public static BookmarkService Instance
        {
            get
            {
                return _instance ?? (_instance = new BookmarkService());
            }
        }

        public void Init()
        {
			TextEditorService.LineCountChanged += OnLineCountChanged;
            IdeApp.Initialized += delegate
            {
                IdeApp.Workspace.StoringUserPreferences += OnStoreUserPrefs;
                IdeApp.Workspace.LoadingUserPreferences += OnLoadUserPrefs;
                IdeApp.Workspace.LastWorkspaceItemClosed += OnSolutionClosed;
                IdeApp.Workbench.DocumentOpened += OnDocumentOpened;
				IdeApp.Workbench.ActiveDocumentChanged += (sender, e) => RaiseFileChange();
            };
        }  

        #region Event Handlers

        private void OnLineCountChanged(object ob, LineCountEventArgs a)
        {
            bookmarks.FixLineNumbers(a);
            RaiseBookmarksChange();
        }
        
        private void OnStoreUserPrefs(object s, UserPreferencesEventArgs args)
        {
            args.Properties.SetValue("MonoDevelop.Ide.BookmarkService.Bookmarks", bookmarks.ToXml());
        }
        
        private void OnLoadUserPrefs(object s, UserPreferencesEventArgs args)
        {
            XmlElement elem = args.Properties.GetValue<XmlElement>("MonoDevelop.Ide.BookmarkService.Bookmarks");
            if (elem == null)
                return;
            bookmarks.Load(elem);
            RaiseBookmarksChange();
            foreach (var doc in IdeApp.Workbench.Documents)
            {
                bookmarks.InitBookmarksForDocument(doc);
            }
        }
        
        private void OnSolutionClosed(object s, EventArgs args)
        {
            Clear();
        }

        private void OnDocumentOpened(object sender, MonoDevelop.Ide.Gui.DocumentEventArgs e)
        {
            bookmarks.InitBookmarksForDocument(e.Document);
			RaiseFileChange();
        }

        #endregion

        #region Private members

        private TextDocument GetActiveDocument()
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
        
        private void RemoveMarkerFromDocument(NumberBookmark bookmark)
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
        
        private void AddMarkerToLine(NumberBookmark bookmark)
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

        private void InternalDeleteBookmark(NumberBookmark bookmark)
        {
            bookmarks.Remove(bookmark);
            RemoveMarkerFromDocument(bookmark); 
        }

        private void RaiseBookmarksChange()
        {
            if (OnBookmarksChange != null)
                OnBookmarksChange();
        }

		private void RaiseFileChange()
		{
			if (OnFileChage != null)
				OnFileChage();
		}

        #endregion

        internal int BookmarkCount { get { return bookmarks.Count; } }

        /// <summary>
        /// Clear all bookmarks
        /// </summary>
        internal void Clear()
        {
            foreach (var b in bookmarks)
            {
                RemoveMarkerFromDocument(b);
            }
            bookmarks.Clear();
            RaiseBookmarksChange();
        }

        /// <summary>
        /// Adds the bookmark. Raises event for change
        /// </summary>
        /// <param name='bookmark'>
        /// Bookmark.
        /// </param>
        internal void AddBookmark(NumberBookmark bookmark)
        {
            NumberBookmark sameBookmark = null;
            //Check for same bookmar on differnt line
            if (bookmark.BookmarkType == BookmarkType.Local) //If local add file condition
            {
                sameBookmark = bookmarks.FirstOrDefault(b => string.Equals(b.FileName, bookmark.FileName, StringComparison.OrdinalIgnoreCase) && 
                    b.BookmarkType == BookmarkType.Local && b.Number == bookmark.Number &&
                    b.LineNumber != bookmark.LineNumber);
            } else
            {
                sameBookmark = bookmarks.FirstOrDefault(b => b.BookmarkType == BookmarkType.Global && b.Number == bookmark.Number
                    && !(string.Equals(b.FileName, bookmark.FileName) && b.LineNumber == bookmark.LineNumber));
            
            }
            if (sameBookmark != null)
            {
                InternalDeleteBookmark(sameBookmark);
            }

            //Same line differnet number - replace the bookmark
            var lineBookmark = bookmarks.FirstOrDefault(b => string.Equals(b.FileName, bookmark.FileName, StringComparison.OrdinalIgnoreCase) && 
                b.LineNumber == bookmark.LineNumber && 
                (b.Number != bookmark.Number || b.BookmarkType != bookmark.BookmarkType));
            if (lineBookmark != null)
            {
                InternalDeleteBookmark(lineBookmark);
            }

            //Same line same number and type - clear the bookmark
            var lineBookmarkSameNumber = bookmarks.FirstOrDefault(b => string.Equals(b.FileName, bookmark.FileName, StringComparison.OrdinalIgnoreCase) && 
                b.LineNumber == bookmark.LineNumber && 
                b.Number == bookmark.Number && b.BookmarkType == bookmark.BookmarkType);
            if (lineBookmarkSameNumber != null) //Clear bookmark.
            {
                DeleteBookmark(lineBookmarkSameNumber);
                return;
            }
            bookmarks.Add(bookmark);
            AddMarkerToLine(bookmark);
            RaiseBookmarksChange();
        }

        /// <summary>
        /// Deletes the bookmark and raise event for change
        /// </summary>
        /// <param name='bookmark'>
        /// Bookmark for deletion
        /// </param>
        internal void DeleteBookmark(NumberBookmark bookmark)
        {
            InternalDeleteBookmark(bookmark);
            RaiseBookmarksChange();
        }

        /// <summary>
        /// Gets local bookmark.
        /// </summary>
        /// <returns>
        /// Local bookmark .
        /// </returns>
        /// <param name='fileName'>
        /// File name.
        /// </param>
        /// <param name='number'>
        /// Bookmark Number.
        /// </param>
        internal NumberBookmark GetBookmarkLocal(string fileName, int number)
        {
            return bookmarks.SingleOrDefault(b => string.Equals(b.FileName, fileName, StringComparison.OrdinalIgnoreCase) && 
                b.Number == number && b.BookmarkType == BookmarkType.Local);
        }

        /// <summary>
        /// Gets global bookmark.
        /// </summary>
        /// <returns>
        /// Global bookmark.
        /// </returns>
        /// <param name='number'>
        /// Number.
        /// </param>
        internal NumberBookmark GetBookmarkGlobal(int number)
        {
            return bookmarks.SingleOrDefault(b => b.Number == number && b.BookmarkType == BookmarkType.Global);
        }

        /// <summary>
        /// Checks if line has bookmark.
        /// </summary>
        /// <returns>
        /// true has bookmar, false - no bookmark
        /// </returns>
        /// <param name='fileName'>
        /// File name.
        /// </param>
        /// <param name='lineNumber'>
        /// Line number.
        /// </param>
        internal bool CheckLineForBookmark(string fileName, int lineNumber)
        {
            var bookmark = bookmarks.SingleOrDefault(b => string.Equals(b.FileName, fileName, StringComparison.OrdinalIgnoreCase) && 
                b.LineNumber == lineNumber);
            return bookmark != null;
        }

        public event Action OnBookmarksChange;

		public event Action OnFileChage;

        public IEnumerable<NumberBookmark> Bookmarks
        {
			get
			{
				return bookmarks;
			}
        }
    }
}


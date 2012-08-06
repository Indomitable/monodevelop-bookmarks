//
// GoToGlobalBookmarkHandler.cs
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
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;

namespace MonoDevelop.Bookmarks
{
    public abstract class GoToGlobalBookmarkHandler : CommandHandler
    {
        internal protected abstract int BookmarkNumber { get; }
        
        internal protected BookmarkType BookmarkType { get { return BookmarkType.Global; } }
        
        protected override void Run ()
        {
            var bookmark = BookmarkService.Instance.GetBookmarkGlobal(this.BookmarkNumber);
            IdeApp.Workbench.OpenDocument(bookmark.FileName, bookmark.LineNumber, bookmark.Column);
        }
        
        protected override void Update (CommandInfo info)
        {
            var bookmark = BookmarkService.Instance.GetBookmarkGlobal(this.BookmarkNumber);
            info.Enabled = bookmark != null;
        }
    }


    #region Go To Global BookMark Handler
    
    public class GoToGlobalBookmarkHandler0 : GoToGlobalBookmarkHandler
    {
        internal protected override int BookmarkNumber { get { return 0; } }
    }
    
    public class GoToGlobalBookmarkHandler1 : GoToGlobalBookmarkHandler
    {
        internal protected override int BookmarkNumber { get { return 1; } }
    }
    
    public class GoToGlobalBookmarkHandler2 : GoToGlobalBookmarkHandler
    {
        internal protected override int BookmarkNumber { get { return 2; } }
    }
    
    public class GoToGlobalBookmarkHandler3 : GoToGlobalBookmarkHandler
    {
        internal protected override int BookmarkNumber { get { return 3; } }
    }
    
    public class GoToGlobalBookmarkHandler4 : GoToGlobalBookmarkHandler
    {
        internal protected override int BookmarkNumber { get { return 4; } }
    }
    
    public class GoToGlobalBookmarkHandler5 : GoToGlobalBookmarkHandler
    {
        internal protected override int BookmarkNumber { get { return 5; } }
    }
    
    public class GoToGlobalBookmarkHandler6 : GoToGlobalBookmarkHandler
    {
        internal protected override int BookmarkNumber { get { return 6; } }
    }
    
    public class GoToGlobalBookmarkHandler7 : GoToGlobalBookmarkHandler
    {
        internal protected override int BookmarkNumber { get { return 7; } }
    }
    
    public class GoToGlobalBookmarkHandler8 : GoToGlobalBookmarkHandler
    {
        internal protected override int BookmarkNumber { get { return 8; } }
    }
    
    public class GoToGlobalBookmarkHandler9 : GoToGlobalBookmarkHandler
    {
        internal protected override int BookmarkNumber { get { return 9; } }
    }
    
    #endregion
}


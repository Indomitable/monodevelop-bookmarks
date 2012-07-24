//
// SetBookmarkHandler.cs
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
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using Mono.TextEditor;
using MonoDevelop.Ide.Gui;

namespace MonoDevelop.Bookmarks
{
    public abstract class SetBookmarkHandler : CommandHandler
    {
        internal protected abstract int BookmarkNumber { get; }

        internal protected abstract BookmarkType BookmarkType { get; }

        protected override void Run()
        {
            var editor = IdeApp.Workbench.ActiveDocument.Editor;
            var currentLine = editor.GetLineByOffset(editor.Caret.Offset);
            NumberBookmark bookmark = new NumberBookmark(editor.FileName, currentLine.LineNumber, editor.Caret.Column, BookmarkNumber, BookmarkType);
            bookmark.LineContent = editor.GetLineText(currentLine.LineNumber).Trim();
            BookmarkService.Instance.AddBookmark(bookmark);
        }
        
        protected override void Update(CommandInfo info)
        {
            var activeDocument = IdeApp.Workbench.ActiveDocument;
            if (activeDocument == null || activeDocument.Editor == null)
            {
                info.Enabled = false;
                return;
            }
            var textEditor = activeDocument.GetContent<ITextEditorDataProvider>();
            info.Enabled = activeDocument.Editor.Document != null && textEditor != null;
        }
    }

    public abstract class SetLocalBookmarkHandler : SetBookmarkHandler
    {
        internal protected override BookmarkType BookmarkType { get { return BookmarkType.Local; } }
    }

    public abstract class SetGlobalBookmarkHandler : SetBookmarkHandler
    {
        internal protected override BookmarkType BookmarkType { get { return BookmarkType.Global; } }
    }

    #region Set Local BookMark Handler

    public class SetLocalBookmarkHandler0 : SetLocalBookmarkHandler
    {
        internal protected override int BookmarkNumber { get { return 0; } }
    }

    public class SetLocalBookmarkHandler1 : SetLocalBookmarkHandler
    {
        internal protected override int BookmarkNumber { get { return 1; } }
    }

    public class SetLocalBookmarkHandler2 : SetLocalBookmarkHandler
    {
        internal protected override int BookmarkNumber { get { return 2; } }
    }

    public class SetLocalBookmarkHandler3 : SetLocalBookmarkHandler
    {
        internal protected override int BookmarkNumber { get { return 3; } }
    }

    public class SetLocalBookmarkHandler4 : SetLocalBookmarkHandler
    {
        internal protected override int BookmarkNumber { get { return 4; } }
    }

    public class SetLocalBookmarkHandler5 : SetLocalBookmarkHandler
    {
        internal protected override int BookmarkNumber { get { return 5; } }
    }

    public class SetLocalBookmarkHandler6 : SetLocalBookmarkHandler
    {
        internal protected override int BookmarkNumber { get { return 6; } }
    }

    public class SetLocalBookmarkHandler7 : SetLocalBookmarkHandler
    {
        internal protected override int BookmarkNumber { get { return 7; } }
    }

    public class SetLocalBookmarkHandler8 : SetLocalBookmarkHandler
    {
        internal protected override int BookmarkNumber { get { return 8; } }
    }

    public class SetLocalBookmarkHandler9 : SetLocalBookmarkHandler
    {
        internal protected override int BookmarkNumber { get { return 9; } }
    }

    #endregion
}


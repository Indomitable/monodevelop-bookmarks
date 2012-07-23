//
// GoToBookmarkHandler.cs
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
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using Mono.TextEditor;
using MonoDevelop.Ide.Gui;

namespace MonoDevelop.Bookmarks
{
	public abstract class GoToBookmarkHandler : CommandHandler
	{
	    internal protected abstract int BookmarkNumber { get; }
		
        internal protected abstract BookmarkType BookmarkType { get; }
		
		protected override void Run ()
		{
			var activeDocument = IdeApp.Workbench.ActiveDocument;
            var textEditor = activeDocument.GetContent<ITextEditorDataProvider>();
            var data = GetLineWithBookmark(textEditor.GetTextEditorData());
            activeDocument.Editor.Caret.Offset = data.Item1.Offset;
			activeDocument.Editor.Caret.Column = data.Item2.Column;
		}
		
		protected override void Update (CommandInfo info)
		{
			var activeDocument = IdeApp.Workbench.ActiveDocument;
			var textEditor = activeDocument.GetContent<ITextEditorDataProvider>();
			if (activeDocument == null || textEditor == null)
			{
				info.Enabled = false;
				return;
			}
			var data = GetLineWithBookmark(textEditor.GetTextEditorData());
            info.Enabled = data != null && data.Item1 != null;
		}

		private Tuple<DocumentLine, NumberBookmark> GetLineWithBookmark(TextEditorData editor)
		{
            var bookmark = BookmarkType == BookmarkType.Local ? BookmarkService.GetBookmarkLocal(editor.FileName, this.BookmarkNumber) :
                                                                BookmarkService.GetBookmarkGlobal(this.BookmarkNumber);
            if (bookmark == null)
                return null;
            return new Tuple<DocumentLine, NumberBookmark>(editor.GetLine(bookmark.LineNumber), bookmark);
		}
	}

	public abstract class GoToLocalBookmarkHandler : GoToBookmarkHandler
	{
        internal protected override BookmarkType BookmarkType { get { return BookmarkType.Local; } }
	}

	public abstract class GoToGlobalBookmarkHandler : GoToBookmarkHandler
	{
        internal protected override BookmarkType BookmarkType { get { return BookmarkType.Global; } }
	}

	#region Go To Local BookMark Handler

	public class GoToLocalBookmarkHandler0 : GoToLocalBookmarkHandler
	{
        internal protected override int BookmarkNumber { get { return 0; } }
	}

	public class GoToLocalBookmarkHandler1 : GoToLocalBookmarkHandler
	{
        internal protected override int BookmarkNumber { get { return 1; } }
	}

	public class GoToLocalBookmarkHandler2 : GoToLocalBookmarkHandler
	{
        internal protected override int BookmarkNumber { get { return 2; } }
	}

	public class GoToLocalBookmarkHandler3 : GoToLocalBookmarkHandler
	{
        internal protected override int BookmarkNumber { get { return 3; } }
	}

	public class GoToLocalBookmarkHandler4 : GoToLocalBookmarkHandler
	{
        internal protected override int BookmarkNumber { get { return 4; } }
	}

	public class GoToLocalBookmarkHandler5 : GoToLocalBookmarkHandler
	{
        internal protected override int BookmarkNumber { get { return 5; } }
	}

	public class GoToLocalBookmarkHandler6 : GoToLocalBookmarkHandler
	{
        internal protected override int BookmarkNumber { get { return 6; } }
	}

	public class GoToLocalBookmarkHandler7 : GoToLocalBookmarkHandler
	{
        internal protected override int BookmarkNumber { get { return 7; } }
	}

	public class GoToLocalBookmarkHandler8 : GoToLocalBookmarkHandler
	{
        internal protected override int BookmarkNumber { get { return 8; } }
	}

	public class GoToLocalBookmarkHandler9 : GoToLocalBookmarkHandler
	{
        internal protected override int BookmarkNumber { get { return 9; } }
	}

	#endregion
}


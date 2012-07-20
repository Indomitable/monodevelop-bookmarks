using System;
using System.Linq;
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

		protected override void Run ()
		{
			var activeDocument = IdeApp.Workbench.ActiveDocument;
			var editor = activeDocument.Editor;
			var currentLine = editor.GetLineByOffset(editor.Caret.Offset);
			NumberBookmark bookmark = new NumberBookmark(editor.FileName, currentLine, editor.Caret.Column, BookmarkNumber, BookmarkType);
			ClearMarkers(editor, currentLine);
			AddMarkerToLine(editor, currentLine, bookmark);
		}
		
		protected override void Update (CommandInfo info)
		{
			var activeDocument = IdeApp.Workbench.ActiveDocument;
			if (activeDocument == null || activeDocument.Editor ==null)
			{
				info.Enabled = false;
				return;
			}
			var textEditor = activeDocument.GetContent<ITextEditorDataProvider>();
			info.Enabled = activeDocument.Editor.Document != null && textEditor != null;
		}

		private void ClearMarkers(TextEditorData editor, DocumentLine currentLine)
		{
			//Clear line bookmarks
			if (Helpers.DocumentLineHelper.IsLineHasBookMark(currentLine))
			{
				RemoveMarkerFromLine(editor, currentLine);
			}

			//Remove bookmarks with same number
			DocumentLine line = editor.Lines.FirstOrDefault(x => Helpers.DocumentLineHelper.IsLineHasBookMark(x) && 
			                                                	 Helpers.DocumentLineHelper.GetBookmark(x).Bookmark.BookmarkType == BookmarkType &&
			                                       				 Helpers.DocumentLineHelper.GetBookmark(x).Bookmark.Number == BookmarkNumber);
			RemoveMarkerFromLine(editor, line);
		}

		private void RemoveMarkerFromLine(TextEditorData editor, DocumentLine line)
		{
			if (line == null)
				return;
			editor.Document.RemoveMarker(line, typeof(NumberBookmarkMarker));
			editor.Document.RequestUpdate (new LineUpdate (line.LineNumber));
			editor.Document.CommitDocumentUpdate ();
		}

		private void AddMarkerToLine(TextEditorData editor, DocumentLine line, NumberBookmark bookmark)
		{

			editor.Document.AddMarker (line, new NumberBookmarkMarker (bookmark));
			editor.Document.RequestUpdate (new LineUpdate (line.LineNumber));
			editor.Document.CommitDocumentUpdate();
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


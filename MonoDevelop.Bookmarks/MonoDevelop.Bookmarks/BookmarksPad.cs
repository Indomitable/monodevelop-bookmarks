//
// BookmarksPad.cs
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
using MonoDevelop.Ide.Gui;
using Gtk;
using MonoDevelop.Components;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide.Gui.Components;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Commands;
using Mono.TextEditor;
using System.Collections.Generic;

namespace MonoDevelop.Bookmarks
{
	public class BookmarksPad : IPadContent
	{
		PadTreeView localTree;
		PadTreeView globalTree;
		PadTreeView allTree;
		TreeStore localStore;
		TreeStore globalStore;
		TreeStore allStore;
		Notebook notebook;
		TreeViewState treeState;
		CommandEntrySet menuSet;
		System.Action onBookmarksChanged;
		System.Action onFileChanged;

		enum Columns
		{
			Icon,
			FileName,
			Bookmark,
			LineNumber,
			LineContent
		}

		enum PadCommands
		{
			GoToBookmark
		}

        #region IDisposable implementation

		public void Dispose ()
		{
			BookmarkService.Instance.OnBookmarksChange -= onBookmarksChanged;
			BookmarkService.Instance.OnFileChage -= onFileChanged;
		}

        #endregion

        #region IPadContent implementation

		public void Initialize (IPadWindow window)
		{
			InitPopupMenu();
            
			// The breakpoint list
			localStore = new TreeStore (typeof(string), typeof(string), typeof(object), typeof(string), typeof(string));
			localTree = CreateTree (localStore);

			globalStore = new TreeStore (typeof(string), typeof(string), typeof(object), typeof(string), typeof(string));
			globalTree = CreateTree (globalStore);

			allStore = new TreeStore (typeof(string), typeof(string), typeof(object), typeof(string), typeof(string));
			allTree = CreateTree (allStore);

			notebook = new Notebook ();
			notebook.TabPos = PositionType.Top;

			var localControl = new ScrolledWindow ();
			localControl.ShadowType = ShadowType.None;
			localControl.Add (localTree);

			var globalControl = new ScrolledWindow ();
			globalControl.ShadowType = ShadowType.None;
			globalControl.Add (globalTree);

			var allControl = new ScrolledWindow ();
			allControl.ShadowType = ShadowType.None;
			allControl.Add (allTree);

			Label localLabel = new Label ("Local bookmarks");
			notebook.AppendPage (localControl, localLabel);

			Label globalLabel = new Label ("Global bookmarks");
			notebook.AppendPage (globalControl, globalLabel);

			Label allLabel = new Label ("All bookmarks");
			notebook.AppendPage (allControl, allLabel);

			notebook.ShowAll ();

			UpdateDisplay ();

			onBookmarksChanged = DispatchService.GuiDispatch<System.Action> (OnBookmarksChanged);
			BookmarkService.Instance.OnBookmarksChange += onBookmarksChanged;

			onFileChanged = DispatchService.GuiDispatch<System.Action>(OnFileChanged);
			BookmarkService.Instance.OnFileChage += onFileChanged;

			notebook.SwitchPage += (o, args) => { UpdateDisplay(); };
            
		}

		private void InitPopupMenu()
		{
			ActionCommand gotoCmd = new ActionCommand (PadCommands.GoToBookmark, GettextCatalog.GetString ("Go to bookmark"));
			
			menuSet = new CommandEntrySet ();
			menuSet.Add (gotoCmd);
			menuSet.AddSeparator ();
			menuSet.AddItem (EditCommands.Delete);
			menuSet.AddSeparator ();
			menuSet.AddItem (BookmarkCommands.ClearAllBookmarks);
		}

		private PadTreeView CreateTree (TreeStore store)
		{
			var tree = new PadTreeView ();
			tree.Model = store;
			tree.RulesHint = true;
			tree.HeadersVisible = true;
			tree.DoPopupMenu = ShowPopup;
			
			treeState = new TreeViewState (tree, (int)Columns.Bookmark);
			
			TreeViewColumn col = new TreeViewColumn ();
			CellRenderer crp = new CellRendererBookmark ();
			col.PackStart (crp, true);
			col.AddAttribute (crp, "stock-id", (int)Columns.Icon);
			tree.AppendColumn (col);
			
			col = new TreeViewColumn ();
			CellRenderer crt = tree.TextRenderer;
			col.Title = GettextCatalog.GetString ("Name");
			col.PackStart (crt, true);
			col.AddAttribute (crt, "text", (int)Columns.FileName);
			col.Resizable = true;
			//col.Expand = true;
			col.Alignment = 0.0f;
			
			tree.AppendColumn (col);
			col = tree.AppendColumn (GettextCatalog.GetString ("Line Number"), crt, "text", (int)Columns.LineNumber);
			col.Resizable = true;
			
			col = tree.AppendColumn (GettextCatalog.GetString ("Line Content"), crt, "text", (int)Columns.LineContent);
			col.Resizable = true;
			tree.RowActivated += (o, args) => GoToBookmark ();
			return tree;
		}


		public void RedrawContent ()
		{
			UpdateDisplay ();
		}

		public Widget Control {
			get {
				return notebook;
			}
		}

        #endregion

		public void UpdateDisplay ()
		{
			var store = GetActiveStore();
			if (store == null)
				return;
			treeState.Save ();       
			store.Clear ();
			IEnumerable<NumberBookmark> bookmarks = null;
			if (store == localStore)
			{
				var document = Ide.IdeApp.Workbench.ActiveDocument;
				if (document == null)
					return;
				var fileName = document.FileName;
				bookmarks = BookmarkService.Instance.Bookmarks.Where(x => x.BookmarkType == BookmarkType.Local && x.FileName == fileName);
			}
			else 
				if (store == globalStore)
					bookmarks = BookmarkService.Instance.Bookmarks.Where(x => x.BookmarkType == BookmarkType.Global);
				else
					bookmarks = BookmarkService.Instance.Bookmarks;

			foreach (var bookmark in bookmarks.OrderBy(x => x.FileName).ThenBy(x => x.Number)) {
				string iconName = "md-bookmark-" + (bookmark.BookmarkType == BookmarkType.Local ? "l" : "g") + "-" +
					Convert.ToString (bookmark.Number);
				store.AppendValues (iconName, bookmark.FileName, bookmark, Convert.ToString (bookmark.LineNumber), bookmark.LineContent);
			}
			treeState.Load ();
		}

		private void OnBookmarksChanged ()
		{
			UpdateDisplay ();   
		}

		private void OnFileChanged()
		{
			if (notebook.Page == 0)
				UpdateDisplay();
		}

		private PadTreeView GetActiveTree()
		{
			var page = notebook.Page;
			switch (page) {
				case 0:
					return localTree;
				case 1 : 
					return globalTree;
				default:
					return allTree;
			}
		}


		private TreeStore GetActiveStore()
		{
			var page = notebook.Page;
			if (page == -1)
				return null;
			switch (page) {
			case 0:
				return localStore;
			case 1 : 
				return globalStore;
			default:
				return allStore;
			}
		}

		private void ShowPopup (Gdk.EventButton evt)
		{
			var tree = GetActiveTree();
			IdeApp.CommandService.ShowContextMenu (tree, evt, menuSet, tree);
		}

        #region Commands 

		[CommandHandler(PadCommands.GoToBookmark)]
		protected void GoToBookmark ()
		{
			var tree = GetActiveTree();
			var store = GetActiveStore();
			if (store == null)
				return;
			TreeIter iter;
			if (tree.Selection.GetSelected (out iter)) {
				NumberBookmark bookmark = (NumberBookmark)store.GetValue (iter, (int)Columns.Bookmark);
				IdeApp.Workbench.OpenDocument (bookmark.FileName, bookmark.LineNumber, bookmark.Column);
			}
		}

		[CommandHandler(EditCommands.Delete)]
		protected void DeleteBookmark ()
		{
			var tree = GetActiveTree();
			var store = GetActiveStore();
			if (store == null)
				return;
			TreeIter iter;
			if (tree.Selection.GetSelected (out iter)) {
				NumberBookmark bookmark = (NumberBookmark)store.GetValue (iter, (int)Columns.Bookmark);
				BookmarkService.Instance.DeleteBookmark (bookmark);
			}
		}
        
		[CommandUpdateHandler (EditCommands.Delete)]
		[CommandUpdateHandler (PadCommands.GoToBookmark)]
		protected void UpdateBpCommand (CommandInfo cmd)
		{
			var tree = GetActiveTree();
			TreeIter iter;
			cmd.Enabled = tree.Selection.GetSelected (out iter);
		}

        #endregion
	}
}


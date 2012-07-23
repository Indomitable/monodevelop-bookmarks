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
using MonoDevelop.Ide.Gui;
using Gtk;
using MonoDevelop.Components;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide.Gui.Components;
using MonoDevelop.Core;

namespace MonoDevelop.Bookmarks
{
	public class BookmarksPad : IPadContent
	{
		PadTreeView tree;
		TreeStore store;
		ScrolledWindow control;
		TreeViewState treeState;
		CommandEntrySet menuSet;

		enum Columns
		{
			Icon,
			FileName,
			Bookmark,
			LineNumber,
			LineContent
		}

		public BookmarksPad ()
		{
		}

		#region IDisposable implementation
		public void Dispose ()
		{

		}
		#endregion

		#region IPadContent implementation
		public void Initialize (IPadWindow window)
		{
//			ActionCommand gotoCmd = new ActionCommand (LocalCommands.GoToFile, GettextCatalog.GetString ("Go to File"));
//			ActionCommand propertiesCmd = new ActionCommand (LocalCommands.Properties, GettextCatalog.GetString ("Properties"), Gtk.Stock.Properties);	
			menuSet = new CommandEntrySet ();
//			menuSet.Add (gotoCmd);
//			menuSet.AddSeparator ();
//			menuSet.AddItem (DebugCommands.EnableDisableBreakpoint);
//			menuSet.AddItem (DebugCommands.ClearAllBreakpoints);
//			menuSet.AddItem (DebugCommands.DisableAllBreakpoints);
//			menuSet.AddItem (EditCommands.Delete);
//			menuSet.AddSeparator ();
//			menuSet.Add (propertiesCmd);

			
			// The breakpoint list
			store = new TreeStore (typeof(string), typeof(string), typeof(object), typeof(string), typeof(string));
			
			tree = new PadTreeView ();
			tree.Model = store;
			tree.RulesHint = true;
			tree.HeadersVisible = true;
			//tree.DoPopupMenu = ShowPopup;
			
			treeState = new TreeViewState (tree, (int)Columns.Bookmark);
			
			TreeViewColumn col = new TreeViewColumn ();
			CellRenderer crp = new CellRendererIcon ();
			col.PackStart (crp, false);
			col.AddAttribute (crp, "stock_id", (int) Columns.Icon);
			tree.AppendColumn (col);
			
			col = new TreeViewColumn ();
			CellRenderer crt = tree.TextRenderer;
			col.Title = GettextCatalog.GetString ("Name");
			col.PackStart (crt, true);
			col.AddAttribute (crt, "text", (int) Columns.FileName);
			col.Resizable = true;
			col.Expand = true;
			col.Alignment = 0.0f;

			tree.AppendColumn (col);
			col = tree.AppendColumn (GettextCatalog.GetString ("Line Number"), crt, "text", (int) Columns.LineNumber);
			col.Resizable = true;

			col = tree.AppendColumn (GettextCatalog.GetString ("Line Content"), crt, "text", (int) Columns.LineContent);
			col.Resizable = true;
			
			control = new ScrolledWindow ();
			control.ShadowType = ShadowType.None;
			control.Add (tree);
			control.ShowAll ();
			
			UpdateDisplay ();
		}

		public void RedrawContent ()
		{
			UpdateDisplay();
		}

		public Widget Control {
			get {
				return control;
			}
		}
		#endregion

		public void UpdateDisplay ()
		{
			treeState.Save ();		
			store.Clear ();
			treeState.Load ();
		}
	}
}


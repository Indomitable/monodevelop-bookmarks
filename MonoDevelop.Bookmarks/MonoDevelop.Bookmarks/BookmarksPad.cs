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
using MonoDevelop.Ide;
using MonoDevelop.Ide.Commands;
using Mono.TextEditor;

namespace MonoDevelop.Bookmarks
{
    public class BookmarksPad : IPadContent
    {
        PadTreeView tree;
        TreeStore store;
        ScrolledWindow control;
        TreeViewState treeState;
        CommandEntrySet menuSet;
        System.Action onBookmarksChanged;

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

        public void Dispose()
        {
            BookmarkService.Instance.OnBookmarksChange -= onBookmarksChanged;
        }

        #endregion

        #region IPadContent implementation

        public void Initialize(IPadWindow window)
        {
            ActionCommand gotoCmd = new ActionCommand(PadCommands.GoToBookmark, GettextCatalog.GetString("Go to bookmark"));
            
            menuSet = new CommandEntrySet();
            menuSet.Add(gotoCmd);
            menuSet.AddSeparator();
            menuSet.AddItem(EditCommands.Delete);
            //menuSet.AddItem (PadCommands.Clear);
            
            // The breakpoint list
            store = new TreeStore(typeof (string), typeof (string), typeof (object), typeof (string), typeof (string));
            
            tree = new PadTreeView();
            tree.Model = store;
            tree.RulesHint = true;
            tree.HeadersVisible = true;
            tree.DoPopupMenu = ShowPopup;
            
            treeState = new TreeViewState(tree, (int)Columns.Bookmark);
            
            TreeViewColumn col = new TreeViewColumn();
            CellRenderer crp = new CellRendererBookmark();
            col.PackStart(crp, true);
            col.AddAttribute(crp, "stock-id", (int)Columns.Icon);
            tree.AppendColumn(col);
            
            col = new TreeViewColumn();
            CellRenderer crt = tree.TextRenderer;
            col.Title = GettextCatalog.GetString("Name");
            col.PackStart(crt, true);
            col.AddAttribute(crt, "text", (int)Columns.FileName);
            col.Resizable = true;
            //col.Expand = true;
            col.Alignment = 0.0f;

            tree.AppendColumn(col);
            col = tree.AppendColumn(GettextCatalog.GetString("Line Number"), crt, "text", (int)Columns.LineNumber);
            col.Resizable = true;

            col = tree.AppendColumn(GettextCatalog.GetString("Line Content"), crt, "text", (int)Columns.LineContent);
            col.Resizable = true;
            
            control = new ScrolledWindow();
            control.ShadowType = ShadowType.None;
            control.Add(tree);
            control.ShowAll();
            
            UpdateDisplay();

            onBookmarksChanged = DispatchService.GuiDispatch<System.Action>(OnBookmarksChanged);
            BookmarkService.Instance.OnBookmarksChange += onBookmarksChanged;

            tree.RowActivated += (o, args) => GoToBookmark();
        }

        public void RedrawContent()
        {
            UpdateDisplay();
        }

        public Widget Control
        {
            get
            {
                return control;
            }
        }

        #endregion

        public void UpdateDisplay()
        {
            treeState.Save();       
            store.Clear();
            foreach (var bookmark in BookmarkService.Instance.Bookmarks)
            {
                string iconName = "md-bookmark-" + (bookmark.BookmarkType == BookmarkType.Local ? "local" : "global") + "-" +
                    Convert.ToString(bookmark.Number);
                store.AppendValues(iconName, bookmark.FileName, bookmark, Convert.ToString(bookmark.LineNumber), bookmark.LineContent);
            }
            treeState.Load();
        }

        private void OnBookmarksChanged()
        {
            UpdateDisplay();   
        }

        private void ShowPopup(Gdk.EventButton evt)
        {
            IdeApp.CommandService.ShowContextMenu(tree, evt, menuSet, tree);
        }

        #region Commands 

        [CommandHandler(PadCommands.GoToBookmark)]
        protected void GoToBookmark()
        {
            TreeIter iter;
            if (tree.Selection.GetSelected(out iter))
            {
                NumberBookmark bookmark = (NumberBookmark)store.GetValue(iter, (int)Columns.Bookmark);
                IdeApp.Workbench.OpenDocument(bookmark.FileName, bookmark.LineNumber, bookmark.Column);
            }
        }

        [CommandHandler(EditCommands.Delete)]
        protected void DeleteBookmark()
        {
            TreeIter iter;
            if (tree.Selection.GetSelected(out iter))
            {
                NumberBookmark bookmark = (NumberBookmark)store.GetValue(iter, (int)Columns.Bookmark);
                BookmarkService.Instance.DeleteBookmark(bookmark);
            }
        }
        
        [CommandUpdateHandler (EditCommands.Delete)]
        [CommandUpdateHandler (PadCommands.GoToBookmark)]
        protected void UpdateBpCommand(CommandInfo cmd)
        {
            TreeIter iter;
            cmd.Enabled = tree.Selection.GetSelected(out iter);
        }

        #endregion
    }
}


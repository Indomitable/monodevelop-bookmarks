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
using MonoDevelop.Projects.Text;
using MonoDevelop.Ide;

namespace MonoDevelop.Bookmarks
{
	public static class BookmarkService
	{
		static BookmarkCollection bookmarks = new BookmarkCollection();

		static BookmarkService ()
		{
			TextFileService.LineCountChanged += OnLineCountChanged;
			IdeApp.Initialized += delegate {
				IdeApp.Workspace.StoringUserPreferences += OnStoreUserPrefs;
				IdeApp.Workspace.LoadingUserPreferences += OnLoadUserPrefs;
				IdeApp.Workspace.LastWorkspaceItemClosed += OnSolutionClosed;
			};
		}

		static void OnLineCountChanged (object ob, LineCountEventArgs a)
		{
//			foreach (Breakpoint bp in breakpoints.GetBreakpoints ()) {
//				if (bp.FileName == a.TextFile.Name) {
//					if (bp.Line > a.LineNumber) {
//						if (bp.Line + a.LineCount >= a.LineNumber)
//							breakpoints.UpdateBreakpointLine (bp, bp.Line + a.LineCount);
//						else
//							breakpoints.Remove (bp);
//					}
//					else if (bp.Line == a.LineNumber && a.LineCount < 0)
//						breakpoints.Remove (bp);
//				}
//			}
		}
		
		static void OnStoreUserPrefs (object s, UserPreferencesEventArgs args)
		{
//			args.Properties.SetValue ("MonoDevelop.Ide.DebuggingService.Breakpoints", breakpoints.Save ());
//			args.Properties.SetValue ("MonoDevelop.Ide.DebuggingService.PinnedWatches", pinnedWatches);
		}
		
		static void OnLoadUserPrefs (object s, UserPreferencesEventArgs args)
		{
//			XmlElement elem = args.Properties.GetValue<XmlElement> ("MonoDevelop.Ide.DebuggingService.Breakpoints");
//			if (elem == null)
//				elem = args.Properties.GetValue<XmlElement> ("MonoDevelop.Ide.DebuggingService");
//			if (elem != null)
//				breakpoints.Load (elem);
//			PinnedWatchStore wstore = args.Properties.GetValue<PinnedWatchStore> ("MonoDevelop.Ide.DebuggingService.PinnedWatches");
//			if (wstore != null)
//				pinnedWatches.LoadFrom (wstore);
//			pinnedWatches.BindAll (breakpoints);
		}
		
		static void OnSolutionClosed (object s, EventArgs args)
		{
			bookmarks.Clear ();
		}
	}
}


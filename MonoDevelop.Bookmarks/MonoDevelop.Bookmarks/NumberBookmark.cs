using System;
using System.Xml;
using Mono.TextEditor;

namespace MonoDevelop.Bookmarks
{
	public class NumberBookmark
	{
		private const string BookmarkTag = "NumberBookmark";
		private const string FileNameTag = "FileName";
		private const string LineNumberTag = "LineNumber";

		public NumberBookmark ()
		{
			
		}

		public NumberBookmark (MonoDevelop.Core.FilePath fileName, DocumentLine line, int column, int number, BookmarkType bookmarkType)
		{
			FileName = fileName;
			Line = line;
			Column = column; 
			Number = number;
			BookmarkType = bookmarkType;
		}

		public int Number { get; set; }

		public BookmarkType BookmarkType { get; set; }

		public string FileName { get; set; }

		public DocumentLine Line { get; set; }

		public int Column { get; set; }

		public string LineContent { get; set; }

		public int LineNumber { get; set; }

		internal virtual XmlElement ToXml (XmlDocument doc)
		{
			XmlElement elem = doc.CreateElement (BookmarkTag);
			elem.SetAttribute (FileNameTag, FileName);
			elem.SetAttribute (LineNumberTag, Convert.ToString (LineNumber));
			return elem;
		}
		
		internal static NumberBookmark FromXml (XmlElement elem)
		{
			if (string.Equals (elem.Name, BookmarkTag, StringComparison.OrdinalIgnoreCase)) {
				NumberBookmark bookMark = new NumberBookmark ();
				bookMark.FileName = elem.GetAttribute (FileNameTag);
				if (string.IsNullOrEmpty (bookMark.FileName))
					return null;
				var lineNumber = Parser.ParseInt(elem.GetAttribute(LineNumberTag));
				if (lineNumber.HasValue)
					bookMark.LineNumber = lineNumber.Value;
				else
					return null;
				return bookMark;
			}
			return null;
		}
	}
}


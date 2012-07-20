//
// Parser.cs
//
// Author:
//       Ventsislav Mladenov <ventsislav.mladenov@gmail.com>
//
// Copyright (c) 2012 Ventsislav Mladenov
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
using System.Globalization;
using System.Text.RegularExpressions;

namespace MonoDevelop.Bookmarks
{
	public class Parser
	{
		#region Inner Paser
		
		private class InnerParser<T>
			where T : struct
		{
			internal delegate bool DParseNumberString (string value,NumberStyles style,IFormatProvider formatProvider,out T output);
			
			internal T? ParseNumberString (string value, DParseNumberString parseType, NumberStyles style, IFormatProvider formatProvider)
			{
				T result;
				T? resultNullable;
				if (parseType (value, style, formatProvider, out result)) {
					resultNullable = result;
				} else {
					resultNullable = null;
				}
				return resultNullable;
			}
			
			internal delegate bool DParseDateString (string value,IFormatProvider formatProvider,DateTimeStyles style,out T output);
			
			internal T? ParseDateString (string value, DParseDateString parseType, DateTimeStyles style, IFormatProvider formatProvider)
			{
				T result;
				T? resultNullable;
				if (parseType (value, formatProvider, style, out result)) {
					resultNullable = result;
				} else {
					resultNullable = null;
				}
				return resultNullable;
			}
			
			internal delegate T DParseObject (object value);
			
			internal T? ParseObject (object value, DParseObject parseObject)
			{
				T? resultNullable;
				if (value == null) {
					return null;
				}
				try {
					resultNullable = parseObject (value);
				} catch (Exception) {
					resultNullable = null;
				}
				return resultNullable;
			}
		}
		
		#endregion

		#region Parse Double
		
		public static double? ParseDouble (string value)
		{
			var parser = new InnerParser<double>();
			return parser.ParseNumberString (value, double.TryParse, NumberStyles.Float | NumberStyles.AllowThousands, NumberFormatInfo.CurrentInfo);
		}
		
		public static double? ParseDouble (object value)
		{
			var parser = new InnerParser<double>();
			return parser.ParseObject (value, Convert.ToDouble);
		}
		
		#endregion
		
		#region Parse Decimal
		
		public static decimal? ParseDecimal (string value)
		{
			var parser = new InnerParser<decimal>();
			return parser.ParseNumberString (value, decimal.TryParse, NumberStyles.Float | NumberStyles.AllowThousands, NumberFormatInfo.CurrentInfo);
		}
		
		public static decimal? ParseDecimal (object value)
		{
			var parser = new InnerParser<decimal>();
			return parser.ParseObject (value, Convert.ToDecimal);
		}
		
		#endregion
		
		#region Parse Int16
		
		public static short? ParseShort (string value)
		{
			var parser = new InnerParser<short>();
			return parser.ParseNumberString (value, short.TryParse, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
		}

		public static short? ParseShort (object value)
		{
			var parser = new InnerParser<short>();
			return parser.ParseObject (value, Convert.ToInt16);
		}
		
		#endregion

		#region Parse Int32
		
		public static int? ParseInt (string value)
		{
			var parser = new InnerParser<int> ();
			return parser.ParseNumberString (value, int.TryParse, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
		}
		
		public static int? ParseInt (object value)
		{
			var parser = new InnerParser<int> ();
			return parser.ParseObject (value, Convert.ToInt32);
		}
		
		#endregion

		#region Parse Int64
		
		public static long? ParseLong (string value)
		{
			var parser = new InnerParser<long>();
			return parser.ParseNumberString (value, long.TryParse, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
		}
		
		public static long? ParseLong (object value)
		{
			var parser = new InnerParser<long>();
			return parser.ParseObject (value, Convert.ToInt64);
		}
		
		#endregion
		
	}
}


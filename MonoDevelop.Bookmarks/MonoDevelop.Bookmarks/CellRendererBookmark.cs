//
// CellRendererBookmark.cs
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
using Gtk;
using MonoDevelop.Ide;
using Mono.TextEditor.Highlighting;

namespace MonoDevelop.Bookmarks
{
    public class CellRendererBookmark : CellRendererPixbuf
    {
        public CellRendererBookmark()
        {
            this.Width = 14;
        }

        protected override void Render(Gdk.Drawable window, Widget widget, Gdk.Rectangle background_area, Gdk.Rectangle cell_area, Gdk.Rectangle expose_area, CellRendererState flags)
        {
            using (var context = Gdk.CairoHelper.Create(window))
            {
                int x = 10;
                int y = cell_area.Top + 4;
                int r = 8;
                int width = 14;
                int height = 12;// 12;

                DrawRoundRectangle(context, x, y, r, width, height);
                Cairo.Color color1 = ColorScheme.ToCairoColor(new Gdk.Color(255, 255, 255));
                Cairo.Color color2 = ColorScheme.ToCairoColor(new Gdk.Color(105, 156, 235));

                using (var pat = new Cairo.LinearGradient (x + width / 4, y, x + width / 2, y + height - 4))
                {
                    pat.AddColorStop(0, color1);
                    pat.AddColorStop(1, color2);
                    context.Pattern = pat;
                    context.FillPreserve();
                }
            
                using (var pat = new Cairo.LinearGradient (x, y + height, x + width, y))
                {
                    pat.AddColorStop(0, color2);
                    context.Pattern = pat;
                    context.Stroke();
                }

                var bookmarkCode = this.StockId;
                var bookmarkNumber = bookmarkCode.Substring(bookmarkCode.LastIndexOf("-") + 1);

                context.Color = new Cairo.Color(0, 0, 0);
                context.SelectFontFace(DesktopService.DefaultMonospaceFont, Cairo.FontSlant.Normal, Cairo.FontWeight.Bold); 
                context.SetFontSize(12);
                var te = context.TextExtents(bookmarkNumber);
                context.MoveTo(x + 5, y + 1 + te.Height);
                context.ShowText(bookmarkNumber);
            }
        }

        public static void DrawRoundRectangle(Cairo.Context cr, double x, double y, double r, double w, double h)
        {
            const double ARC_TO_BEZIER = 0.55228475;
            double radius_x = r;
            double radius_y = r / 4;
            
            if (radius_x > w - radius_x)
                radius_x = w / 2;
            
            if (radius_y > h - radius_y)
                radius_y = h / 2;
            
            double c1 = ARC_TO_BEZIER * radius_x;
            double c2 = ARC_TO_BEZIER * radius_y;
            
            cr.NewPath();
            cr.MoveTo(x + radius_x, y);

            cr.RelLineTo(w - 2 * radius_x, 0.0);
            cr.RelCurveTo(c1, 0.0, radius_x, c2, radius_x, radius_y);

            cr.RelLineTo(0, h - 2 * radius_y);
            cr.RelCurveTo(0.0, c2, c1 - radius_x, radius_y, -radius_x, radius_y);

            cr.RelLineTo(-w + 2 * radius_x, 0);
            cr.RelCurveTo(-c1, 0, -radius_x, -c2, -radius_x, -radius_y);

            cr.RelLineTo(0, -h + 2 * radius_y);
            cr.RelCurveTo(0.0, -c2, radius_x - c1, -radius_y, radius_x, -radius_y);

            cr.ClosePath();
        }
    }
}


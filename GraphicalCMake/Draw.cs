using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;

namespace GraphicalCMake
{
    public class Draw
    {
        #region Line
        public static Line Line(double x1, double y1, double x2, double y2, Brush color)
        {
            var line = new Line();
            line.X1 = x1;
            line.X2 = x2;
            line.Y1 = y1;
            line.Y2 = y2;
            line.Stroke = color;
            return line;
        }
        #endregion

        #region Rectangle
        public static Rectangle Rectangle(double x, double y, double w, double h, Brush border)
        {
            return Rectangle(x, y, w, h, border, Brushes.Transparent);
        }

        public static Rectangle Rectangle(double x, double y, double w, double h, Brush border, Brush fill)
        {
            var rect = new Rectangle();
            rect.Width = w;
            rect.Height = h;
            rect.Stroke = border;
            rect.Fill = fill;
            rect.IsHitTestVisible = false;
            Draw.Transform.Bind(rect);
            var tt = Draw.Transform.GetTranslateTransform(rect);
            tt.X = x;
            tt.Y = y;
            return rect;
        }

        public static Rectangle Rectangle(Point pt, Size sz, Brush border) { return Rectangle(pt.X, pt.Y, sz.Width, sz.Height, border); }
        public static Rectangle Rectangle(Point pt, Size sz, Brush border, Brush fill) { return Rectangle(pt.X, pt.Y, sz.Width, sz.Height, border, fill); }
        #endregion

        public static class Transform
        {
            public static TranslateTransform GetTranslateTransform(UIElement element)
            {
                return (TranslateTransform)((TransformGroup)element.RenderTransform)
                  .Children.First(tr => tr is TranslateTransform);
            }

            public static ScaleTransform GetScaleTransform(UIElement element)
            {
                return (ScaleTransform)((TransformGroup)element.RenderTransform)
                  .Children.First(tr => tr is ScaleTransform);
            }

            public static void Bind(UIElement element)
            {
                TransformGroup group = new TransformGroup();
                ScaleTransform st = new ScaleTransform();
                group.Children.Add(st);
                TranslateTransform tt = new TranslateTransform();
                group.Children.Add(tt);
                element.RenderTransform = group;
                element.RenderTransformOrigin = new Point(0.0, 0.0);
            }
        }

        #region getTextBlockSize
        public static Size getTextBlockSize(TextBlock tb)
        {
            tb.Measure(new Size(0, 0));
            tb.Arrange(new Rect());
            return new Size(tb.ActualWidth, tb.ActualHeight);
        }
        public static Size getTextBlockSize(out TextBlock tb, string text, double fontSize)
        {
            tb = new TextBlock();
            tb.Text = text;
            tb.FontSize = fontSize;
            return getTextBlockSize(tb);
        }
        public static Size getTextBlockSize(string text, double fontSize)
        {
            TextBlock tb;
            return getTextBlockSize(out tb, text, fontSize);
        }
        #endregion

    }

}

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicalCMake
{

    /* http://stackoverflow.com/questions/741956/pan-zoom-image */
    /* by Wiesław Šoltés */
    /* edited by Konrad Viltersten */

    public partial class CanvasViewBorder : Border
    {
        private CMakeArchRenderableTargetCanvas child = null;
        private Point origin;
        private Point start;

        public override UIElement Child
        {
            get { return base.Child; }
            set
            {
                if (value != null && value != this.Child)
                    this.Initialize(value as CMakeArchRenderableTargetCanvas);
                base.Child = value;
            }
        }

        public void Initialize(CMakeArchRenderableTargetCanvas element)
        {
            this.child = element;
            if (child != null)
            {
                Draw.Transform.Bind(child);
                this.MouseWheel += this_MouseWheel;
                this.MouseLeftButtonDown += this_MouseLeftButtonDown;
                this.MouseLeftButtonUp += this_MouseLeftButtonUp;
                this.MouseRightButtonDown += this_MouseRightButtonDown;
                this.MouseRightButtonUp += this_MouseRightButtonUp;

                this.DragOver += this_DragOver;
                this.MouseMove += this_MouseMove;

                child.AllowDrop = true;
                child.Background = Brushes.Transparent;
                AllowDrop = true;
                Drop += CanvasViewBorder_Drop;
            }
        }

        private void this_DragOver(object sender, DragEventArgs e)
        {
            if (leftSelected == null)
            {
                if (curMouseMoveSel != null)
                    child.setRenderOption(curMouseMoveSel, RenderOption.OnSelection, new Tuple<object, object>(false, null));
                detCursorSelection(child, e.GetPosition(child), out curMouseMoveSel);
                if (curMouseMoveSel != null && child.isRenderOptionAvailable(curMouseMoveSel, RenderOption.OnSelection))
                {
                    child.setRenderOption(curMouseMoveSel, RenderOption.OnSelection, new Tuple<object, object>(true, null));
                }
            }
        }

        private void CanvasViewBorder_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(string)))
            {
                string dat = e.Data.GetData(typeof(string)) as string;
                MessageBox.Show(dat);
                if (System.IO.File.Exists(dat))
                {
                    CMakeFile cf = new CMakeFile(new System.IO.FileInfo(dat));
                    cf.Load();
                    child.Add(cf);
                    Canvas.SetLeft(cf.getRenderedResult(), e.GetPosition(child).X);
                    Canvas.SetTop(cf.getRenderedResult(), e.GetPosition(child).Y);
                    BringToFront(child, cf.getRenderedResult());
                }
                else if (System.IO.Directory.Exists(dat))
                {
                    CMakeDirectory cd = new CMakeDirectory(new System.IO.DirectoryInfo(dat));
                    cd.Load();
                    child.Add(cd);
                    Canvas.SetLeft(cd.getRenderedResult(), e.GetPosition(child).X);
                    Canvas.SetTop(cd.getRenderedResult(), e.GetPosition(child).Y);
                    BringToFront(child, cd.getRenderedResult());
                }
            }
        }

        public void ResetView()
        {
            if (child != null)
            {
                // reset zoom
                var st = Draw.Transform.GetScaleTransform(child);
                st.ScaleX = 1.0;
                st.ScaleY = 1.0;

                // reset pan
                var tt = Draw.Transform.GetTranslateTransform(child);
                tt.X = 0.0;
                tt.Y = 0.0;
            }
        }

        #region Child Events
        private void this_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (child != null && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                var st = Draw.Transform.GetScaleTransform(child);
                var tt = Draw.Transform.GetTranslateTransform(child);

                double zoom = e.Delta > 0 ? .2 : -.2;
                if (!(e.Delta > 0) && (st.ScaleX < .4 || st.ScaleY < .4))
                    return;

                Point relative = e.GetPosition(child);
                double abosuluteX;
                double abosuluteY;

                abosuluteX = relative.X * st.ScaleX + tt.X;
                abosuluteY = relative.Y * st.ScaleY + tt.Y;

                st.ScaleX += zoom;
                st.ScaleY += zoom;

                tt.X = abosuluteX - relative.X * st.ScaleX;
                tt.Y = abosuluteY - relative.Y * st.ScaleY;
            }
        }

        enum MouseCaptureOwner
        {
            NONE, LEFT, RIGHT
        }
        MouseCaptureOwner mco = MouseCaptureOwner.NONE;

        #region RightButton Events
        private void this_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (child != null && mco == MouseCaptureOwner.NONE)
            {
                var tt = Draw.Transform.GetTranslateTransform(child);
                start = e.GetPosition(this);
                origin = new Point(tt.X, tt.Y);
                this.Cursor = Cursors.Hand;
                this.CaptureMouse();
                mco = MouseCaptureOwner.RIGHT;
            }
        }

        private void this_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (child != null)
            {
                this.ReleaseMouseCapture();
                this.Cursor = Cursors.Arrow;
                mco = MouseCaptureOwner.NONE;
            }
            
        }

        private void this_MouseRightButtonMove(object sender, MouseEventArgs e)
        {
            if (child != null)
            {
                var tt = Draw.Transform.GetTranslateTransform(child);
                Vector v = start - e.GetPosition(this);
                tt.X = origin.X - v.X;
                tt.Y = origin.Y - v.Y;
            }
        }

        #endregion

        private static void BringToFront(Canvas rel, UIElement toFront)
        {
            if (!rel.Children.Contains(toFront)) throw new ArgumentOutOfRangeException();
            int max = int.MinValue;
            foreach (UIElement ue in rel.Children)
            {
                if (ue != toFront)  max = Math.Max(max, Panel.GetZIndex(ue));
            }
            Panel.SetZIndex(toFront, max + 1);
        }

        public static bool detCursorSelection(Canvas rel, Point relPos, out UIElement result)
        {
            var list = new System.Collections.Generic.SortedList<int, UIElement>();
            foreach (UIElement ue in rel.Children)
            {
                var ta = ue.TransformToAncestor(rel);
                var p = ta.Transform(new Point(0, 0));
                var r = new Rect(p, ue.RenderSize);

                if (r.Contains(relPos))
                    try { list.Add(Canvas.GetZIndex(ue), ue); }
                    catch (Exception) { }
            }
            if (list.Count != 0)
            {
                var pr = list.Keys.Max();
                result = list[pr];
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }

        public static bool detCursorSelection(Canvas rel, MouseEventArgs e, out UIElement result)
        {
            return detCursorSelection(rel, e.GetPosition(rel), out result);
        }

        #region LeftButton

        private UIElement leftSelected = null;
        private void this_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (child != null && mco == MouseCaptureOwner.NONE)
            {
                this.CaptureMouse();
                mco = MouseCaptureOwner.LEFT;
                start = e.GetPosition(child);

                detCursorSelection(child, e, out leftSelected);
                if (leftSelected != null && child.isRenderOptionAvailable(leftSelected, RenderOption.OnSelection))
                {
                    child.setRenderOption(leftSelected, RenderOption.OnSelection, new Tuple<object, object>(true, null));
                    BringToFront(child, leftSelected);
                }
            }
            if (child != null && mco == MouseCaptureOwner.LEFT && e.ClickCount == 2 && child.targets.ContainsKey(leftSelected))
            {
                //MessageBox.Show(child.targets[leftSelected].ToString());
                MainWindow.StatusLabel.Content = child.targets[leftSelected].ToStatusString();
                if (child.targets[leftSelected] is CMakeDirectory) MainWindow.lastSelectedCD = (CMakeDirectory)child.targets[leftSelected];
                DetailWindow dw = new DetailWindow();
                dw.DetailTb.Text = child.targets[leftSelected].ToStatusString();
                dw.Title = child.targets[leftSelected].ToString();
                dw.Show();
                this_MouseLeftButtonUp(sender, e);
            }
        }

        private void this_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (child != null)
            {
                this.ReleaseMouseCapture();
                mco = MouseCaptureOwner.NONE;
                if (leftSelected != null && leftSelected != curMouseMoveSel && child.isRenderOptionAvailable(leftSelected, RenderOption.OnSelection))
                {
                    child.setRenderOption(leftSelected, RenderOption.OnSelection, new Tuple<object, object>(false, null));
                }
                leftSelected = null;
            }
        }


        private void this_MouseLeftButtonMove(object sender, MouseEventArgs e)
        {
            if (child != null && leftSelected != null)
            {
                Vector v = start - e.GetPosition(child);
                start = e.GetPosition(child);

                var ta = leftSelected.TransformToAncestor(child);
                var p = ta.Transform(new Point(0, 0));
                var t = new TranslateTransform();

                t.X = p.X - v.X;
                t.Y = p.Y - v.Y;

                Canvas.SetLeft(leftSelected, t.X);
                Canvas.SetTop(leftSelected, t.Y);
                //leftSelected.RenderTransform = t;
            }
        }

        #endregion
        UIElement curMouseMoveSel = null;
        private void this_MouseMove(object sender, MouseEventArgs e)
        {
            switch (mco)
            {
                case MouseCaptureOwner.NONE:
                    break;
                case MouseCaptureOwner.LEFT:
                    this_MouseLeftButtonMove(sender, e);
                    break;
                case MouseCaptureOwner.RIGHT:
                    this_MouseRightButtonMove(sender, e);
                    break;
                default:
                    throw new System.Exception();
            }

            if (leftSelected == null)
            {
                if (curMouseMoveSel != null)
                    child.setRenderOption(curMouseMoveSel, RenderOption.OnSelection, new Tuple<object, object>(false, null));
                detCursorSelection(child, e, out curMouseMoveSel);
                if (curMouseMoveSel != null && child.isRenderOptionAvailable(curMouseMoveSel, RenderOption.OnSelection))
                {
                    child.setRenderOption(curMouseMoveSel, RenderOption.OnSelection, new Tuple<object, object>(true, null));
                }
            }
        }

        #endregion

    }


    public class KeyboardActivatedEvent
    {
        public Key key { get; private set; }
        public bool isActivated { get; private set; }

        public KeyboardActivatedEvent(Key key)
        {
            this.key = key;
        }

        public bool Try()
        {
            if (Keyboard.IsKeyDown(key))
            {
                isActivated = true;
                return true;
            }
            return isActivated;
        }

        public void End()
        {
            isActivated = false;
        }
    }


}


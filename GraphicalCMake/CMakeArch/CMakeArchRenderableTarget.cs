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
    public enum RenderOption    /* Tuple<(object)option-specific-indicator, (object)option-specific-class> */
    {
        OnSelection,    /* bool, curUIElement */
    }

    public interface CMakeArchRenderableTarget
    {
        void RenderOptionChanged(RenderOption option, Tuple<object, object> original, Tuple<object, object> current);   /* Modifying Board According to the new status */
        void Load();    /* RePainting */
        UIElement getRenderedResult();  /* Get Border */

        #region RenderOptions
        /* 
        RenderOption is an interface for outer-space access to target-dependent rendering requirements.  
        
        isRenderOptionAvailable checks if a desired access to a rendering requirement is available
        setRenderOption/getRenderOption sets/gets the value of a specific rendering option of that target IF EXISTS.  
            -> Returns the value of the previous rendering option (can be null)
            -> Returns null if the specified option does not exist

        */
        bool isRenderOptionAvailable(RenderOption option);
        Tuple<object, object> setRenderOption(RenderOption option, Tuple<object, object> value);
        Tuple<object, object> getRenderOption(RenderOption option);

        #endregion

        string ToStatusString();
    }

    public abstract class CMakeArchRenderableTargetBase : CMakeArchRenderableTarget
    {
        public Border border = null;
        public Canvas canvas = null;
        public UIElement getRenderedResult() { return border; }

        public CMakeArchRenderableTargetBase()
        {
            border = new Border(); canvas = new Canvas();
            renderoptions = new Dictionary<RenderOption, Tuple<object, object>>();
            isLoaded = false;
        }

        #region Rendering
        protected Dictionary<RenderOption, Tuple<object, object>> renderoptions;
        public virtual bool isLoaded { get; protected set; }
        public virtual void Load()  // refreshing all renderoptions and set isLoaded to true
        {
            /* Validate all initialized render options (call render option changed for every single render option) */
            if (renderoptions != null)
                foreach (RenderOption ro in renderoptions.Keys.ToList())    /* since render option's data might be changed */
                    setRenderOption(ro, getRenderOption(ro));

            /* Produce actual size of the canvas */
            canvas.Measure(new Size(0, 0));
            canvas.Arrange(new Rect());

            /* End Loading */
            isLoaded = true;
        }

        public virtual void RenderOptionChanged(RenderOption option, Tuple<object, object> original, Tuple<object, object> current)
        {
            DefRenderOptionChanged(option, original, current);
        }
        protected virtual void DefRenderOptionChanged(RenderOption option, Tuple<object, object> original, Tuple<object, object> current)
        {
            switch (option)
            {
                case RenderOption.OnSelection:
                    this.CommonRenderOptionOnSelectionBehaviour(canvas, option, original, ref current);
                    break;
            }
        }

        #endregion

        #region RenderOption
        public bool isRenderOptionAvailable(RenderOption option)
        {
            return renderoptions.ContainsKey(option);
        }

        public Tuple<object, object> setRenderOption(RenderOption option, Tuple<object, object> value)
        {
            if (isRenderOptionAvailable(option))
            {
                Tuple<object, object> r = renderoptions[option];
                renderoptions[option] = value;
                RenderOptionChanged(option, r, value);
                return r;
            }
            else
            {
                return null;
            }
        }

        public Tuple<object, object> getRenderOption(RenderOption option)
        {
            Tuple<object, object> result;
            renderoptions.TryGetValue(option, out result);
            return result;
        }
        #endregion

        public virtual string ToStatusString()
        {
            return ToString();
        }
    }

    public static class CMakeArchRenderableTargetExtension
    {
        public static void CommonRenderOptionOnSelectionBehaviour<T>(this T o, Canvas canvas, RenderOption option, Tuple<object, object> original, ref Tuple<object, object> current) where T : CMakeArchRenderableTarget
        {
            if (original != null && original.Item2 != null) canvas.Children.Remove(original.Item2 as UIElement);
            if (current != null && current.Item1 != null)
            {
                current = new Tuple<object, object>(current.Item1, Draw.Rectangle(0, 0, canvas.Width, canvas.Height, (bool)current.Item1 ? Brushes.Orange : Brushes.Black));
                canvas.Children.Add(current.Item2 as UIElement);
            }
        }

        public static void CommonTargetedBoardInitialization<T>(this T o, int width, int height, Border border, Canvas canvas) where T : CMakeArchRenderableTarget
        {
            canvas.BeginInit();
            canvas.Width = width;
            canvas.Height = height;
            canvas.Margin = new Thickness(0, 0, 0, 0);
            canvas.AllowDrop = true;
            canvas.Background = Brushes.Transparent;
            canvas.EndInit();

            border.BeginInit();
            border.Width = width;
            border.Height = height;
            border.ClipToBounds = true;
            border.Child = canvas;
            Draw.Transform.Bind(border);
            border.AllowDrop = true;
            border.Background = Brushes.Transparent;
            border.EndInit();
        }

        public static bool detCursorSelection<T>(this T o, Canvas rel, Point relPos, out UIElement result) where T : CMakeArchRenderableTarget
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

        public static bool detCursorSelection<T>(this T o, Canvas rel, MouseEventArgs e, out UIElement result) where T : CMakeArchRenderableTarget
        {
            return detCursorSelection(o, rel, e.GetPosition(rel), out result);
        }
    }

    public class CMakeArchRenderableTargetCanvas : Canvas
    {
        public Dictionary<UIElement, CMakeArchRenderableTarget> targets = new Dictionary<UIElement, CMakeArchRenderableTarget>();
        public void Add(CMakeArchRenderableTarget target)
        {
            UIElement ue = target.getRenderedResult();
            targets.Add(ue, target);
            Children.Add(ue);
        }

        public bool isRenderOptionAvailable(UIElement ue, RenderOption option)
        {
            if (targets.ContainsKey(ue)) return targets[ue].isRenderOptionAvailable(option);
            return false;
        }

        public Tuple<object, object> getRenderOption(UIElement ue, RenderOption option)
        {
            if (!isRenderOptionAvailable(ue, option)) return null;
            return targets[ue].getRenderOption(option);
        }

        public Tuple<object, object> setRenderOption(UIElement ue, RenderOption option, Tuple<object, object> value)
        {
            if (!isRenderOptionAvailable(ue, option)) return null;
            return targets[ue].setRenderOption(option, value);
        }
    }


}

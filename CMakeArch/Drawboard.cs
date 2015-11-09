using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Drawing.Drawing2D;

namespace GraphicalCMake
{
    /* Drawboard uses vector-style elements */

    public interface DrawboardElement
    {
        Bitmap Render();
        double scale { get; set; }
        Rectangle area { get; set; }
    }

    public class DrawBoardCMakeElement : DrawboardElement
    {
        public DrawBoardCMakeElement(Rectangle rect)
        {
            scale = 1;
            area = rect;
        }

        public Rectangle area { get; set; }
        public double scale { get; set; }

        public virtual Bitmap Render()
        {
            Bitmap img = new Bitmap(area.Width, area.Height);
            Graphics g = Graphics.FromImage(img);
            g.Clear(Color.White);
            g.DrawLine(SystemPens.ActiveBorder, new Point(0, 0), new Point(area.Width, area.Height));
            g.Dispose();
            return img;
        }
    }

    public class Drawboard
    {
        public static Bitmap scaleBitmap(Bitmap src, double scale)
        {
            try
            {
                Bitmap b = new Bitmap((int)(src.Width * scale), (int)(src.Height * scale));
                Graphics g = Graphics.FromImage(b);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(src, new Rectangle(0, 0, b.Width, b.Height),
                    new Rectangle(0, 0, src.Width, src.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch
            {
                return null;
            }
        }
        protected Bitmap board;
        protected Graphics graphics;
        public int Width { get { return board.Width; } }
        public int Height { get { return board.Height; } }
        public Drawboard(int width, int height)
        {
            board = new Bitmap(width, height);
            graphics = Graphics.FromImage(board);
        }

        public Drawboard(Size size) : this(size.Width, size.Height) { }

        public LinkedList<DrawboardElement> elements = new LinkedList<DrawboardElement>();

        public virtual Bitmap Render()
        {
            graphics.Clear(Color.White);
            foreach(DrawboardElement element in elements)
            {
                graphics.DrawImage(scaleBitmap(element.Render(), element.scale), element.area.Location);
            }
            return board;
        }
    }

    public class World : Drawboard
    {
        public World(int width, int height) : base(width, height) { projectionScaler = 1; }
        public World(Size size) : base(size) { projectionScaler = 1; }
        public World(int width, int height, Rectangle curProj) : base(width, height) { this.curProj = curProj; projectionScaler = 1; }
        public World(Size size, Rectangle curProj) : base(size) { this.curProj = curProj; projectionScaler = 1; }

        // only render what can be seen
        public override Bitmap Render()
        {
            Rectangle finalProj = curProj;
            finalProj.Width = (int)(finalProj.Width * projectionScaler);
            finalProj.Height = (int)(finalProj.Height * projectionScaler);
            normalizeProj(ref finalProj);

            graphics.Clear(Color.Aqua);
            foreach (DrawboardElement element in elements)
            {
                if (finalProj.IntersectsWith(element.area))
                    graphics.DrawImage(scaleBitmap(element.Render(), element.scale), element.area.Location);
            }

            Bitmap cropped = new Bitmap(finalProj.Size.Width, finalProj.Size.Height);

            using (Graphics g = Graphics.FromImage(cropped))
                g.DrawImage(board, new Rectangle(0, 0, finalProj.Size.Width, finalProj.Size.Height), finalProj, GraphicsUnit.Pixel);

            return cropped;
        }

        public virtual void resize(Size NewSize) { resize(NewSize.Width, NewSize.Height); }
        public virtual void resize(Size NewSize, Rectangle croppingArea, Point pasteLocation) { resize(NewSize.Width, NewSize.Height, croppingArea, pasteLocation); }
        public virtual void resize(int NewWidth, int NewHeight) { resize(NewWidth, NewHeight, new Rectangle(0, 0, NewWidth, NewHeight), new Point(0, 0)); }
        public virtual void resize(int NewWidth, int NewHeight, Rectangle croppingArea, Point pasteLocation)
        {
            Bitmap newBmp = new Bitmap(NewWidth, NewHeight);
            Graphics graphics = Graphics.FromImage(newBmp);
            Rectangle newRect = new Rectangle(0, 0, NewWidth, NewHeight);
            Rectangle pasteRect = new Rectangle(pasteLocation, croppingArea.Size);
            graphics.DrawImage(board, pasteRect, croppingArea, GraphicsUnit.Pixel);

            board.Dispose();
            board = newBmp;
            this.graphics.Dispose();
            this.graphics = graphics;
            curProj = new Rectangle();
        }

        // The Projection of the board to be seen
        public Rectangle curProj = new Rectangle();
        public double projectionScaler { get; set; }
        protected void normalizeProj(ref Rectangle curProj)
        {
            if (curProj.X < 0) curProj.X = 0;
            if (curProj.Y < 0) curProj.Y = 0;

            if (curProj.X + curProj.Width > board.Width) curProj.X = board.Width - curProj.Width;
            if (curProj.Y + curProj.Height > board.Height) curProj.Y = board.Height - curProj.Height;

            if (curProj.X < 0 || curProj.Y < 0)
            {
                curProj.X = 0;
                curProj.Y = 0;
                curProj.Width = board.Width;
                curProj.Height = board.Height;
            }

            if (curProj.Width <= 0 || curProj.Height <= 0)
            {
                throw new Exception("The curProj.Size after all transformations is invalid");
            }
        }
    }
}


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
    public class CMakeDirectory : CMakeArchRenderableTargetBase, CMakeArchRenderableTarget
    {
        public CMakeArch.CMakeDirectory cdirectory;

        public const int width = 160;
        public const int height = 90;

        public Connector conn_subdirectories;
        public CMakeDirectory(DirectoryInfo thisDirectory)
        {
            cdirectory = new CMakeArch.CMakeDirectory(thisDirectory);
            renderoptions.Add(RenderOption.OnSelection, new Tuple<object, object>(false, null));
            this.CommonTargetedBoardInitialization(width, height, border, canvas);
            isProject = false;
        }
        
        private void init_conn(out Connector conn, string text)
        {
            conn = new Connector(this);
            canvas.Children.Add(conn);
            conn.Content = text;
            //var szTb = Draw.getTextBlockSize(text, conn.FontSize);
            //conn.Width = szTb.Width+5;
            //conn.Height = szTb.Height+5;
            //conn.Measure(new Size(0, 0));
            //conn.Arrange(new Rect());
            conn.DroppedAsSource += Conn_DroppedAsSource;
            conn.DroppedAsTarget += Conn_DroppedAsTarget;
            conn.Clicked += Conn_Clicked;
        }

        public override void Load()
        {
            canvas.Children.Clear();
            base.Load();

            TextBlock tb = new TextBlock();
            tb.Text = cdirectory.directory.Name;
            tb.FontSize = 20;
            canvas.Children.Add(tb);

            init_conn(out conn_subdirectories, "SubDirectories");
            Canvas.SetLeft(conn_subdirectories, 0);
            Canvas.SetBottom(conn_subdirectories, 0);
        }

        private void Conn_Clicked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Clicked");
        }

        private void Conn_DroppedAsTarget(object sender, RoutedEventArgs e)
        {
            if (!(sender is Connector)) return;
            var conn = sender as Connector;

            if (conn == conn_subdirectories)
            {
                if ((e.OriginalSource as Connector).info.owner is CMakeDirectory)
                {
                    cdirectory.subdirectories.Add(((e.OriginalSource as Connector).info.owner as CMakeDirectory).cdirectory);
                    //MessageBox.Show(cdirectory.subdirectories.Count.ToString());
                    MessageBox.Show("MessageBox from Target " + conn.GetHashCode() + "! EventSource(SourceConnector) is " + e.OriginalSource.GetHashCode());
                }
            }
        }

        private void Conn_DroppedAsSource(object sender, RoutedEventArgs e)
        {
            if (!(sender is Connector)) return;
            var conn = sender as Connector;

            if (conn == conn_subdirectories)
                MessageBox.Show("MessageBox from Source " + conn.GetHashCode() + "! EventSource(TargetConnector) is " + e.OriginalSource.GetHashCode());
        }

        //public override void RenderOptionChanged(RenderOption option, Tuple<object, object> original, Tuple<object, object> current)
        //{
        //    switch (option)
        //    {
        //        default:
        //            DefRenderOptionChanged(option, original, current);
        //    }

        //}
        public override string ToStatusString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("CMakeDirectory at ");
            sb.Append(cdirectory.directory.FullName);
            sb.Append("\t Target(s) Count: ");
            sb.Append(cdirectory.targets.Count);

            return sb.ToString();
        }

        public bool isProject { get; set; }
    }

    public class CMakeTarget : CMakeArchRenderableTargetBase, CMakeArchRenderableTarget
    {
        public CMakeArch.CMakeTarget ctarget;

        public const int width = 160;
        public const int height = 90;

        public CMakeTarget(CMakeArch.CMakeTarget.TargetType type, bool isImported)
        {
            ctarget = new CMakeArch.CMakeTarget(type, isImported);
            renderoptions.Add(RenderOption.OnSelection, new Tuple<object, object>(false, null));
            this.CommonTargetedBoardInitialization(width, height, border, canvas);
        }


        public override void Load()
        {
            canvas.Children.Clear();
            base.Load();
        }

        public override string ToStatusString()
        {
            return ToString();
        }
    }

    public class CMakeFile : CMakeArchRenderableTargetBase, CMakeArchRenderableTarget
    {
        public string Name;
        protected FileInfo file;

        private TextBlock tb;
        public CMakeFile(FileInfo f)
        {
            Name = f.Name;
            file = f;

            var sztb = Draw.getTextBlockSize(out tb, Name, 20);

            this.CommonTargetedBoardInitialization((int)sztb.Width, (int)sztb.Height, border, canvas);

            renderoptions.Add(RenderOption.OnSelection, new Tuple<object, object>(false, null));
        }

        public override void Load()
        {
            canvas.Children.Clear();
            base.Load();

            canvas.Children.Add(tb);
        }
        public override string ToStatusString()
        {
            return "File at " + file.FullName;
        }
    }
}

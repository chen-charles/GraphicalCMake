using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GraphicalCMake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    
    public partial class MainWindow : Window
    {
        public static Label StatusLabel = null;
        public MainWindow()
        {
            InitializeComponent();
            StatusLabel = Status;
            StatusLabel.Content = "Graphical CMake 1.0.0";

            /* https://social.msdn.microsoft.com/Forums/vstudio/en-US/2e618697-fee6-4b40-bcee-3b2d28e71d68/wpf-drag-and-drop */
            var exw = new ExploreWindow();
            exw.Show();


            //CMakeDirectory archDir = new CMakeDirectory(new DirectoryInfo("D:/gcmake_testproj/"));
            //HashSet<CMakeDirectory> subdirectories = new HashSet<CMakeDirectory>();
            //foreach (DirectoryInfo dinfo in archDir.directory.EnumerateDirectories())
            //{
            //    CMakeDirectory cd = new CMakeDirectory(dinfo);
            //    foreach (FileInfo finfo in cd.directory.EnumerateFiles())
            //    {
            //        cd.sources.Add(finfo);
            //    }
            //    subdirectories.Add(cd);
            //}

            //foreach (CMakeDirectory cd in subdirectories)
            //{
            //    MessageBox.Show(cd.ToString());
            //}


            //archDir.Name = archDir.directory.ToString();
            //CMakeTarget archTar = new CMakeTarget(CMakeArch.CMakeTarget.TargetType.EXECUTABLE, false);
            //archTar.Name = "archTar";
            //archDir.targets.Add(archTar);
            //MessageBox.Show(archDir.ToString());




            //archDir.Render();
            //mCanvas.Add(archDir);
        }


        private void MenuItemOnClick_Exit(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItem_ViewResetClick(object sender, RoutedEventArgs e)
        {
            mBorder.ResetView();
            //archDir.canvas.Children.Clear();
        }

        /* Removing the overflow part of the toolbar */
        private void ToolBar_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ToolBar toolBar = sender as ToolBar;
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = toolBar.HasOverflowItems ? Visibility.Visible : Visibility.Collapsed;
            }

            var mainPanelBorder = toolBar.Template.FindName("MainPanelBorder", toolBar) as FrameworkElement;
            if (mainPanelBorder != null)
            {
                var defaultMargin = new Thickness(0, 0, 11, 0);
                mainPanelBorder.Margin = toolBar.HasOverflowItems ? defaultMargin : new Thickness(0);
            }
        }

        private void MenuItem_Build_CMakeLists(object sender, RoutedEventArgs e)
        {
            foreach(CMakeDirectory cd in from cart in mCanvas.targets.Values where (cart is CMakeDirectory) select cart)
            {
                if (cd.isProject)
                {
                    //MessageBox.Show(cd.cdirectory.subdirectories.Count.ToString());
                    new CMakeIO.CMakeWriter(new CMakeArch.CMakeProject(cd.cdirectory.Name, cd.cdirectory));
                }
            }
        }
    }
}

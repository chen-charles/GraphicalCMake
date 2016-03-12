using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;

namespace GraphicalCMake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    
    public partial class GraphicalCMakeProject
    {
        public CanvasViewBorder border { get; private set; }
        public string savedFileName { get; private set; }
        private XmlDocument savedFile = null;
        public CMakeArch.CMakeGlobal cglobal;
        public GraphicalCMakeProject()
        {
            border = new CanvasViewBorder();
            border.Child = new CMakeArchRenderableTargetCanvas();
            border.ClipToBounds = true;
            border.AllowDrop = true;

            cglobal = new CMakeArch.CMakeGlobal();
        }

        public bool Save()
        {
            if (savedFileName == null) return false;
            savedFile = new XmlDocument();

            savedFile.CreateXmlDeclaration("1.0", "utf-8", "yes");
            XmlNode root = savedFile.CreateElement("GraphicalCMakeProject");

            var canv = (border.Child as CMakeArchRenderableTargetCanvas).targets.Values;
            foreach (CMakeArchRenderableTarget elem in canv)
            {
                if (elem.getRenderedResult() is Border)
                {
                    var bdr = elem.getRenderedResult() as Border;
                    var marg = bdr.Margin;
                    XmlNode xmlelem = savedFile.CreateElement("CMakeArchRenderableTarget");
                    XmlAttribute loc = savedFile.CreateAttribute("location");
                    loc.Value = marg.ToString();
                    XmlCDataSection cdata;
                    if (elem is CMakeDirectory)
                    {
                        cdata = savedFile.CreateCDataSection(ObjectToString((elem as CMakeDirectory).cdirectory));
                        xmlelem.AppendChild(cdata);
                    }
                    else if (elem is CMakeTarget)
                    {
                        cdata = savedFile.CreateCDataSection(ObjectToString((elem as CMakeTarget).ctarget));
                        xmlelem.AppendChild(cdata);
                    }
                    else if (elem is CMakeFile)
                    {
                        cdata = savedFile.CreateCDataSection(ObjectToString((elem as CMakeFile).file.FullName));
                        xmlelem.AppendChild(cdata);
                    }
                    root.AppendChild(xmlelem);
                }
            }
            XmlNode cg = savedFile.CreateElement("CMakeArch.CMakeGlobal");
            cg.AppendChild(savedFile.CreateCDataSection(ObjectToString(cglobal)));
            root.AppendChild(cg);

            savedFile.AppendChild(root);
            savedFile.Save(savedFileName);
            if (tabItem != null 
                && (tabItem.Header as string).Substring((tabItem.Header as string).Length - 2) == " *")
                tabItem.Header = (tabItem.Header as string).Substring(0, (tabItem.Header as string).Length - 2);
            return true;
        }

        public bool SaveAs(string fname)
        {
            savedFileName = fname;
            return Save();
        }

        public string ObjectToString(object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, obj);
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        public object StringToObject(string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String);
            using (MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length))
            {
                ms.Write(bytes, 0, bytes.Length);
                ms.Position = 0;
                return new BinaryFormatter().Deserialize(ms);
            }
        }

        public TabItem tabItem { get; set; }    //for header change, if needed
    }

    public partial class MainWindow : Window
    {
        public static Label StatusLabel = null;


        public MainWindow()
        {
            InitializeComponent();
            StatusLabel = Status;
            StatusLabel.Content = "Graphical CMake 1.0.0";


            GraphicalCMakeProject gcmp = new GraphicalCMakeProject();
            gcmp.tabItem = (viewCtrl.SelectedItem as TabItem);

            viewCtrl.AllowDrop = true;
            (viewCtrl.SelectedItem as TabItem).Header = "Project *";
            (viewCtrl.SelectedItem as TabItem).AllowDrop = true;
            (viewCtrl.SelectedItem as TabItem).Content = gcmp.border;
            (viewCtrl.SelectedItem as TabItem).Tag = gcmp;

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

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }

        private void MenuItemOnClick_Exit(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItem_ViewResetClick(object sender, RoutedEventArgs e)
        {
            //mBorder.ResetView();
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
            //foreach(CMakeDirectory cd in from cart in mCanvas.targets.Values where (cart is CMakeDirectory) select cart)
            //{
            //    if (cd.isProject)
            //    {
            //        //MessageBox.Show(cd.cdirectory.subdirectories.Count.ToString());
            //        new CMakeIO.CMakeWriter(new CMakeArch.CMakeProject(cd.cdirectory.Name, cd.cdirectory));
            //    }
            //}
        }

        /* http://stackoverflow.com/questions/10738161/is-it-possible-to-rearrange-tab-items-in-tab-control-in-wpf */
        private void TabItem_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var tabItem = e.Source as TabItem;

            if (tabItem == null)
                return;

            if (Mouse.PrimaryDevice.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(tabItem, tabItem, DragDropEffects.All);
            }
        }


        private void TabItem_Drop(object sender, DragEventArgs e)
        {
            var tabItemTarget = e.Source as TabItem;
            if (e.Data.GetDataPresent(typeof(TabItem)))
            {
                var tabItemSource = e.Data.GetData(typeof(TabItem)) as TabItem;

                if (!tabItemTarget.Equals(tabItemSource))
                {
                    var tabControl = tabItemTarget.Parent as TabControl;
                    int sourceIndex = tabControl.Items.IndexOf(tabItemSource);
                    int targetIndex = tabControl.Items.IndexOf(tabItemTarget);

                    tabControl.Items.Remove(tabItemSource);
                    tabControl.Items.Insert(targetIndex, tabItemSource);

                    tabControl.Items.Remove(tabItemTarget);
                    tabControl.Items.Insert(sourceIndex, tabItemTarget);
                }
            }
        }

        private void MenuItemOnClick_Save(object sender, RoutedEventArgs e)
        {
            if (viewCtrl.SelectedItem != null)
            {
                if ((viewCtrl.SelectedItem as TabItem).Tag is GraphicalCMakeProject)
                {
                    if (!((viewCtrl.SelectedItem as TabItem).Tag as GraphicalCMakeProject).Save())
                    {
                        MenuItemOnClick_SaveAs(sender, e);
                    }
                }
            }
        }

        private void MenuItemOnClick_SaveAs(object sender, RoutedEventArgs e)
        {
            if (viewCtrl.SelectedItem != null)
            {
                if ((viewCtrl.SelectedItem as TabItem).Tag is GraphicalCMakeProject)
                {
                    OpenFileName ofn = new OpenFileName();
                    ofn.structSize = Marshal.SizeOf(ofn);
                    ofn.filter = "XML file\0*.xml\0";

                    ofn.file = new String(new char[256]);
                    ofn.maxFile = ofn.file.Length;

                    ofn.fileTitle = new String(new char[64]);
                    ofn.maxFileTitle = ofn.fileTitle.Length;

                    ofn.initialDir = "C:\\";
                    ofn.title = "Save as ...";
                    ofn.defExt = "xml";
                    if (GetSaveFileName(ofn))
                        ((viewCtrl.SelectedItem as TabItem).Tag as GraphicalCMakeProject).SaveAs(ofn.file);
                }
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class OpenFileName
        {
            public int structSize = 0;
            public IntPtr dlgOwner = IntPtr.Zero;
            public IntPtr instance = IntPtr.Zero;

            public String filter = null;
            public String customFilter = null;
            public int maxCustFilter = 0;
            public int filterIndex = 0;

            public String file = null;
            public int maxFile = 0;

            public String fileTitle = null;
            public int maxFileTitle = 0;

            public String initialDir = null;

            public String title = null;

            public int flags = 0;
            public short fileOffset = 0;
            public short fileExtension = 0;

            public String defExt = null;

            public IntPtr custData = IntPtr.Zero;
            public IntPtr hook = IntPtr.Zero;

            public String templateName = null;

            public IntPtr reservedPtr = IntPtr.Zero;
            public int reservedInt = 0;
            public int flagsEx = 0;
        }

        [DllImport("Comdlg32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool GetSaveFileName([In, Out] OpenFileName lpofn);

        [DllImport("comdlg32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool GetOpenFileName([In, Out] OpenFileName ofn);
    }
}

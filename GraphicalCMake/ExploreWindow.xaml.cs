using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

using System.IO;

namespace GraphicalCMake
{
    /// <summary>
    /// Interaction logic for ExploreWindow.xaml
    /// </summary>
    public partial class ExploreWindow : Window
    {
        private object dummyNode = null;

        public ExploreWindow()
        {
            InitializeComponent();
            //Window_Loaded(null, null);  // Seems like this func is not called automatically, thus do it manually
        }

        public string SelectedImagePath { get; set; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (string s in Directory.GetLogicalDrives())
            {
                TreeViewItem item = new TreeViewItem();
                item.Header = s;
                item.Tag = s;
                item.FontWeight = FontWeights.Normal;
                item.Items.Add(dummyNode);
                item.Expanded += new RoutedEventHandler(folder_Expanded);

                item.MouseMove += foldersItem_MouseMove;
                foldersItem.Items.Add(item);
            }
            //MessageBox.Show("");
        }

        void folder_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)sender;
            if (item.Items.Count == 1 && item.Items[0] == dummyNode)
            {
                item.Items.Clear();
                //try
                //{
                    foreach (string s in Directory.GetDirectories(item.Tag.ToString()))
                    {
                        TreeViewItem subitem = new TreeViewItem();
                    
                        subitem.Header = s.Substring(s.LastIndexOf("\\") + 1);
                        
                        subitem.FontWeight = FontWeights.Normal;
                        subitem.Items.Add(dummyNode);
                        subitem.Expanded += new RoutedEventHandler(folder_Expanded);

                        subitem.MouseMove += foldersItem_MouseMove;

                        item.Items.Add(subitem);
                        subitem.Tag = static_foldersItem_fetchPath(subitem);
                    }

                    foreach (string s in Directory.GetFiles(item.Tag.ToString()))
                    {
                        TreeViewItem subitem = new TreeViewItem();
                        subitem.Header = s.Substring(s.LastIndexOf("\\") + 1);
                        subitem.FontWeight = FontWeights.Normal;
                        subitem.Expanded += FileSel_Expanded;
                        
                        subitem.MouseMove += foldersItem_MouseMove;
                        item.Items.Add(subitem);
                        subitem.Tag = static_foldersItem_fetchPath(subitem) + s;
                  }
                //}
                //catch (Exception err) { MessageBox.Show(err.Message); }
            }
        }

        private void FileSel_Expanded(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        //private void foldersItem_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        private string foldersItem_fetchPath(TreeView sender) { return foldersItem_fetchPath(sender.SelectedItem as TreeViewItem); }
        private string foldersItem_fetchPath(TreeViewItem tvi)
        {
            SelectedImagePath = static_foldersItem_fetchPath(tvi);
            return SelectedImagePath;
        }

        public static string static_foldersItem_fetchPath(TreeViewItem tvi)
        {
            TreeViewItem temp = tvi;

            if (temp == null)
                return null;
            var SelectedImagePath = "";

            string temp1 = "";
            string temp2 = "";
            while (true)
            {
                temp1 = temp.Header.ToString();
                if (temp1.Contains(@"\"))
                {
                    temp2 = "";
                }
                SelectedImagePath = temp1 + temp2 + SelectedImagePath;

                if (temp.Parent.GetType().Equals(typeof(TreeView)))
                {
                    break;
                }

                
                temp = ((TreeViewItem)temp.Parent);
                temp2 = @"\";
            }
            //show user selected path
            //MessageBox.Show(SelectedImagePath);
            return SelectedImagePath;
        }

        private void foldersItem_MouseMove(object sender, MouseEventArgs e)
        {
            var tvi = sender as TreeViewItem;
            DataObject d = new DataObject(typeof(string), foldersItem_fetchPath(tvi));
            if (tvi != null && e.LeftButton == MouseButtonState.Pressed)
                DragDrop.DoDragDrop(tvi, d, DragDropEffects.Copy);
        }
    }
}

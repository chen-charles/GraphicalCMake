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

namespace GraphicalCMake
{
    /// <summary>
    /// Interaction logic for DetailWindow.xaml
    /// </summary>
    public partial class DetailWindow : Window
    {
        public CMakeArchRenderableTarget cart_on_disp { get; private set; }
        public DetailWindow()
        {
            cart_on_disp = null;
            InitializeComponent();
        }

        public DetailWindow(CMakeArchRenderableTarget cart) : this()
        {
            cart_on_disp = cart;
            DetailTb.Text = cart_on_disp.ToStatusString();
            Title = cart_on_disp.ToString();
            Show();

            if (!(cart is CMakeDirectory))
            {
                isProject.IsEnabled = false;
            }
        }

        private void isProject_Checked(object sender, RoutedEventArgs e)
        {
            (cart_on_disp as CMakeDirectory).isProject = true;
        }

        private void isProject_Unchecked(object sender, RoutedEventArgs e)
        {
            (cart_on_disp as CMakeDirectory).isProject = false;
        }
    }
}


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

namespace StudentSocial.GUI
{
    /// <summary>
    /// Interaction logic for WDialog.xaml
    /// </summary>
    public partial class WDialog : Window
    {
        public WDialog()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnHuy_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnXacNhan_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void BtnXacNhan_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DialogResult = true;
        }

        private void BtnHuy_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DialogResult = false;
        }
    }
}

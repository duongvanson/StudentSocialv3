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
    /// Interaction logic for WDialogNoti.xaml
    /// </summary>
    public partial class WDialogNoti : Window
    {
        public WDialogNoti()
        {
            InitializeComponent();
        }
        public WDialogNoti(String content)
        {
            InitializeComponent();
            lblContent.Text = content;
        }
        private void BtnXacNhan_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace StudentSocial.GUI
{
    /// <summary>
    /// Interaction logic for PHelp.xaml
    /// </summary>
    public partial class PHelp : Page
    {
        public PHelp()
        {
            InitializeComponent();
        }

        private void SpnlMail_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("mailto:dev.eagleteam@gmail.com?subject=StudentSocial&amp;body=HiEagleTeam");
        }

        private void SpnlMess_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://www.facebook.com/messages/t/duongvansonit");
        }
    }
}

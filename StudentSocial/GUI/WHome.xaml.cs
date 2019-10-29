using StudentSocial.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StudentSocial.GUI
{
    /// <summary>
    /// Interaction logic for WHome.xaml
    /// </summary>
    public partial class WHome : Window
    {
        public WHome()
        {
            InitializeComponent();
        }
        #region private var
        //private bool move = false;
        //private Point mouse = new Point();
        #endregion
        Thread thUpdate;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lboxMenu.SelectedIndex = 0;
            //Đọc hết dữ liệu đã lưu trong file
            Commons.readDataToFile();
            //Lấy thông tin sinh viên
            getInfoStudent();
            File.WriteAllText(Paths.version, "1.3.1.3");
            thUpdate = new Thread(new ThreadStart(checkUpdate));
            thUpdate.Start();
        }

        private void checkUpdate()
        {
            if (Commons.checkInternet())
            {
                var urlVersion = "https://raw.githubusercontent.com/duongvanson/ss/master/version.txt";
                WebClient client = new WebClient();
                Stream stream = client.OpenRead(urlVersion);
                StreamReader reader = new StreamReader(stream);
                String content = reader.ReadToEnd();
                var versionOld = File.ReadAllText(Paths.version);
                if (content.Trim() != versionOld.Trim())
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        lblUpdateVersion.Content = "Đã có phiên bản mới";
                    });
                    thUpdate.Abort();
                }
            }
        }
        private void getInfoStudent()
        {
            string name = Commons.profile.HoTen;
            lblLogo.Content = name.Substring(name.LastIndexOf(" ") + 1, 1);
            lblName.Content = name;
            lblClass.Content = Commons.profile.Lop;
        }

        private void LboxMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lboxMenu.SelectedIndex == -1) return;
            setEffectMenu();
            var menu = (ListBoxItem)lboxMenu.SelectedItem;
            switch (menu.Tag.ToString())
            {
                case "time":
                    viewMain.Content = new PTime();
                    break;
                case "mark":
                    viewMain.Content = new PMark();
                    break;
                case "timeout":
                    viewMain.Content = new PInAndOut();
                    break;
                case "setting":
                    viewMain.Content = new PSetting();
                    break;
                case "action":
                    viewMain.Content = new PAction();
                    break;
                case "logout":
                    WDialog wDialog = new WDialog();
                    wDialog.ShowDialog();
                    if (wDialog.DialogResult.HasValue && wDialog.DialogResult.Value)
                    {
                        Paths.clearData();
                        Process.Start(Application.ResourceAssembly.Location);
                        Application.Current.Shutdown();
                    }
                    else
                    {
                        lboxMenu.SelectedIndex = 0;
                    }
                    break;
                default:
                    break;
            }
        }

        private void setEffectMenu()
        {
            //Loại bỏ hiệu ứng
            for (int i = 0; i < lboxMenu.Items.Count; i++)
            {
                var temp = (ListBoxItem)lboxMenu.Items[i];
                //temp.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#1abc9c"));
                //temp.BorderThickness = new Thickness(0, 0, 0, 0);
                temp.FontWeight = FontWeights.Normal;
            }
            //Áp dụng hiệu ứng cho menu được chọn
            var menu = (ListBoxItem)lboxMenu.SelectedItem;
            //menu.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#1abc9c"));
            //menu.BorderThickness = new Thickness(4, 0, 0, 0);
            menu.FontWeight = FontWeights.SemiBold;
        }
        private void PnlTop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void ImgMini_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ImgClose_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //do some thing
        }

        private void SpnlHelp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            viewMain.Content = new PHelp();
            lboxMenu.SelectedIndex = -1;
        }

        private void ViewMain_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            var ta = new ThicknessAnimation();
            ta.Duration = TimeSpan.FromSeconds(0.5);
            ta.DecelerationRatio = 0.7;
            ta.To = new Thickness(0, 0, 0, 0);
            if (e.NavigationMode == NavigationMode.New)
            {
                ta.From = new Thickness(0, 100, 0, 0);
            }
            else if (e.NavigationMode == NavigationMode.Back)
            {
                ta.From = new Thickness(0, 0, 0, 100);
            }
                 (e.Content as Page).BeginAnimation(MarginProperty, ta);
        }

        private void LblUpdateVersion_MouseDown(object sender, MouseButtonEventArgs e)
        {
            viewMain.Content = new PUpdateVersion();
            lboxMenu.SelectedIndex = -1;
        }

        private void LblUpdate_MouseDown(object sender, MouseButtonEventArgs e)
        {
            viewMain.Content = new PUpdate();
            lboxMenu.SelectedIndex = -1;
        }
    }
}

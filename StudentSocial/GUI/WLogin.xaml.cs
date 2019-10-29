using StudentSocial.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for WLogin.xaml
    /// </summary>
    public partial class WLogin : Window
    {
        public WLogin()
        {
            InitializeComponent();
        }

        private void LblForget_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("http://dangkytinchi.ictu.edu.vn/kcntt/LostPassword.aspx");
        }

        private void LblExit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            lblStatus.Content = "Đang đăng nhập...";
            lblStatus.Foreground = Brushes.Green;
            new Thread(new ThreadStart(login)).Start();
        }
        private void login()
        {
            var username = "";
            var password = "";
            this.Dispatcher.Invoke(() =>
            {
                username = txtCode.Text;
                password = txtPass.Password;
            });
            username = username.ToUpper();
            var result = ConnectAPI.Login(username, password);
            if (result != "false" && result != "error")
            {
                this.Dispatcher.Invoke(() =>
                {
                    lblStatus.Content = "Đăng nhập thành công!\nĐang chuẩn bị lưu dữ liệu, vui lòng đợi!";
                    lblStatus.Foreground = Brushes.Green;
                });
                ConnectAPI.getProfile();
                this.Dispatcher.Invoke(() =>
                {
                    lblStatus.Content = "(1) Đang tải danh sách kỳ học...";
                    lblStatus.Foreground = Brushes.Green;
                });
                ConnectAPI.getSemester();
                getSemester();
                this.Dispatcher.Invoke(() =>
                {
                    lblStatus.Content = "(2) Đang lưu lịch học...";
                    lblStatus.Foreground = Brushes.Green;
                });
                ConnectAPI.getTime();
                this.Dispatcher.Invoke(() =>
                {
                    lblStatus.Content = "(3) Đang kiểm tra lịch thi...";
                    lblStatus.Foreground = Brushes.Green;
                });
                ConnectAPI.getExam();
                this.Dispatcher.Invoke(() =>
                {
                    lblStatus.Content = "(4) Đang tải điểm học phần...";
                    lblStatus.Foreground = Brushes.Green;
                });
                ConnectAPI.getMark();
                this.Dispatcher.Invoke(() =>
                {
                    Process.Start(Application.ResourceAssembly.Location);
                    Application.Current.Shutdown();
                });
            }
            else
            {
                if (result == "error")
                {
                    WDialogNoti noti = new WDialogNoti("Có lỗi xảy ra vui lòng thử lại sau!");
                    noti.ShowDialog();
                }
                this.Dispatcher.Invoke(() =>
                {
                    lblStatus.Content = "Đăng nhập thất bại!";
                    lblStatus.Foreground = Brushes.Red;
                });

            }
        }
        private void getSemester()
        {
            Commons.readSemester();
            for (int i = 0; i < Commons.lstKyHoc.Count; i++)
            {
                if (Commons.lstKyHoc[i].KyHienTai == true)
                {
                    Commons.semesterNow = Commons.lstKyHoc[i].MaKy;
                    break;
                }
            }
        }
    }
}

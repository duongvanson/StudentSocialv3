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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AltoHttp;
using StudentSocial.Common;

namespace StudentSocial.GUI
{
    /// <summary>
    /// Interaction logic for PUpdateVersion.xaml
    /// </summary>
    public partial class PUpdateVersion : Page
    {
        public PUpdateVersion()
        {
            InitializeComponent();
        }
        HttpDownloader downloader;
        static string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Student Social");
        string version = path + @"/version.txt";
        string setup = path + @"/SetupStudentSocial.msi";
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (!Commons.checkInternet())
            {
                lblStatus.Content = "Vui lòng kiểm tra kết nối mạng rồi thử lại.";
                lblStatus.Foreground = Brushes.Red;
            }
            else
            {
                var urlVersion = "https://raw.githubusercontent.com/duongvanson/ss/master/version.txt";
                WebClient client = new WebClient();
                Stream stream = client.OpenRead(urlVersion);
                StreamReader reader = new StreamReader(stream);
                String content = reader.ReadToEnd();
                var versionOld = File.ReadAllText(Paths.version);
                if (content != versionOld)
                {
                    spnlView.Visibility = Visibility.Visible;
                    lblStatus.Foreground = Brushes.Green;
                    lblStatus.Content = "Có phiên bản mới, hãy cập nhật luôn nào!\nCó gì mới: "+Commons.infoUpdate;
                }
                else
                {
                    lblStatus.Foreground = Brushes.Green;
                    lblStatus.Content = "Bạn đang sử dụng phiên bản mới nhất của Student Social.";
                }
            }
        }

        private void Downloader_DownloadCompleted(object sender, EventArgs e)
        {
            lblSpeed.Content = "Tải xuống hoàn tất!\nĐang tiến hành cài đặt . . .";
            Thread.Sleep(1000);
            Process.Start(path + "/SetupStudentSocial.msi");
            Application.Current.Shutdown();
        }

        private void Downloader_DownloadProgressChanged(object sender, EventArgs e)
        {
            proView.Visibility = Visibility.Visible;
            double received = downloader.TotalBytesReceived / 1024;
            double total = downloader.SizeInBytes / 1024;
            double speed = downloader.SpeedInBytes / 1024;
            lblSpeed.Content = "Đang tải xuống: " + received + "Kb/"+total + "Kb\tTốc độ: "+speed+"Kb/s";
            proView.Value = received / total * 100;
        }

        private void BtnCapNhat_Click(object sender, RoutedEventArgs e)
        {
            lblSpeed.Content = "Đang kết nối, vui lòng đợi . . .";
            btnCapNhat.IsEnabled = false;
            var urlSetup = "https://github.com/duongvanson/ss/raw/master/SetupStudentSocial.msi";
            downloader = new HttpDownloader(urlSetup, setup);
            downloader.DownloadProgressChanged += Downloader_DownloadProgressChanged; ;
            downloader.DownloadCompleted += Downloader_DownloadCompleted; ;
            downloader.Start();
        }

        private void LblUninstall_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("appwiz.cpl");
        }

        private void LblFile_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start(path);
        }
    }
}

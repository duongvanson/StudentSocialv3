using StudentSocial.Common;
using StudentSocial.Model;
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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StudentSocial.GUI
{
    /// <summary>
    /// Interaction logic for PTime.xaml
    /// </summary>
    public partial class PTime : Page
    {
        public PTime()
        {
            InitializeComponent();
        }
        private int day;
        private int month;
        private int year;
        private Label lblNow = new Label();
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            getMonth(DateTime.Now.Month, DateTime.Now.Year);
            loadCalender(DateTime.Now.Month, DateTime.Now.Year);
            pushData();
        }

        private void loadCalender(int month, int year)
        {
            gCalender.Children.Clear();
            var first = new DateTime(year, month, 1);
            var day = (int)first.DayOfWeek;
            day = day == 0 ? 7 : day;
            int now = 1;
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (i == 0 && j >= day - 1)
                    {
                        StackPanel panel = new StackPanel()
                        {

                        };
                        Label lblDay = new Label()
                        {
                            Cursor = Cursors.Hand,
                            Content = now++,
                            HorizontalContentAlignment = HorizontalAlignment.Center,
                            VerticalContentAlignment = VerticalAlignment.Center,
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            VerticalAlignment = VerticalAlignment.Stretch,
                            FontSize = 18,
                            Padding = new Thickness(0, 5, 0, 0),
                        };
                        lblDay.MouseLeftButtonDown += LblDay_MouseLeftButtonDown;
                        panel.Children.Add(lblDay);
                        gCalender.Children.Add(panel);
                        Grid.SetRow(panel, i);
                        Grid.SetColumn(panel, j);
                    }
                    else if (i > 0)
                    {
                        StackPanel panel = new StackPanel();
                        Label lblDay = new Label()
                        {
                            Cursor = Cursors.Hand,
                            Content = now++,
                            //BorderThickness = new Thickness(1),
                            //BorderBrush = Brushes.Green,
                            HorizontalContentAlignment = HorizontalAlignment.Center,
                            VerticalContentAlignment = VerticalAlignment.Center,
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            VerticalAlignment = VerticalAlignment.Stretch,
                            FontSize = 18,
                            Padding = new Thickness(0, 5, 0, 0)
                        };
                        lblDay.MouseLeftButtonDown += LblDay_MouseLeftButtonDown;
                        panel.Children.Add(lblDay);
                        gCalender.Children.Add(panel);
                        Grid.SetRow(panel, i);
                        Grid.SetColumn(panel, j);
                    }
                    if (now > DateTime.DaysInMonth(year, month))
                    {
                        break;
                    }
                }
                if (now > DateTime.DaysInMonth(year, month))
                {
                    break;
                }
            }
            //Kiem tra ngay hom nay
            getFullDate();
            if (month == DateTime.Now.Month)
            {
                for (int i = 0; i < gCalender.Children.Count; i++)
                {
                    if (gCalender.Children[i] != null)
                    {
                        StackPanel stack = (StackPanel)gCalender.Children[i];
                        Label lbl = (Label)stack.Children[0];
                        if ((int)lbl.Content == DateTime.Now.Day)
                        {
                            stack.Background = Brushes.Orange;
                            lblNow = lbl;
                        }
                    }
                }
            }
        }
        private void LblDay_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Label lbl = sender as Label;
            setBackgroundSelectDay(lbl);
            //Đẩy lịch học theo ngày
            int soLich = 0;
            gView.Children.Clear();
            spnlView.Children.Clear();
            getFullDate();
            day = Convert.ToInt32(lbl.Content);
            var date = new DateTime(year, month, day);
            var lstLich = new List<Lich>();
            //Đếm số lịch
            for (int i = 0; i < Commons.lstLichHoc.Count; i++)
            {
                var lich = Commons.lstLichHoc[i];
                var mon = "";
                for (int j = 0; j < Commons.lstMonHoc.Count; j++)
                {
                    if (lich.MaMon == Commons.lstMonHoc[j].MaMon)
                    {
                        mon = Commons.lstMonHoc[j].TenMon;
                        break;
                    }
                }
                if (lich.Ngay == date.ToString("yyyy-MM-dd"))
                {
                    soLich++;
                    lstLich.Add(lich);
                    
                }
            }
            #region Sắp xếp lịch học theo thứ tự tiết
            for (int i = 0; i < soLich; i++)
            {
                for (int j = i+1; j < soLich; j++)
                {
                    int dauI = Convert.ToInt32(lstLich[i].ThoiGian.Substring(0, 1));
                    int dauJ = Convert.ToInt32(lstLich[j].ThoiGian.Substring(0, 1));
                    if (dauI > dauJ)
                    {
                        var temp = lstLich[i];
                        lstLich[i] = lstLich[j];
                        lstLich[j] = temp;
                    }
                }
            }
            #endregion
            if (soLich == 0)
            {
                #region Border & Panel
                StackPanel panel = new StackPanel();
                Border border = new Border()
                {
                    Background = Brushes.White,
                    Margin = new Thickness(5, 3, 5, 3),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(7),
                    Height = 80,
                };
                UIElement uie = new UIElement();
                uie.Effect =
                            new DropShadowEffect
                            {
                                Color = Colors.Gray,
                                ShadowDepth = 0,
                                BlurRadius = 8,
                                RenderingBias = RenderingBias.Quality
                            };
                border.Effect = uie.Effect;
                #endregion

                Label lblNghi = new Label()
                {
                    Background = Brushes.DarkOrange,
                    Content = "Hôm nay bạn không có lịch học!",
                    FontSize = 14,
                    FontWeight = FontWeights.DemiBold,
                    Foreground = Brushes.White,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Padding = new Thickness(10, 5, 10, 5)
                };
                panel.Children.Add(lblNghi);
                border.Child = panel;
                spnlView.Children.Add(border);
            }
            if (soLich > 0 && soLich <= 2)
            {
                for (int i = 0; i < soLich; i++)
                {
                    #region Border & Panel
                    StackPanel panel = new StackPanel();
                    Border border = new Border()
                    {
                        Background = Brushes.White,
                        Margin = new Thickness(5, 3, 5, 3),
                        BorderThickness = new Thickness(1),
                        CornerRadius = new CornerRadius(7),
                        Height = 80,
                    };
                    UIElement uie = new UIElement();
                    uie.Effect =
                                new DropShadowEffect
                                {
                                    Color = Colors.Gray,
                                    ShadowDepth = 0,
                                    BlurRadius = 8,
                                    RenderingBias = RenderingBias.Quality
                                };
                    border.Effect = uie.Effect;
                    #endregion
                    var mon = "";
                    for (int j = 0; j < Commons.lstMonHoc.Count; j++)
                    {
                        if (lstLich[i].MaMon == Commons.lstMonHoc[j].MaMon)
                        {
                            mon = Commons.lstMonHoc[j].TenMon;
                            break;
                        }
                    }
                    if (lstLich[i].LoaiLich == "LichHoc")
                    {
                        #region Lịch học
                        Label lblTenMoon = new Label()
                        {
                            Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#1abc5c")),
                            Content = mon,
                            Foreground = Brushes.White,
                            FontWeight = FontWeights.DemiBold,
                            FontSize = 14,
                            Padding = new Thickness(10, 5, 10, 5),
                        };
                        Label lblTime = new Label()
                        {
                            Content = "● Tiết " + lstLich[i].ThoiGian + " tại " + lstLich[i].DiaDiem,
                            FontSize = 14,
                            Padding = new Thickness(15, 5, 15, 5),
                        };
                        Label lblTeacher = new Label()
                        {
                            Content = "● Giảng viên: " + lstLich[i].GiaoVien,
                            FontSize = 14,
                            Padding = new Thickness(15, 5, 15, 5),
                        };

                        panel.Children.Add(lblTenMoon);
                        panel.Children.Add(lblTime);
                        panel.Children.Add(lblTeacher);
                        border.Child = panel;
                        spnlView.Children.Add(border);
                        #endregion
                    }
                    else if (lstLich[i].LoaiLich == "LichThi")
                    {
                        border.Height = 110;
                        #region Lịch thi
                        Label lblTenMon = new Label()
                        {
                            Background = Brushes.Red,
                            Content = mon,
                            Foreground = Brushes.White,
                            FontWeight = FontWeights.DemiBold,
                            FontSize = 14,
                            Padding = new Thickness(10, 5, 10, 5),
                        };
                        Label lblTime = new Label()
                        {
                            Content = "● " + lstLich[i].ThoiGian + " tại " + lstLich[i].DiaDiem,
                            FontSize = 14,
                            Padding = new Thickness(15, 5, 15, 5),
                        };
                        Label lblHinhThuc = new Label()
                        {
                            Content = "● Hình thức: " + lstLich[i].HinhThuc,
                            FontSize = 14,
                            Padding = new Thickness(15, 5, 15, 5),
                        };
                        Label lblSoBaoDanh = new Label()
                        {
                            Content = "● Số báo danh: " + lstLich[i].SoBaoDanh,
                            FontSize = 14,
                            Padding = new Thickness(15, 5, 15, 5),
                        };
                        panel.Children.Add(lblTenMon);
                        panel.Children.Add(lblTime);
                        panel.Children.Add(lblHinhThuc);
                        panel.Children.Add(lblSoBaoDanh);
                        border.Child = panel;
                        spnlView.Children.Add(border);
                        #endregion

                    }
                }
            }
            if (soLich >= 3)
            {
                int check = 0, index = 0;
                int loop = soLich / 2;
                for (int i = 0; i <= loop; i++)
                {
                    if (check == -1) break;
                    for (int j = 0; j < 2; j++)
                    {
                        //soLich--;
                        //if (soLich <= 0) check = -1;
                        if (index >= lstLich.Count)
                        {
                            break;
                        }
                        #region Border & Panel
                        StackPanel panel = new StackPanel();
                        Border border = new Border()
                        {
                            Background = Brushes.White,
                            Margin = new Thickness(5, 3, 5, 3),
                            BorderThickness = new Thickness(1),
                            CornerRadius = new CornerRadius(7),
                            Height = 80,
                        };
                        UIElement uie = new UIElement();
                        uie.Effect =
                                    new DropShadowEffect
                                    {
                                        Color = Colors.Gray,
                                        ShadowDepth = 0,
                                        BlurRadius = 8,
                                        RenderingBias = RenderingBias.Quality
                                    };
                        border.Effect = uie.Effect;
                        #endregion
                        var mon = "";
                        for (int z = 0; z < Commons.lstMonHoc.Count; z++)
                        {
                            if (lstLich[index].MaMon == Commons.lstMonHoc[z].MaMon)
                            {
                                mon = Commons.lstMonHoc[z].TenMon;
                                break;
                            }
                        }
                        if (lstLich[index].LoaiLich == "LichHoc")
                        {
                            #region Lịch học
                            Label lblTenMoon = new Label()
                            {
                                Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#1abc5c")),
                                Content = mon,
                                Foreground = Brushes.White,
                                FontWeight = FontWeights.DemiBold,
                                FontSize = 14,
                                Padding = new Thickness(10, 5, 10, 5),
                            };
                            Label lblTime = new Label()
                            {
                                Content = "● Tiết " + lstLich[index].ThoiGian + " tại " + lstLich[index].DiaDiem,
                                FontSize = 14,
                                Padding = new Thickness(15, 5, 15, 5),
                            };
                            Label lblTeacher = new Label()
                            {
                                Content = "● Giảng viên: " + lstLich[index].GiaoVien,
                                FontSize = 14,
                                Padding = new Thickness(15, 5, 15, 5),
                            };

                            panel.Children.Add(lblTenMoon);
                            panel.Children.Add(lblTime);
                            panel.Children.Add(lblTeacher);
                            border.Child = panel;
                            gView.Children.Add(border);
                            Grid.SetColumn(border, j);
                            Grid.SetRow(border, i);
                            #endregion
                        }
                        else if (lstLich[index].LoaiLich == "LichThi")
                        {
                            border.Height = 110;
                            #region Lịch thi
                            Label lblTenMon = new Label()
                            {
                                Background = Brushes.Red,
                                Content = mon,
                                Foreground = Brushes.White,
                                FontWeight = FontWeights.DemiBold,
                                FontSize = 14,
                                Padding = new Thickness(10, 5, 10, 5),
                            };
                            Label lblTime = new Label()
                            {
                                Content = "● " + lstLich[index].ThoiGian + " tại " + lstLich[index].DiaDiem,
                                FontSize = 14,
                                Padding = new Thickness(15, 5, 15, 5),
                            };
                            Label lblHinhThuc = new Label()
                            {
                                Content = "● Hình thức: " + lstLich[index].HinhThuc,
                                FontSize = 14,
                                Padding = new Thickness(15, 5, 15, 5),
                            };
                            Label lblSoBaoDanh = new Label()
                            {
                                Content = "● Số báo danh: " + lstLich[index].SoBaoDanh,
                                FontSize = 14,
                                Padding = new Thickness(15, 5, 15, 5),
                            };
                            panel.Children.Add(lblTenMon);
                            panel.Children.Add(lblTime);
                            panel.Children.Add(lblHinhThuc);
                            panel.Children.Add(lblSoBaoDanh);
                            border.Child = panel;
                            gView.Children.Add(border);
                            Grid.SetColumn(border, j);
                            Grid.SetRow(border, i);
                            #endregion

                        }
                        index++;
                    }
                }
            }
        }

        private void setBackgroundSelectDay(Label lbl)
        {
            ImageBrush ib = new ImageBrush()
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/Assets/select.png")),
                Stretch = Stretch.Uniform,
            };
            ((StackPanel)lbl.Parent).Background = ib;
            lbl.Foreground = Brushes.White;
            //lbl.FontWeight = FontWeights.SemiBold;
            for (int i = 0; i < gCalender.Children.Count; i++)
            {
                if (lbl != ((Label)(((StackPanel)gCalender.Children[i]).Children[0])))
                {
                    ((StackPanel)gCalender.Children[i]).Background = Brushes.White;
                    ((Label)(((StackPanel)gCalender.Children[i]).Children[0])).Foreground = Brushes.Black;
                }
                if (lblNow == ((Label)(((StackPanel)gCalender.Children[i]).Children[0])))
                {
                    ((StackPanel)gCalender.Children[i]).Background = new ImageBrush()
                    {
                        ImageSource = new BitmapImage(new Uri("pack://application:,,,/Assets/selectnow.png")),
                        Stretch = Stretch.Uniform,
                    }; ;
                }
            }
        }

        private void getFullDate()
        {
            var str = lblMonth.Text;
            day = DateTime.Now.Day;
            month = Convert.ToInt32(str.Substring(str.IndexOf(" ") + 1, 2));
            year = Convert.ToInt32(str.Substring(str.LastIndexOf(" ") + 1, 4));
        }

        private void getMonth(int month, int year)
        {
            if (month < 10)
            {
                lblMonth.Text = "Tháng 0" + month + ", " + year;
            }
            else lblMonth.Text = "Tháng " + month + ", " + year;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            getFullDate();
            if (month > 1)
            {
                month -= 1;
            }
            else
            {
                month = 12;
                year -= 1;
            }
            getMonth(month, year);
            loadCalender(month, year);
            pushData();
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            getFullDate();
            if (month < 12)
            {
                month += 1;
            }
            else
            {
                month = 1;
                year += 1;
            }
            getMonth(month, year);
            loadCalender(month, year);
            pushData();
        }

        private void pushData()
        {
            getFullDate();
            var maxDay = DateTime.DaysInMonth(year, month);
            int[] a = new int[maxDay + 1]; //Lưu số lịch của ngày
            for (int i = 0; i < Commons.lstLichHoc.Count; i++)
            {
                Lich lich = Commons.lstLichHoc[i] as Lich;
                var ngay = Convert.ToDateTime(lich.Ngay);
                if (ngay.Month == month && ngay.Year == year)
                {
                    a[ngay.Day]++;
                    if (lich.LoaiLich == "LichThi")
                    {
                        a[ngay.Day] = -1;
                    }
                }
            }
            for (int i = 0; i < gCalender.Children.Count; i++)
            {
                if (gCalender.Children[i] != null)
                {
                    StackPanel stack = (StackPanel)gCalender.Children[i];
                    Label lbl = (Label)stack.Children[0];
                    for (int j = 0; j < a.Length; j++)
                    {
                        if ((int)lbl.Content == j)
                        {
                            Label cal = new Label()
                            {
                                FontSize = 14,
                                FontWeight = FontWeights.Bold,
                                HorizontalContentAlignment = HorizontalAlignment.Center,
                                VerticalContentAlignment = VerticalAlignment.Center,
                                Padding = new Thickness(0)

                            };
                            if (a[j] == 0)
                            {
                                lbl.MinHeight = 50;
                                lbl.Padding = new Thickness(0);
                            }
                            if (a[j] == 1)
                            {
                                cal.Content = "●";
                                cal.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#1abc5c"));
                                stack.Children.Add(cal);
                            }
                            if (a[j] == 2)
                            {
                                cal.Content = "●●";
                                cal.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#1abc5c"));
                                stack.Children.Add(cal);
                            }
                            if (a[j] == 3)
                            {
                                cal.Content = "●●●";
                                cal.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#1abc5c"));
                                stack.Children.Add(cal);
                            }
                            if (a[j] > 3)
                            {
                                cal.Content = "●●+";
                                cal.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#1abc5c"));
                                stack.Children.Add(cal);
                            }
                            if (a[j] == -1)
                            {
                                cal.Content = "●";
                                cal.Foreground = Brushes.Red;
                                stack.Children.Add(cal);
                            }
                        }
                    }
                }
            }
            getFullDate();
            if (day == DateTime.Now.Day && month == DateTime.Now.Month && year == DateTime.Now.Year)
            {
                MouseButtonEventArgs mouse = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left);
                LblDay_MouseLeftButtonDown(lblNow, mouse);
            }
        }
    }
}

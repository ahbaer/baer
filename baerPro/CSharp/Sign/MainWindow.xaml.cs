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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Sign
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSign_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnRemindNext_Click(object sender, RoutedEventArgs e)
        {
            //设定半小时间隔
            timer.Interval = TimeSpan.FromHours(0.5);
            timer.Tick += Timer_Tick;
            timer.Start();

            //隐藏当前窗体
            this.Visibility = Visibility.Hidden;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            //显示当前窗体
            this.Visibility = Visibility.Visible;
            timer.Stop();
        }
    }
}

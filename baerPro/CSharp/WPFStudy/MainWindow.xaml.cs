using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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

namespace WPFStudy
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Human h = (Human)this.FindResource("human");
            if (!h.Equals(null))
            {
                System.Windows.MessageBox.Show(h.Child.Name);
            }
        }
    }

    [TypeConverter(typeof(NameToHumanConverter))]
    public class Human
    {
        public string Name { set; get; }
        public Human Child { set; get; }
    }

    public class NameToHumanConverter:TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string name = value.ToString();
            Human chlid = new Human();
            chlid.Name = name;
            return chlid;
        }
    }
}

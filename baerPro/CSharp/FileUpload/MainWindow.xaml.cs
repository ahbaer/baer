using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FileUpload
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

        private void btnPick_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = "d:\\";
                openFileDialog.Filter = "文本文件|*.txt|C#文件|*.cs|所有文件|*.*";
                openFileDialog.RestoreDirectory = true;
                openFileDialog.FilterIndex = 1;
                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string path = openFileDialog.FileName;
                    string fileName = path.Split('\\')[path.Split('\\').Length - 1];
                    string fileType = fileName.Split('.')[1];

                    txtFile.Text = fileName;

                    byte[] buffer = File.ReadAllBytes(path);
                    string fileContent = Convert.ToBase64String(buffer);

                    BaersTool.DB.Data data = new BaersTool.DB.Data("UploadFile");
                    data["FileName"] = fileName;
                    data["FileType"] = fileType;
                    data["FileContent"] = fileContent;
                    data.Insert();

                    System.Windows.MessageBox.Show("文件上传成功！");
                }
            }
            catch (Exception ex)
            {
                BaersTool.Log.LogOperate.WriteLog(ex.ToString(), DateTime.Now.ToString("yyyy_MM_dd") + ".log", System.Windows.Forms.Application.StartupPath + "\\LogFiles");
                throw;
            }
        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                string path = System.Windows.Forms.Application.StartupPath + "\\Download\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                DataView dvFile = BaersTool.DB.Data.ExecuteToDataView("select * from UploadFile");
                foreach (DataRowView file in dvFile)
                {
                    byte[] buffer = Convert.FromBase64String(Convert.ToString(file["FileContent"]));

                    FileStream fs = new FileStream(path + Convert.ToString(file["FileName"]), FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                    fs.Write(buffer, 0, buffer.Length);
                    fs.Close();
                }

                System.Windows.MessageBox.Show("文件下载成功！");
            }
            catch (Exception ex)
            {
                BaersTool.Log.LogOperate.WriteLog(ex.ToString(), DateTime.Now.ToString("yyyy_MM_dd") + ".log", System.Windows.Forms.Application.StartupPath + "\\LogFiles");
                throw;
            }
        }
    }
}

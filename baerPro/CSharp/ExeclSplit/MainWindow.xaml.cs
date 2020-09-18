using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using Spire.Xls;

namespace ExeclSplit
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string FilePath;
        public static string FileName;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnPick_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "d:\\";
            openFileDialog.Filter = "文本文件|*.xlsx|所有文件|*.*";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FilePath = openFileDialog.FileName;
                FileName = FilePath.Split('\\')[FilePath.Split('\\').Length - 1];
                txtFile.Text = FileName;
            }
        }

        private void btnSplit_Click(object sender, RoutedEventArgs e)
        {
            //读取将要拆分的execl
            Workbook bookOriginal = new Workbook();
            bookOriginal.LoadFromFile(FilePath);
            Worksheet sheet = bookOriginal.Worksheets[0];

            //新建一个Workbook
            Workbook newBook = new Workbook();
            newBook.CreateEmptySheets(1);
            Worksheet newSheet = newBook.Worksheets[0];

            int j = 1;
            int rangeIndex = Convert.ToInt32(txtRange.Text);
            for (int i = 1; i < sheet.LastRow; i += rangeIndex)
            {
                CellRange range;
                if (i + rangeIndex > sheet.LastRow)
                {
                    range = sheet.Range[i, 1, sheet.LastRow, sheet.LastColumn];
                }
                else
                {
                    range = sheet.Range[i, 1, i + rangeIndex, sheet.LastColumn];
                }

                newSheet.Copy(range, newSheet.Range[1, 1]);
                newBook.SaveToFile("D:\\execl\\" + FileName + "_" + j + ".xlsx", ExcelVersion.Version2007);
                j++;
            }

            System.Windows.MessageBox.Show("文件切割成功！");
        }
    }
}

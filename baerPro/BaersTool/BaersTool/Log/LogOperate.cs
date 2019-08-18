using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaersTool.Log
{
    public class LogOperate
    {
        public static void WriteLog(string logContent)
        {
            string strLogFilePath = System.Environment.CurrentDirectory.Replace("bin\\Debug", "LogFiles\\");
            if (!Directory.Exists(strLogFilePath))
            {
                Directory.CreateDirectory(strLogFilePath);
            }

            string strLogFileName = strLogFilePath + DateTime.Now.ToString("yyyy_MM_dd") + ".log"; //日期
            using (FileStream fs = new FileStream(strLogFileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                logContent = DateTime.Now.ToString() + "：" + logContent + "\r\n";
                byte[] bytes = System.Text.Encoding.Default.GetBytes(logContent);
                fs.Write(bytes, 0, bytes.Length);
            }
        }

        public static void WriteLog(string logContent, string logFileName)
        {
            string strLogFilePath = System.Environment.CurrentDirectory.Replace("bin\\Debug", "LogFiles\\");
            if (!Directory.Exists(strLogFilePath))
            {
                Directory.CreateDirectory(strLogFilePath);
            }

            string strLogFileName = strLogFilePath + logFileName + ".log"; //日期
            using (FileStream fs = new FileStream(strLogFileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                logContent = DateTime.Now.ToString() + "：" + logContent + "\r\n";
                byte[] bytes = System.Text.Encoding.Default.GetBytes(logContent);
                fs.Write(bytes, 0, bytes.Length);
            }
        }

        public static void WriteLog(string logContent, string logFileName, string logFilePath)
        {
            if (!Directory.Exists(logFilePath))
            {
                Directory.CreateDirectory(logFilePath);
            }

            string strLogFileName = logFilePath + "\\" + logFileName + ".log"; //日期
            using (FileStream fs = new FileStream(strLogFileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                logContent = DateTime.Now.ToString() + "：" + logContent + "\r\n";
                byte[] bytes = System.Text.Encoding.Default.GetBytes(logContent);
                fs.Write(bytes, 0, bytes.Length);
            }
        }
    }
}

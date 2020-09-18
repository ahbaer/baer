using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//ftp类
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;

namespace ConsoleApplication2
{
    public class FtpHelper
    {
        //基本设置
        private string _path = @"ftp://" + "192.168.1.103" + "/";//目标路径（根路径）
        private string _ftpIP = "192.168.1.103";//ftp IP地址
        private string _userName = "root";//ftp用户名
        private string _password = "";//ftp密码

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">目标ftp根路径</param>
        /// <param name="ftpIP">目标ftp ip</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        public FtpHelper(string path, string ftpIP, string userName, string password)
        {
            this._path = path;
            this._ftpIP = ftpIP;
            this._userName = userName;
            this._password = password;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objPath">目标ftp路径（不填为根路径）</param>
        /// <returns></returns>
        public List<string> GetFileList(string objPath = "")
        {
            List<string> downloadFiles = new List<string>();
            FtpWebRequest request;
            try
            {
                request = (FtpWebRequest)FtpWebRequest.Create(new Uri(_path + objPath));
                request.UseBinary = true;
                request.Credentials = new NetworkCredential(_userName, _password);//设置用户名和密码
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.UseBinary = true;

                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());

                string line = reader.ReadLine();
                while (line != null)
                {
                    downloadFiles.Add(line);
                    line = reader.ReadLine();
                }
                reader.Close();
                response.Close();
                return downloadFiles;
            }
            catch (Exception ex)
            {
                Epoint.Frame.Common.LogOperate.WriteLog_ToFileName("FtpHelper_" + DateTime.Now.ToString("yyyyMMdd") + ".log", "GetFileList：" + ex.ToString());
                downloadFiles = null;
                return downloadFiles;
            }
        }

        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <param name="objPath">目标ftp路径（不填为根路径）</param>
        /// <param name="file">目前ftp文件</param>
        /// <returns>文件大小</returns>
        public int GetFileSize(string file, string objPath = "")
        {
            StringBuilder result = new StringBuilder();
            FtpWebRequest request;
            try
            {
                request = (FtpWebRequest)FtpWebRequest.Create(new Uri(_path + objPath + file));
                request.UseBinary = true;
                request.Credentials = new NetworkCredential(_userName, _password);//设置用户名和密码
                request.Method = WebRequestMethods.Ftp.GetFileSize;

                int dataLength = (int)request.GetResponse().ContentLength;

                return dataLength;
            }
            catch (Exception ex)
            {
                Epoint.Frame.Common.LogOperate.WriteLog_ToFileName("FtpHelper_" + DateTime.Now.ToString("yyyyMMdd") + ".log", "GetFileSize：" + ex.ToString());
                return -1;
            }
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="filePath">源文件路径（路径+文件名）</param>
        /// <param name="objPath">目标ftp文件夹路径</param>
        public bool FileUpLoad(string filePath, string objPath = "")
        {
            FtpWebRequest reqFTP = null;
            //待上传的文件 （全路径）
            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                using (FileStream fs = fileInfo.OpenRead())
                {
                    long length = fs.Length;
                    reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(_path + objPath + fileInfo.Name));

                    //设置连接到FTP的帐号密码
                    reqFTP.Credentials = new NetworkCredential(_userName, _password);
                    //设置请求完成后是否保持连接
                    reqFTP.KeepAlive = false;
                    //指定执行命令
                    reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
                    //指定数据传输类型
                    reqFTP.UseBinary = true;

                    using (Stream stream = reqFTP.GetRequestStream())
                    {
                        //设置缓冲大小
                        int BufferLength = 5120;
                        byte[] b = new byte[BufferLength];
                        int i;
                        while ((i = fs.Read(b, 0, BufferLength)) > 0)
                        {
                            stream.Write(b, 0, i);
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Epoint.Frame.Common.LogOperate.WriteLog_ToFileName("FtpHelper_" + DateTime.Now.ToString("yyyyMMdd") + ".log", "FileUpLoad：" + ex.ToString());

                return false;
            }
        }

        /// <summary>
        /// 删除ftp文件
        /// </summary>
        /// <param name="objPath">目标ftp文件夹路径</param>
        /// <param name="file">目标ftp文件</param>
        public bool DeleteFile(string objPath, string file)
        {
            try
            {
                string uri = _path + objPath + file;
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                // 指定数据传输类型
                reqFTP.UseBinary = true;
                // ftp用户名和密码
                reqFTP.Credentials = new NetworkCredential(_userName, _password);
                // 默认为true，连接不会被关闭
                // 在一个命令之后被执行
                reqFTP.KeepAlive = false;
                // 指定执行什么命令
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();

                return true;
            }
            catch (Exception ex)
            {
                Epoint.Frame.Common.LogOperate.WriteLog_ToFileName("FtpHelper_" + DateTime.Now.ToString("yyyyMMdd") + ".log", "DeleteFile：" + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 新建目录 上一级必须先存在
        /// </summary>
        /// <param name="dirName">服务器下的相对路径</param>
        public bool MakeDir(string dirName)
        {
            try
            {
                string uri = _path + dirName;
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                // 指定数据传输类型
                reqFTP.UseBinary = true;
                // ftp用户名和密码
                reqFTP.Credentials = new NetworkCredential(_userName, _password);
                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();

                return true;
            }
            catch (Exception ex)
            {
                Epoint.Frame.Common.LogOperate.WriteLog_ToFileName("FtpHelper_" + DateTime.Now.ToString("yyyyMMdd") + ".log", "MakeDir：" + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 删除目录 上一级必须先存在
        /// </summary>
        /// <param name="dirName">服务器下的相对路径</param>
        public bool DelDir(string dirName)
        {
            try
            {
                string uri = _path + dirName;
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                // ftp用户名和密码
                reqFTP.Credentials = new NetworkCredential(_userName, _password);
                reqFTP.Method = WebRequestMethods.Ftp.RemoveDirectory;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();

                return true;
            }
            catch (Exception ex)
            {
                Epoint.Frame.Common.LogOperate.WriteLog_ToFileName("FtpHelper_" + DateTime.Now.ToString("yyyyMMdd") + ".log", "DelDir：" + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 从ftp服务器上获得文件夹列表
        /// </summary>
        /// <param name="RequedstPath">服务器下的相对路径</param>
        /// <returns></returns>
        public List<string> GetDirctory(string reequedstPath)
        {
            List<string> strs = new List<string>();
            try
            {
                string uri = _path + reequedstPath;//目标路径_path为服务器地址
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                // ftp用户名和密码
                reqFTP.Credentials = new NetworkCredential(_userName, _password);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());//中文文件名

                string line = reader.ReadLine();
                while (line != null)
                {
                    if (line.Contains("<DIR>"))
                    {
                        string msg = line.Substring(line.LastIndexOf("<DIR>") + 5).Trim();
                        strs.Add(msg);
                    }
                    line = reader.ReadLine();
                }
                reader.Close();
                response.Close();
                return strs;
            }
            catch (Exception ex)
            {
                Epoint.Frame.Common.LogOperate.WriteLog_ToFileName("FtpHelper_" + DateTime.Now.ToString("yyyyMMdd") + ".log", "GetDirctory：" + ex.ToString());
            }
            return strs;
        }

        /// <summary>
        /// 从ftp服务器上获得文件列表
        /// </summary>
        /// <param name="RequedstPath">服务器下的相对路径</param>
        /// <returns></returns>
        public List<string> GetFile(string RequedstPath)
        {
            List<string> strs = new List<string>();
            try
            {
                string uri = _path + RequedstPath;//目标路径 path为服务器地址
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                // ftp用户名和密码
                reqFTP.Credentials = new NetworkCredential(_userName, _password);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());//中文文件名

                string line = reader.ReadLine();
                while (line != null)
                {
                    if (!line.Contains("<DIR>"))
                    {
                        string msg = line.Substring(39).Trim();
                        strs.Add(msg);
                    }
                    line = reader.ReadLine();
                }
                reader.Close();
                response.Close();
                return strs;
            }
            catch (Exception ex)
            {
                Epoint.Frame.Common.LogOperate.WriteLog_ToFileName("FtpHelper_" + DateTime.Now.ToString("yyyyMMdd") + ".log", "GetFile：" + ex.ToString());
            }
            return strs;
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string Download(string objPath, string file, string downPath)
        {
            FtpWebRequest reqFTP;
            try
            {
                string filePath = System.Web.HttpContext.Current.Server.MapPath(@"~\LogFiles\Download\" + downPath);
                if (!System.IO.Directory.Exists(filePath))
                {
                    System.IO.Directory.CreateDirectory(filePath);//创建文件目录
                }
                FileStream outputStream = new FileStream(filePath + file, FileMode.Create);
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(_path + objPath + file));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(_userName, _password);
                reqFTP.UsePassive = false;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];
                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
                ftpStream.Close();
                outputStream.Close();
                response.Close();

                return filePath + file;
            }
            catch (Exception ex)
            {
                Epoint.Frame.Common.LogOperate.WriteLog_ToFileName("FtpHelper_" + DateTime.Now.ToString("yyyyMMdd") + ".log", "Download：" + ex.ToString());
                return "";
            }
        }
    }
}
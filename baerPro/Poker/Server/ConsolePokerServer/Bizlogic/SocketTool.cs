using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConsolePokerServer.Bizlogic;
using Newtonsoft.Json;
using Poker.Bizlogic.Models;
using System.Configuration;

namespace ConsolePokerServer.Bizlogic
{
    public class SocketTool
    {
        // 创建一个和客户端通信的套接字
        public static Socket listenSocket = null;
        //定义一个集合，存储客户端信息
        //public static List<Socket> listClient = new List<Socket>();

        public static void OnStart()
        {
            int port = Convert.ToInt16(ConfigurationManager.AppSettings["Port"] ?? "9090");
            string host = ConfigurationManager.AppSettings["Host"] ?? "127.0.0.1";

            ///创建终结点（EndPoint）
            IPAddress ip = IPAddress.Parse(host);//把ip地址字符串转换为IPAddress类型的实例
            IPEndPoint ipe = new IPEndPoint(ip, port);//用指定的端口和ip初始化IPEndPoint类的新实例

            ///创建socket并开始监听
            listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建一个socket对像，如果用udp协议，则要用SocketType.Dgram类型的套接字
            listenSocket.Bind(ipe);//绑定EndPoint对像（9090端口和ip地址）
            listenSocket.Listen(0);//开始监听

            //负责监听客户端的线程:创建一个监听线程  
            Thread threadListen = new Thread(ListenConnecting);

            //将窗体线程设置为与后台同步，随着主线程结束而结束  
            threadListen.IsBackground = true;

            //启动线程     
            threadListen.Start();

            Console.WriteLine("开启监听...");
            Console.Read();
        }

        public static void ListenConnecting()
        {
            Socket connection = null;
            Tran tran = new Tran();

            //持续不断监听客户端发来的请求     
            while (true)
            {
                if (GameTool.Gamers.Count >= 3)
                {
                    tran = new Tran()
                    {
                        type = 1,
                        msg = "游戏即将开始！\r\n"
                    };
                    Common.SendAllGamer(tran);
                    break;
                }
                else
                {
                    #region 继续等待客户端加入
                    try
                    {
                        connection = listenSocket.Accept();
                    }
                    catch (Exception ex)
                    {
                        //提示套接字监听异常     
                        Console.WriteLine(ex.Message);
                        break;
                    }

                    //反馈客户端
                    tran = new Tran()
                    {
                        type = 1,
                        msg = "加入游戏成功！\r\n"
                    };
                    connection.Send(Poker.Bizlogic.Common.GetSendBytes(tran));

                    //显示与客户端连接情况
                    Console.WriteLine("玩家[" + Common.GetRemotePoat(connection) + "]已加入！\t\n");
                    //添加客户端信息 
                    Gamer gamer = new Gamer();
                    gamer.name = Common.GetRemotePoat(connection);
                    gamer.socket = connection;
                    GameTool.Gamers.Add(gamer);
                    #endregion
                }
            }

            //游戏开始
            GameTool.GameStart();
        }
        
        /// <summary>
        /// 接收客户端发来的信息，客户端套接字对象
        /// </summary>
        /// <param name="socketclientpara"></param>    
        public static void Recv(object socketclientpara)
        {
            Socket socketServer = socketclientpara as Socket;

            while (true)
            {
                //创建一个内存缓冲区，其大小为1024*1024字节  即1M     
                byte[] byteRec = new byte[1024 * 1024];
                //将接收到的信息存入到内存缓冲区，并返回其字节数组的长度    
                try
                {
                    int length = socketServer.Receive(byteRec);
                    string strRec = Encoding.Default.GetString(byteRec, 0, length);

                    //将发送的字符串信息附加到文本框txtMsg上     
                    Console.WriteLine("玩家[" + Common.GetRemotePoat(socketServer) + "]:" + strRec + "\r\n");
                    GameTool.GameLogic(strRec);
                }
                catch (Exception ex)
                {
                    //提示套接字监听异常  
                    Console.WriteLine("客户端" + socketServer.RemoteEndPoint + "已经中断连接" + "\r\n" + ex.Message + "\r\n" + ex.StackTrace + "\r\n");
                    //关闭之前accept出来的和客户端进行通信的套接字 
                    socketServer.Close();
                    break;
                }
            }
        }
    }
}

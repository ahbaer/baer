using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using FromPokerClient.Bizlogic;
using Poker.Bizlogic.Models;
using System.Configuration;
using System.Media;
using FormPokerClient.Bizlogic;

namespace FromPokerClient
{
    public partial class Client : Form
    {
        //配置项
        private static bool IsOpenbgMisic = (ConfigurationManager.AppSettings["IsOpenbgMisic"] ?? "1").Equals("1");
        //创建 1个客户端套接字 和1个负责监听服务端请求的线程  
        Thread threadclient = null;
        Thread musicThread = null;
        Socket socketclient = null;
        PictureBox[] gamerBoxs = null;//玩家（展示）
        PictureBox[] landlordCardBoxs = null;//地主牌（展示）
        List<PictureBox> myCardBoxs = new List<PictureBox>();//玩家0的牌（当前玩家）
        List<PictureBox> sendCardBoxs = new List<PictureBox>();//玩家0的牌
        List<Card> lastSendCards = new List<Card>();//上一轮发送的牌
        List<Card> sendCards = new List<Card>();//本轮选中待发送的牌
        Gamer gamer = new Gamer() { name = "张三", photoUrl = "" };

        int moveY = 20;
        public Point myPokerPoint = new Point(360, 290);
        public Point sendPokerPoint = new Point(372, 140);

        #region 定时器
        private System.Timers.Timer tipTimer = new System.Timers.Timer();
        //private System.Timers.Timer clockTimer = new System.Timers.Timer(1000);

        private void btnTipClose_Click(object sender, EventArgs e)
        {
            if (lblTip.InvokeRequired)
            {
                Action<bool> actionDelegate = (tip) =>
                {
                    lblTip.Visible = tip;
                };
                lblTip.Invoke(actionDelegate, false);
            };

            tipTimer.Stop();
        }

        //private void btnCloseShow_Click(object sender, EventArgs e)
        //{
        //    if (BTNClock.InvokeRequired)
        //    {
        //        Action<string> actionDelegate = (text) =>
        //        {
        //            BTNClock.Text = text;
        //            BTNClock.Visible = true;
        //        };
        //        lblTip.Invoke(actionDelegate, Countdown.ToString());
        //    };

        //    Countdown--;
        //    if (Countdown == -2)
        //    {
        //        if (BTNClock.InvokeRequired)
        //        {
        //            Action<bool> actionDelegate = (bo) =>
        //            {
        //                BTNClock.Visible = bo;
        //            };
        //            lblTip.Invoke(actionDelegate, false);
        //        };
        //        clockTimer.Stop();
        //    }
        //}
        #endregion

        public Client()
        {
            if (IsOpenbgMisic)
            {
                musicThread = new Thread(Player.PlayBackground);
                musicThread.IsBackground = true;
                musicThread.Start();
            }

            InitializeComponent();

            #region 设置背景样式
            this.BackgroundImage = Image.FromFile(Common.GetBackgorundImageUrl("background.jpg"));
            this.BackgroundImageLayout = ImageLayout.Stretch;

            Common.SetContorlStyle(Controls);
            #endregion

            #region 展示牌区及展示玩家
            //玩家
            gamerBoxs = new PictureBox[] { gamerBox0, gamerBox1, gamerBox2 };
            for(int i = 0; i < 3; i++)
            {
                System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                path.AddEllipse(this.gamerBoxs[i].ClientRectangle);
                Region reg = new Region(path);
                gamerBoxs[i].Region = reg;
                gamerBoxs[i].Load(Common.GetPhotoImageUrl("gamer" + i));
                gamerBoxs[i].SizeMode = PictureBoxSizeMode.StretchImage;
            }

            //地主牌
            landlordCardBoxs = new PictureBox[] { landlordCardBox0, landlordCardBox1, landlordCardBox2 };
            foreach (PictureBox picBox in landlordCardBoxs)
            {
                picBox.Visible = false;
                picBox.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            #endregion

            #region 玩家牌初始化
            for(int i = 19; i >= 0; i--)
            {
                //玩家牌
                myCardBoxs.Add(Common.CreatePictureBox(this.Controls, myPokerPoint, new Card(0,0), 19 - i, "myCard"));

                //发送牌
                sendCardBoxs.Add(Common.CreatePictureBox(this.Controls, sendPokerPoint, new Card(0, 0), 19 - i, "sendCard"));
            }

            foreach (PictureBox picBox in myCardBoxs)
            {
                picBox.Click += new EventHandler(btnCard_Click);
            }
            #endregion

            //当选地主
            btnAgree.Visible = btnDisAgree.Visible = false;
            //出牌
            btnNoSend.Visible = btnSend.Visible = false;
        }

        #region socket处理
        private void btnStart_Click(object sender, EventArgs e)
        {
            //Countdown = 30;
            //clockTimer.Elapsed += new System.Timers.ElapsedEventHandler(btnCloseShow_Click);
            //clockTimer.Start();

            //定义一个套接字监听  
            socketclient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            int port = Convert.ToInt16(ConfigurationManager.AppSettings["Port"] ?? "9090");
            string host = ConfigurationManager.AppSettings["Host"] ?? "127.0.0.1";
            //获取文本框中的IP地址  
            IPAddress address = IPAddress.Parse(host);
            //将获取的IP地址和端口号绑定在网络节点上  
            IPEndPoint point = new IPEndPoint(address, port);

            try
            {
                //客户端套接字连接到网络节点上，用的是Connect  
                socketclient.Connect(point);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            threadclient = new Thread(recv);
            threadclient.IsBackground = true;
            threadclient.Start();
        }

        // 接收服务端发来信息的方法    
        public void recv()
        {
            //持续监听服务端发来的消息 
            while (true)
            {
                try
                {
                    //定义一个1M的内存缓冲区，用于临时性存储接收到的消息  
                    byte[] byteRec = new byte[1024 * 1024];

                    //将客户端套接字接收到的数据存入内存缓冲区，并获取长度  
                    int length = socketclient.Receive(byteRec);
                    string strRec = Encoding.Default.GetString(byteRec, 0, length);
                    foreach (string itemRec in strRec.Trim('$').Split('$'))
                    {
                        Tran tran = JsonConvert.DeserializeObject<Tran>(itemRec);
                        GameLogic(tran);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        #endregion

        public void GameLogic(Tran tran)
        {
            switch (tran.type)
            {
                case 1:
                    //地主牌
                    if(tran.landlordCards != null)
                    {
                        for (int i = 0; i < tran.landlordCards.Count; i++)
                        {
                            if (landlordCardBoxs[i].InvokeRequired)
                            {
                                Action<string> actionDelegate = (url) => 
                                {
                                    landlordCardBoxs[i].Visible = true;
                                    landlordCardBoxs[i].Load(url);
                                };
                                landlordCardBoxs[i].Invoke(actionDelegate, Common.GetCardImageUrl(tran.landlordCards[i]));
                            }
                        }
                    }

                    //玩家牌
                    if(tran.cards != null && tran.cards.Count > 0)
                    {
                        gamer.cards = tran.cards;
                        for (int i = 0; i < gamer.cards.Count; i++)
                        {
                            if (myCardBoxs[i].InvokeRequired)
                            {
                                Action<string> actionDelegate = (url) =>
                                {
                                    myCardBoxs[i].Visible = true;
                                    myCardBoxs[i].Load(url);
                                    myCardBoxs[i].Location = Common.GetPokerPoint(myPokerPoint, gamer.cards.Count, i, false);
                                };
                                myCardBoxs[i].Invoke(actionDelegate, Common.GetCardImageUrl(tran.cards[i]));
                            }
                        }
                    }

                    //开始游戏
                    if (btnStart.InvokeRequired)
                    {
                        Action<bool> actionDelegate = (x) => { btnStart.Visible = x; };
                        btnAgree.Invoke(actionDelegate, false);
                    }
                    break;
                case 2:
                    //叫地主
                    if (btnAgree.InvokeRequired)
                    {
                        Action<bool> actionDelegate = (x) => { btnAgree.Visible = x; };
                        btnAgree.Invoke(actionDelegate, true);
                    }
                    //不叫
                    if (btnDisAgree.InvokeRequired)
                    {
                        Action<bool> actionDelegate = (x) => { btnDisAgree.Visible = x; };
                        btnDisAgree.Invoke(actionDelegate, true);
                    }
                    break;
                case 3:
                case 4:
                    sendCards = new List<Card>();
                    //出牌
                    if (tran.indexOpt == 0)//todo
                    {
                        if (btnSend.InvokeRequired)
                        {
                            Action<bool> actionDelegate = (x) => { btnSend.Visible = x; };
                            btnSend.Invoke(actionDelegate, true);
                        }
                        if (btnNoSend.InvokeRequired)
                        {
                            Action<bool> actionDelegate = (x) => { btnNoSend.Visible = x; };
                            btnNoSend.Invoke(actionDelegate, true);
                        }
                    }

                    if (tran.cards != null)
                    {
                        if (tran.type == 3)
                        {
                            gamer.cards = tran.cards;
                            for (int i = 0; i < gamer.cards.Count; i++)
                            {
                                if (myCardBoxs[i].InvokeRequired)
                                {
                                    Action<string> actionDelegate = (url) =>
                                    {
                                        myCardBoxs[i].Visible = true;
                                        myCardBoxs[i].Load(url);
                                        myCardBoxs[i].Location = Common.GetPokerPoint(myPokerPoint, gamer.cards.Count, i, false);
                                    };
                                    myCardBoxs[i].Invoke(actionDelegate, Common.GetCardImageUrl(tran.cards[i]));
                                }
                            }
                        }
                        else
                        {
                            lastSendCards = tran.cards;
                            for (int i = 0; i < lastSendCards.Count; i++)
                            {
                                if (sendCardBoxs[i].InvokeRequired)
                                {
                                    Action<string> actionDelegate = (url) =>
                                    {
                                        sendCardBoxs[i].Visible = true;
                                        sendCardBoxs[i].Load(url);
                                        sendCardBoxs[i].Location = Common.GetPokerPoint(sendPokerPoint, lastSendCards.Count, i, true);
                                    };
                                    sendCardBoxs[i].Invoke(actionDelegate, Common.GetCardImageUrl(lastSendCards[i]));
                                }
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        #region 叫/不叫地主
        /// <summary>
        /// 叫地主
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAgree_Click(object sender, EventArgs e)
        {
            Tran tran = new Tran()
            {
                type = 2,
                landlord = true
            };

            //调用客户端套接字发送字节数组
            byte[] send = Poker.Bizlogic.Common.GetSendBytes(tran);
            socketclient.Send(send);
            btnAgree.Visible = btnDisAgree.Visible = false;
        }

        /// <summary>
        /// 不叫
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDisAgree_Click(object sender, EventArgs e)
        {
            Tran tran = new Tran()
            {
                type = 2,
                landlord = false
            };

            //调用客户端套接字发送字节数组     
            socketclient.Send(Poker.Bizlogic.Common.GetSendBytes(tran));
            btnAgree.Visible = btnDisAgree.Visible = false;
        }
        #endregion

        #region 出/不出牌
        /// <summary>
        /// 出牌
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            Tran tran = new Tran()
            {
                type = 4,
                cards = sendCards
            };

            if (!Poker.Bizlogic.GameTool.IsRuleCards(sendCards, lastSendCards, gamer.cards))
            {
                #region 不符合出牌规则定时器
                lblTip.Visible = true;
                tipTimer = new System.Timers.Timer(3000);
                tipTimer.Elapsed += new System.Timers.ElapsedEventHandler(btnTipClose_Click);
                tipTimer.Start();
                #endregion

                return;
            }

            #region 发送牌显示
            sendCards.Sort((x, y) => y.CompareTo(x));
            for (int i = 0; i < sendCards.Count; i++)
            {
                sendCardBoxs[i].Visible = true;
                sendCardBoxs[i].Location = Common.GetPokerPoint(sendPokerPoint, sendCards.Count, i, true);
                sendCardBoxs[i].Load(Common.GetCardImageUrl(sendCards[i]));
            }
            for(int i = sendCards.Count; i < sendCardBoxs.Count; i++)
            {
                sendCardBoxs[i].Visible = false;
            }
            #endregion

            gamer.cards.Sort((x, y) => y.CompareTo(x));
            for (int index = 0, sendIndex = 0; index < gamer.cards.Count; index++)
            {
                if (sendIndex < sendCards.Count && gamer.cards[index].CompareTo(sendCards[sendIndex]) == 0)
                {
                    gamer.cards.RemoveAt(index);
                    sendIndex++;
                    index--;
                }
            }

            #region 当前玩家牌面显示
            gamer.cards.Reverse();
            for (int i = 0; i < gamer.cards.Count; i++)
            {
                myCardBoxs[i].Load(Common.GetCardImageUrl(gamer.cards[i]));
                myCardBoxs[i].Location = Common.GetPokerPoint(myPokerPoint, gamer.cards.Count, i, false);
            }
            for(int i = gamer.cards.Count; i < myCardBoxs.Count; i++)
            {
                myCardBoxs[i].Visible = false;
            }
            #endregion

            //调用客户端套接字发送字节数组     
            socketclient.Send(Poker.Bizlogic.Common.GetSendBytes(tran));
            sendCards = new List<Card>();
            btnNoSend.Visible = btnSend.Visible = false;
        }

        private void btnNoSend_Click(object sender, EventArgs e)
        {
            Tran tran = new Tran()
            {
                type = 4,
                cards = sendCards
            };

            for (int i = 0; i < sendCardBoxs.Count; i++)
            {
                sendCardBoxs[i].Visible = false;
            }

            //调用客户端套接字发送字节数组     
            socketclient.Send(Poker.Bizlogic.Common.GetSendBytes(tran));
            sendCards = new List<Card>();
            btnNoSend.Visible = btnSend.Visible = false;
        }
        #endregion

        /// <summary>
        /// 点击牌触发动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCard_Click(object sender, EventArgs e)
        {
            int index = 0;
            PictureBox pictureBox = GetPictureBox(((PictureBox)sender).Name, out index);
            if (pictureBox.Location.Y == myPokerPoint.Y)
            {
                pictureBox.Location = new Point(pictureBox.Location.X, pictureBox.Location.Y - moveY);
                sendCards.Add(Common.GetCardByPictureBox(pictureBox));
            }
            else
            {
                pictureBox.Location = new Point(pictureBox.Location.X, pictureBox.Location.Y + moveY);
                Card card = Common.GetCardByPictureBox(pictureBox);
                sendCards.Remove(sendCards.FirstOrDefault(t => t.color == card.color && t.point == card.point));
            }
        }

        /// <summary>
        /// 获取当前触发的PictureBox
        /// </summary>
        /// <param name="name"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public PictureBox GetPictureBox(string name, out int index)
        {
            index = Convert.ToInt16(name.Substring(20, name.Length - 20));
            return myCardBoxs[index];
        }
    }
}

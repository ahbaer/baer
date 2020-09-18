using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Poker.Bizlogic;
using Poker.Bizlogic.Models;

namespace ConsolePokerServer.Bizlogic
{
    public class Common
    {
        public static string GetRemotePoat(int polling)
        {
            return ((IPEndPoint)GameTool.Gamers[polling].socket.RemoteEndPoint).Port.ToString();
        }

        public static string GetRemotePoat(Socket socket)
        {
            return ((IPEndPoint)socket.RemoteEndPoint).Port.ToString();
        }

        public static void SendAllGamer(Tran tran)
        {
            foreach (Gamer gamer in GameTool.Gamers)
            {
                gamer.socket.Send(Poker.Bizlogic.Common.GetSendBytes(tran));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="f_Tran">农民返回</param>
        /// <param name="l_Tran">地主返回</param>
        public static void SendAllGamer(Tran f_Tran, Tran l_Tran)
        {
            foreach (Gamer gamer in GameTool.Gamers)
            {
                if (gamer.landlord)
                {
                    gamer.socket.Send(Poker.Bizlogic.Common.GetSendBytes(l_Tran));
                }
                else
                {
                    gamer.socket.Send(Poker.Bizlogic.Common.GetSendBytes(f_Tran));
                }
            }
        }
    }
}

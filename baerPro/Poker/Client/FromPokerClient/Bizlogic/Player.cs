using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace FormPokerClient.Bizlogic
{
    public class Player
    {
        public static void PlayBackground()
        {
            //背景音乐
            SoundPlayer player = new SoundPlayer();
            while (true)
            {
                player.SoundLocation = @"..\..\Music\background.wav";
                player.LoadAsync();
                player.PlaySync();
            }
        }
    }
}

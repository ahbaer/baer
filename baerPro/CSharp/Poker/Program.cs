using Poker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    class Program
    {
        static void Main(string[] args)
        {
            Control ctrl = new Control();
            ctrl.playerJoin();

            Console.ReadLine();
        }
    }
}

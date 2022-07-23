using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Player
    {
        private string name;    //player name
        private int count;      //プレイヤーの駒の数

        //プレイヤー名入力(初期化)
        public Player(string name)
        {
            this.name = name;
        }

        //駒を置く座標を指定するメソッド
        public void point(ref int a,ref int n)
        {
            Console.WriteLine("配置場所を指定");
            Console.Write("行");
            a = int.Parse(Console.ReadLine()) - 1;
            Console.Write("列");
            n = int.Parse(Console.ReadLine()) - 1;
        }
    }
}

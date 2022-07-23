using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        //先攻後攻を決めるメソッド
        static void Wfirst(int n)
        {
            Random rand = new Random();
            n = rand.Next(0, 1);
        }

        //メインプログラム
        static void Main(string[] args)
        {
            //プレイヤーの名前入力
            Player[] players = new Player[2];
            string[] names = new string[2];
            Console.WriteLine("Player1.\n名前を入力してください");
            names[0] = Console.ReadLine();
            Console.WriteLine("Player2.\n名前を入力してください");
            names[1] = Console.ReadLine();
            players[0] = new Player(names[0]);
            players[1] = new Player(names[1]);

            //盤面初期化
            Bord bord = new Bord();
            bord.def_set();
            
            int f = 0;
            //先攻後攻を決める
            Wfirst(f);

            //ゲーム進行部
            while(bord.end == false)
            {
                int a = 0;  //行指定
                int n = 0;  //列指定
                bool tf = false;

                //最新の盤面と誰のターンかを表示
                bord.display();
                Console.WriteLine(names[f%2] + "さんのターン");
                if (f % 2 == 0)
                {
                    Console.WriteLine("●");
                }
                else
                {
                    Console.WriteLine("〇");
                }

                //駒の配置場所を決めてもらう
                while (true)
                {
                    players[f % 2].point(ref a, ref n);
                    bord.put(ref a, ref n, ref f,ref tf);
                    if (tf == true)
                    {
                        break;
                    }
                }
                f++;
            }
        }
    }
}

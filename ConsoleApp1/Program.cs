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
            players[1] = new Player(names[1]);  //プレイヤーのインスタンスを作成

            //盤面初期化
            Bord bord = new Bord();
            bord.def_set();
            
            int f = 0;
            //どちらが先攻を決める
            Wfirst(f);

            //ゲーム進行部
            while(bord.end == false)
            {
                int a = 0;  //行指定用変数
                int n = 0;  //列指定用変数
                bool tf = false;  //putメソッド用の配置し、ひっくり返しが完了した場合trueにする変数
                bool putable = false;   //sertchメソッド用の配置可能なマスが存在する場合trueにする変数

                //最新の盤面と誰のターンかを表示
                bord.display();
                Console.WriteLine(names[f%2] + "さんのターン");
                if (f % 2 == 0)
                {
                    Console.WriteLine("〇");
                }
                else
                {
                    Console.WriteLine("●");
                }

                //駒の配置場所を決めてもらう
                while (true)
                {
                    bord.sertch(ref f,ref putable);
                    if (putable == true)
                    {
                        players[f % 2].disable = false; //設置可能である

                        bool error = true; //エラーが発生する場合はtrueとし、正常に動作した場合にfalseを代入しループを抜ける
                        while (error == true)
                        {
                            try
                            {
                                players[f % 2].point(ref a, ref n);
                                error = false;
                            }
                            catch (System.FormatException)      //数値でない値を入れた場合の例外処理
                            {
                                Console.WriteLine("不正な値です\n");
                            }
                            catch (IndexOutOfRangeException)    //盤面に設定されてない範囲の座標を入力した場合の例外処理
                            {
                                Console.WriteLine("座標が盤面の範囲外です\n");
                            }
                        }
                        bord.put(ref a, ref n, ref f, ref tf);
                        if (tf == true)
                        {
                            break;      //指定したマスが設置可能で、設置とひっくり返しが完了したときtrueとし、ループを抜けてターンチェンジ
                        }
                    }
                    else
                    {
                        players[f % 2].disable = true;  //設置不可である→ループを抜けてターンチェンジ
                        Console.WriteLine("設置可能なマスが無いためパスされました");
                        break;
                    }
                }
                if (players[0].disable == true && players[1].disable == true)
                {
                    bord.end = true;
                }

                f++;        //操作プレイヤーを入れ替える
            }
            f = 0;
            bord.counter(ref f,ref players[f % 2].count);
            f++;
            bord.counter(ref f, ref players[f % 2].count); //プレイヤー１，２それぞれの駒の数を数える

            if (players[0].count > players[1].count)
            {
                Console.WriteLine(names[0] + "さんの勝利");
            }
            else if(players[0].count < players[1].count)
            {
                Console.WriteLine(names[1] + "さんの勝利");
            }
            else
            {
                Console.WriteLine("引き分け");
            }
        }
    }
}

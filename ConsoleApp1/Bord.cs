using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Bord
    {
        private int[,] bord = new int[8,8];  //盤面を設定
        public bool end = false;            //ゲーム終了を判定する変数
        
        /*盤面の初期化
            2:そのマスに何も置かれていない
            0:そのマスに白が置かれている
            1:そのマスに黒が置かれている*/
        public void def_set()
        {
            for (int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    this.bord[i,j] = 2;
                }
            }
            this.bord[3, 3] = 0;
            this.bord[4, 4] = 0;
            this.bord[3, 4] = 1;
            this.bord[4, 3] = 1;
        }

        //盤面の表示
        public void display()
        {
            for(int i = -1; i <= 7; i++)
            {
                for(int j = -1; j <= 7; j++)
                {
                    if (i == -1)
                    {
                        switch (j)
                        {
                            case -1:
                                Console.Write("  |");
                                break;
                            default:
                                Console.Write(" " + (j + 1) + "|");
                                break;
                        }
                    }
                    else if (j == -1)
                    {
                        Console.Write(" " + (i + 1) + "|");
                    }
                    //↑行と列の情報を表示
                    else
                    {
                        if (this.bord[i, j] == 2)
                        {
                            Console.Write("  |");
                        }
                        else if (this.bord[i, j] == 0)
                        {
                            Console.Write("〇|");
                        }
                        else
                        {
                            Console.Write("●|");
                        }
                    }
                }
                Console.Write("\n");
                Console.WriteLine("---------------------------");
            }
        }

        //盤面に置けるマスがない場合、終了を判別するend変数をtrueにする
        public void end_serch()
        {
            for(int i = 0; i <= 7; i++)
            {
                for(int j = 0; j <= 7; j++)
                {
                    if(this.bord[i, j] == 0)
                    {
                        this.end = true;
                    }
                }
            }
        }

        private int i1; //行の代入用
        private int j1; //列の代入用
        public int I1   //行の制約
        {
            set
            {
                this.i1 = value;
                if (this.i1 < 0)
                {
                    this.i1 = 0;
                }
                else if (this.i1 >= 8)
                {
                    this.i1 = 7;
                }
            }
            get
            {
                return i1;
            }
        }
        public int J1   //列の制約
        {
            set 
            {
                this.j1 = value;
                if(this.j1 < 0)
                {
                    this.J1 = 0;
                }else if (this.j1 >= 8)
                {
                    this.J1= 7;
                }
            }
            get
            {
                return j1;
            }
        }

        //配置可能であれば、マスに配置し、ひっくり返すメソッド
        public void put(ref int a, ref int n, ref int f, ref bool tf)
        {
            tf = false;

            if (bord[a, n] == 2)        //前提としてマスが空白(2)でないといけない
            {
                for (int p = 0; p < 8; p++)
                    {
                    /*
                     -1,-1| -1, 0| -1, 1
                    ______|______|______
                      0,-1|  0, 0|  0, 1
                    ______|______|______
                      1,-1|  1, 0|  1, 1

                     0,0が置きたいマスで、p=0のとき右(0,1)で時計回りに位置を変え、検証していく*/

                    int ip = 0;
                    int jp = 0;
                    switch (p)
                        {
                            case 0:
                                ip = 0;
                                jp = 1;
                                break;
                            case 1:
                                ip = 1;
                                jp = 1;
                                break;
                            case 2:
                                ip = 1;
                                jp = 0;
                                break;
                            case 3:
                                ip = 1;
                                jp = -1;
                                break;
                            case 4:
                                ip = 0;
                                jp = -1;
                                break;
                            case 5:
                                ip = -1;
                                jp = -1;
                                break;
                            case 6:
                                ip = -1;
                                jp = 0;
                                break;
                            case 7:
                                ip = -1;
                                jp = 1;
                                break;
                        }

                    this.I1 = a + ip;
                    this.J1 = n + jp;
                    if (bord[i1, j1] == (f + 1) % 2)   //ｐの位置に相手の駒がある時 
                    {
                        for (int i = 2; i <= 7; i++)
                        {
                            this.I1 = a + (i * ip);
                            this.J1 = n + (i * jp);
                            if(bord[i1, j1] == 2) //空きのマス(2)があった場合は置けないので即ブレイクする
                            {
                                break;
                            }
                            else if (a + (i * ip) >= 0 && a + (i * ip) < 8 && n + (i * jp) >= 0 && n + (i * jp) < 8)
                            {
                                //調べたい値が配列の定義範囲を超えない場合、実行
                                
                                if (bord[i1, j1] == f % 2)   //i個(i >= 2)先に自分の駒があるとき
                                {                
                                    for (int j = 0; j <= i; j++)
                                    {
                                        this.bord[a + (j * ip), n + (j * jp)] = f % 2;    //置きたい位置に設置、ひっくり返す
                                    }
                                    tf = true;
                                    break;
                                }
                                else if (p == 7 && i == 7)
                                {
                                    Console.WriteLine("そこには置けません");
                                }
                            }
                        }
                    }
                    else if (p == 7)
                    {
                        Console.WriteLine("そこには置けません");
                    }
                }
            }
            else
            {
                Console.WriteLine("そこには置けません");
            }                       
        }

        /*配置可能なマスが存在するかどうかの調査するメソッド*/
        public void sertch(ref int f,ref bool putable)
        {
            putable = false;
            for (int a = 0; a <= 7; a++)
            {
                for (int n = 0; n <= 7; n++)
                {

                    if (bord[a, n] == 2)        //前提としてマスが空白(2)でないといけない
                    {
                        for (int p = 0; p < 8; p++)
                        {
                            int ip = 0;
                            int jp = 0;
                            switch (p)
                            {
                                case 0:
                                    ip = 0;
                                    jp = 1;
                                    break;
                                case 1:
                                    ip = 1;
                                    jp = 1;
                                    break;
                                case 2:
                                    ip = 1;
                                    jp = 0;
                                    break;
                                case 3:
                                    ip = 1;
                                    jp = -1;
                                    break;
                                case 4:
                                    ip = 0;
                                    jp = -1;
                                    break;
                                case 5:
                                    ip = -1;
                                    jp = -1;
                                    break;
                                case 6:
                                    ip = -1;
                                    jp = 0;
                                    break;
                                case 7:
                                    ip = -1;
                                    jp = 1;
                                    break;
                            }           //putメソッドと同じ手法で配置可能か調査

                            this.I1 = a + ip;
                            this.J1 = n + jp;
                            if (bord[i1, j1] == (f + 1) % 2)   //ｐの位置に相手の駒がある時 
                            {
                                for (int i = 2; i <= 7; i++)
                                {
                                    this.I1 = a + (i * ip);
                                    this.J1 = n + (i * jp);
                                    if(bord[i1, j1] == 2)
                                    {
                                        break;
                                    }
                                    else if (a + (i * ip) >= 0 && a + (i * ip) < 8 && n + (i * jp) >= 0 && n + (i * jp) < 8)
                                    {
                                        
                                        if (bord[i1, j1] == f % 2)   //i個(i >= 2)先に自分の駒があるとき
                                        {
                                            putable = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //駒数を数えるメソッド
        public void counter(ref int f,ref int count)
        {
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    if (this.bord[i, j] == f % 2)
                    {
                        count++;
                    }
                }
            }
        }

    }
}

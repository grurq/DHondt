using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;


internal class Program
{
    public struct Votequotient
    {
            public int id;
            public int quotient; //商
            public int remainder; //余り
    };
    
    public static void Main(string[] args)
    {
        int seats = 0;
        int input = 0;
        List<int> votes = new List<int>(); //政党ごとの得票数  
        List<Votequotient> votecells= new List<Votequotient>(); //id,商、余りを入れる構造体のリスト
        int[] wonseats;
        Console.WriteLine("ドント式比例代表制プログラム");
        for( ; ; ){
            seats = Program.GetValue("議席定数を入れてください");
            if (seats > 0) break;
        }
        for (; ; )
        {
            votes.Add(Program.GetValue("政党の得票を入れてください。\r\n※0で入力終了または削除　政党ID:" + (votes.Count + 1).ToString()));
            if (votes[votes.Count - 1] == 0)
            {
                votes.RemoveAt(votes.Count - 1);
                do
                {
                    input = Program.GetValue("次の数からコマンドを選んでください\r\n 1:一つ前の要素を削除 2:入力終了 0:入力続行");
                    switch (input)
                    {
                        case 1:
                            if (votes.Count < 1)
                            {
                                Console.WriteLine("削除できる要素がありません");
                                break;
                            }
                            votes.RemoveAt(votes.Count - 1);
                            break;
                        case 2:
                            input = -1;
                            break;
                        case 0:
                            break;
                        default:
                            input = -2;
                    break;

                    }
                } while (input == -2);
            }
            if (input == -1) break;
        }
        for (int i=1;i<=seats;i++)
        {
            for(int j=0;j<votes.Count;j++)
            {
                    Votequotient cells= new Votequotient();
                    cells.id=j;
                    cells.quotient=votes[j]/i;
                    cells.remainder=votes[j]%i;
                    votecells.Add(cells);
            }
        }
        votecells.Sort((a, b) => a.quotient == b.quotient ? a.remainder == b.remainder ? a.id - b.id : b.remainder - a.remainder : b.quotient - a.quotient);
        wonseats=new int[votes.Count];
        for(int i=0;i<seats;i++)
        {
            wonseats[votecells[i].id]++;
        }
        do
        {
            input=GetValue("結果表示を選択してください\r\n0:画面表示　1:カレントディレクトリにresult.csvを作成");
            switch (input)
            {
                case 0:
                for(int i=0;i<votes.Count;i++)
                {
                    Console.WriteLine("政党ID:{0}　得票数:{1}　獲得議席:{2}",i+1,votes[i],wonseats[i]);
                }
                Console.WriteLine("議員定数:{0}　以上",seats);
                input=0;
                break;
                case 1:
                string filename= "result.csv";
                FileStream fs=new FileStream(filename,FileMode.Create,FileAccess.ReadWrite);
                StreamWriter sw=new StreamWriter(fs);
                sw.WriteLine("議員定数");
                sw.WriteLine(seats);
                sw.WriteLine("政党ID,得票数,獲得議席");
                for(int i=0;i<votes.Count;i++)
                {
                    sw.WriteLine((i+1)+","+votes[i]+","+wonseats[i]);
                }
                sw.Close();
                input=1;
                Console.WriteLine("ファイル出力が終了しました。");
                break;
                default:
                input = -1;
                break;

            }
            

        }while(input==-1);//vote入力時のデフォルト値
        
        Console.ReadKey();
    }
    public static int GetValue(string command)
        {
            var s="";
            int Value;
            Console.WriteLine(command);
        Console.Write(">");
            for (;;)
            {
                s = Console.ReadLine();
                if (int.TryParse(s,out Value))
                {
                    if (Value >= 0)
                    {
                        return Value;
                    }
                    else
                    {
                        Console.WriteLine("負の値は無効です。");
                    }
                }
                Console.Write("整数の値を入力してください\r\n>");
            }

        }

    }

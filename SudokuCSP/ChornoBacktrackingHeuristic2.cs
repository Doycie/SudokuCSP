using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuCSP
{
    class ChornoBacktrackingHeuristic2 : SudokuSolver
    {


        List<Tuple<int,int>> CSPOrderList;
        public override void solve()
        {
            CSPOrderList = new List<Tuple<int, int>>(N*N+1);
            for (int i = 0; i < N *N; i++)
            {
                
               // print(i / N, i % N);
               // Console.WriteLine(checkCount(i));
              //  Console.ReadLine();
                CSPOrderList.Add( Tuple.Create(i,checkCount(i)));
            }

            CSPOrderList.Sort((x, y) =>y.Item2.CompareTo(x.Item2));
            print();
            solveRec(0);
          //  Console.WriteLine("Score: " + Evaluation() + " in " + it + " iterations");
            //print();
        }

 

        public bool solveRec(int startN)
        {
            System.Threading.Thread.Sleep(20);

            if (startN == N * N)
            {
                return true;
            }
            //Console.ReadLine();
           // Console.Clear();
            it++;
            int start = CSPOrderList[startN].Item1;
            //Console.WriteLine(startN + " " + start + " " + CSPOrderList[startN].Item2);
            //print(start / N, start % N);

            if (board[start] == 0)
            {
                for (int i = 0; i < N+1;i++)
                {
                    if (check(start, i))
                    {
                        board[start] = i;

                        Console.SetCursorPosition(2 + 2 * (start % N) + (start % N / 3) * 2, (start / N) + 1 + (((start / N) / 3)));
                        Console.Write(i);
                    }

                    else
                    {
                        continue;
                    }
                    bool result = solveRec(startN + 1);
                    if (result == true)
                    {
                        return true;
                    }
                    else
                    {
                        board[start] = 0;
                        Console.SetCursorPosition(2 + 2 * (start % N) + (start % N / 3) * 2, (start / N) + 1 + (((start / N) / 3)));
                        Console.Write(0);
                    }
                }
                if (board[start] == 0)
                {
                    return false;
                }
            }
            else
            {
                bool result = solveRec(startN + 1);
                return result;
            }

            return false;
        }
    }
}

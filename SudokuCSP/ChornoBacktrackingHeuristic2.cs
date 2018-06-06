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

            solveRec(0);
            Console.WriteLine("Score: " + Evaluation() + " in " + it + " iterations");
            print();
        }

        int it = 0;

        public bool solveRec(int startN)
        {


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

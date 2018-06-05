using System;

namespace SudokuCSP
{
    internal class ChronoBacktracking : SudokuSolver
    {
        public override void solve()
        {
            solveRec(0);
            Console.WriteLine("Score: " + Evaluation()+  " in " + it + " iterations");
            print();
        }

        int it = 0;

        public bool solveRec(int start)
        {
            it++;
           // Console.ReadLine();
          //  Console.Clear();
           // Console.WriteLine(start);
           //  print(start/N,start%N);
            if (start == N * N )
            {
                return true;
            }

            if (board[start] == 0)
            {
                for (int i = 1; i < N + 1; i++)
                {
                    if (check(start, i))
                    {
                        board[start] = i;
                    }
                    else
                    {
                        continue;
                    }
                    bool result = solveRec(start + 1);
                    if (result == true)
                    {
                        return true;
                    }
                    else
                    {
                        board[start] = 0;
                    }
                }
                if (board[start] == 0 )
                {
                    return false;
                }
            }
            else
            {
                bool result = solveRec(start + 1);
                return result;
            }

            return false;
        }
    }
}
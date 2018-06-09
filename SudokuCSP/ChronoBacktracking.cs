using System;

namespace SudokuCSP
{
    internal class ChronoBacktracking : SudokuSolver
    {
        public override void solve()
        {
           // print();

            solveRec(0);

        }



        public bool solveRec(int start)
        {
            it++;
           // System.Threading.Thread.Sleep(20);
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
                       // Console.SetCursorPosition(  2 + 2*(start%N) + (start%N/3) * 2 ,  (start / N) + 1 + (((start/N)/3)));
                        //Console.Write(i);
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
                       // Console.SetCursorPosition( 2 + 2 * (start % N) + (start % N / 3) * 2, (start / N) + 1 + (((start / N) / 3)));
                        //Console.Write(0);
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
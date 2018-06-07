using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuCSP
{
    class ChronoBacktrackingHeuristic : SudokuSolver
    {
        int[] CSPOrderList;
        public override void solve()
        {
            Dictionary<int, int> occurrences = new Dictionary<int, int>();
            for(int i = 1; i < N + 1; i++)
            {
                occurrences.Add(i, 0);
            }

            for(int i = 0; i < N * N; i++)
            {
                if (board[i] != 0)
                    occurrences[board[i]] = occurrences[board[i]] + 1;
            }
           
            var CSPOrderListT = occurrences.ToList();
            CSPOrderListT.Sort((x, y) => x.Value.CompareTo(y.Value));
            CSPOrderList = CSPOrderListT.Select(kvp => kvp.Key).ToArray();

            solveRec(0);
            Console.WriteLine("Score: " + Evaluation() + " in " + it + " iterations");
            print();
        }

  

        public bool solveRec(int start)
        {
            it++;
           // Console.ReadLine();
           // Console.Clear();
            print(start/N,start%N);
            

            if (start == N * N)
            {
                return true;
            }

            if (board[start] == 0)
            {
                foreach (int i in CSPOrderList)
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
                if (board[start] == 0)
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

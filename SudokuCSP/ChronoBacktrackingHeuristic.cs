using System;
using System.Collections.Generic;

namespace SudokuCSP
{
    internal class ChronoBacktrackingHeuristic : SudokuSolver
    {
        private List<Tuple<int, int>> CSPOrderList;

        public override void solve()
        {
            //We make a list of tuples of ints to keep track of which square has the most restrictions next. The first element is the position of the block and the second the amount of restrictions
            CSPOrderList = new List<Tuple<int, int>>(N * N + 1);
            for (int i = 0; i < N * N; i++)
            {

                CSPOrderList.Add(Tuple.Create(i, checkCount(i)));
            }

            CSPOrderList.Sort((x, y) => y.Item2.CompareTo(x.Item2));

            solveRec(0);

        }

        public bool solveRec(int startN)
        {
            //If we reached the end stop
            if (startN == N * N)
                return true;

            it++;
            //Get the next item from the sorted list
            int start = CSPOrderList[startN].Item1;

            //If it is a position we can fill in try to fill it in
            if (board[start] == 0)
            {
                //Go over every number to try and fill it in
                for (int i = 0; i < N + 1; i++)
                {
                    //If it is possible go with it and go into recursion for the next, else try the next number
                    if (check(start, i))
                        board[start] = i;
                    else
                        continue;

                    //Return 
                    if (solveRec(startN + 1))
                        return true;
                    else
                        board[start] = 0;

                }
            }
            else
                 return solveRec(startN + 1);
                
            return false;
        }
    }
}
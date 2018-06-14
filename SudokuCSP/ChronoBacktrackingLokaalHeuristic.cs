using System;
using System.Collections.Generic;

namespace SudokuCSP
{
    internal class ChronoBacktrackingLokaalHeuristic : SudokuSolver
    {
        private List<Tuple<int, int>> CSPOrderList;

        public override void solve()
        {
            //We make a list of tuples of ints to keep track of which square has the most restrictions next. The first element is the position of the block and the second the amount of restrictions
            CSPOrderList = new List<Tuple<int, int>>(N * N + 1);

            for (int i = 0; i < N; i++)
            {
                var tempList = new List<Tuple<int, int>>(N);
                for (int j = 0; j < N; j++)
                {
                    tempList.Add(Tuple.Create(((i % 3) * (N * 3)) + ((i / 3) * 3) + ((j % 3) * N) + (j / 3), checkCount(((i % 3) * (N * 3)) + ((i / 3) * 3) + ((j % 3) * N) + (j / 3))));
                }
                tempList.Sort((x, y) => y.Item2.CompareTo(x.Item2));
                CSPOrderList.AddRange(tempList);
            }

            solveRec(0);
        }

        public bool solveRec(int startN)
        {
            //If we reached the end stop
            if (startN == N * N)
                return true;

            //Get the next item from the sorted list
            int start = CSPOrderList[startN].Item1;

            //If it is a position we can fill in try to fill it in
            if (board[start] == 0)
            {
                it++;
                //Go over every number to try and fill it in
                for (int i = 1; i < N + 1; i++)
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
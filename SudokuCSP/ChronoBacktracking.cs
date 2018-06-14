namespace SudokuCSP
{
    internal class ChronoBacktracking : SudokuSolver
    {
        public override void solve()
        {
            solveRec(0);
        }

        //Recursive function for solving the sudoku, the argument is the current position we are trying to fill in
        public bool solveRec(int start)
        {
            //If we are at the end we have found the solution
            if (start == N * N)
                return true;

            //Count the iterations

            //If we can fill in numbers lets try, else just continue to the next square
            if (board[start] == 0)
            {
                it++;
                //Go over every number we can fill in
                for (int i = 1; i < N + 1; i++)
                {
                    //If we can fill in that numbers do it, and continue to the next square. If we cant fill it in continue to the next number to try and fill in.
                    if (check(start, i))
                        board[start] = i;
                    else
                        continue;

                    //If the next thing we try works out till the end return true, we have solvedd the sudoku. Else reset the board
                    if (solveRec(start + 1))
                        return true;
                    else
                        board[start] = 0;
                }
            }
            else
                return solveRec(start + 1);

            return false;
        }
    }
}
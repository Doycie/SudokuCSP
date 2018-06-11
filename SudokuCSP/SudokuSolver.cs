using System;
using System.Diagnostics;

namespace SudokuCSP
{
    internal abstract class SudokuSolver
    {
        protected const int N = 9;
        protected const int B = 3;

        //Local copy of the board we are working on
        protected int[] board;

        //Local copy of the numbers that are fixed for easy lookup
        protected int[] fixedboard;

        protected Stopwatch swatch = new Stopwatch();

        //Initialize the sudoku by setting the size and the local copies of the board
        public void init(int[,] b)
        {
            board = new int[N * N];
            fixedboard = new int[N * N];

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    board[i * N + j] = b[i, j];
                    fixedboard[i * N + j] = b[i, j];
                }
            }
        }

        //Abstract solve function called from program
        public abstract void solve();

        //Print the current board, you can mark two numbers by the argument, they will be colored
        public void print(int markx = -1, int marky = -1, int markx2 = -1, int marky2 = -1)
        {
            Console.WriteLine("-------------------------");
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if ((j) % 3 == 0)
                        Console.Write("| ");
                    if (markx == i && marky == j)
                        Console.ForegroundColor = ConsoleColor.Red;
                    if (markx2 == i && marky2 == j)
                        Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(board[i * N + j] + " ");
                    Console.ResetColor();
                    if (j == N - 1)
                        Console.Write("|");
                }
                Console.WriteLine();
                if ((i) % 3 == 2)
                    Console.WriteLine("-------------------------");
            }
        }

        public int it = 0;

        //Check if a number can be placed at a certain position used in chronological backtracking
        protected bool check(int place, int num)
        {
            int y = place / N;
            int x = place % N;
            
            //Go over the row
            for (int i = 0; i < N; i++)
            {
                if (board[N * y + i] == num)
                    return false;
            }
            //Go over the column
            for (int i = 0; i < N; i++)
            {
                if (board[N * (i) + x] == num)
                    return false;
            }
            //Go over the 3x3 block
            int xb = x / B;
            int yb = y / B;

            for (int i = xb * B; i < xb * B + B; i++)
            {
                for (int j = yb * B; j < yb * B + B; j++)
                {
                    if (num == board[j * N + i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        //Count the amount of restrictions we have, so bassically the unique numbers from a position in its rows columns and block
        protected int checkCount(int place)
        {
            int restrictions = 0;
            int y = place / N;
            int x = place % N;


            //Set bits at the position of the numbers we find in the row
            for (int i = 0; i < N; i++)
            {
                if (board[N * y + i] != 0)
                    restrictions |= (1 << board[N * y + i]);
            }
            //Set bits at the position of the numbers we find in the column
            for (int i = 0; i < N; i++)
            {
                if (board[N * i + x] != 0)
                    restrictions |= (1 << board[N * i + x]);
            }
            //Set bits at the position of the numbers we find in the block
            restrictions |= RestOfBlockCount(x / B, y / B, place);

            //Count all the unique restrictions
            int restrictionCounts = 0;
            for (int i = 1; i < N + 1; i++)
            {
                if (((restrictions >> i) & 1) == 1)
                {
                    restrictionCounts++;
                }
            }
            return restrictionCounts;
        }

        //Helper function for counting the restrictions in a block
        protected int NumberInBlockCount(int xb, int yb)
        {
            int restrictions = 0;
            for (int i = xb * B; i < xb * B + B; i++)
            {
                for (int j = yb * B; j < yb * B + B; j++)
                {
                    if (board[j * N + i] != 0)
                    {
                        restrictions++;
                    }
                }
            }
            return restrictions;
        }

        //Helper function for counting the restrictions in ablock without counting the row and column
        protected int RestOfBlockCount(int xb, int yb, int place)
        {
            int restrictions = 0;
            for (int i = xb * B; i < xb * B + B; i++)
            {
                for (int j = yb * B; j < yb * B + B; j++)
                {
                    int position = j * N + i;
                    if (board[position] != 0 && (position > place + 2 || position < place - 2) && (position % N != place % N))
                    {
                        restrictions |= (1 << board[position]);
                    }
                }
            }
            return restrictions;
        }

        //Method to count the amount of missing numbers in a row, only used in the evaluation function
        private int CountMissingNumbersC(int i, ref int[] board)
        {
            int total = 0;
            int bits = 0;
            //Go over the row and add the numbers to a their coresponding offset in a bit string
            for (int k = 0; k < N; k++)
            {
                bits |= 1 << board[k * N + i];
            }
            //Now count the bitstring bits that are 1
            for (int k = 1; k < N + 1; k++)
            {
                total += (bits >> k) & 1;
            }
            return N - total;
        }

        //Method to count the amount of missing numbers in a column, only used in the evaluation function
        private int CountMissingNumbersR(int i, ref int[] board)
        {
            int total = 0;
            int bits = 0;
            //Go over the column and add the numbers to a their coresponding offset in a bit string
            for (int k = 0; k < N; k++)
            {
                bits |= 1 << board[i * N + k];
            }
            //Now count the bitstring bits that are 1
            for (int k = 1; k < N + 1; k++)
            {
                total += (bits >> k) & 1;
            }
            return N - total;
        }

        //Evaluation function counts how many numbers are missing in each row and column, the lower the better
        protected int Evaluation()
        {
            int total = 0;
            //Count each row
            for (int i = 0; i < N; i++)
            {
                total += CountMissingNumbersR(i, ref board);
            }
            //Count each column
            for (int i = 0; i < N; i++)
            {
                total += CountMissingNumbersC(i, ref board);
            }
            return total;
        }
    }
}
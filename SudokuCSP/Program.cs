using System;
using System.IO;

namespace SudokuCSP
{
    internal class Program
    {
        //Int array to save the original input board
        private static int[,] OriginalSudoku;

        private static void Main(string[] args)
        {


            Console.WriteLine("\t|CB|\t|CBH\t|FC|\t|FCH|");
            
            for (int i = 0; i < 11; i++)
            {
                readBoardFromFile(i);
                SudokuSolver cb = new ChronoBacktracking();
                cb.init(OriginalSudoku);
                cb.solve();
                SudokuSolver cbh = new ChronoBacktrackingHeuristic();
                cbh.init(OriginalSudoku);
                cbh.solve();
                SudokuSolver fc = new ForwardChecking();
                fc.init(OriginalSudoku);
                fc.solve();

                SudokuSolver fch = new ForwardCheckingHeuristic();
                fch.init(OriginalSudoku);
                fch.solve();

                Console.WriteLine(i + ".\t" + cb.it + "\t" + cbh.it + "\t" + fc.it + "\t" + fch.it );
                //Console.WriteLine("CB: " + cb.it + " and with heuristic in: " + cbh.it + " and with forward checking in: " + fc.it +" and with fw heuristic: " + fch.it);
             
            }

            Console.ReadLine();
        }

        //Function to read the board from a text file. Make sure there are spaces at the end
        private static int readBoardFromFile(int a)
        {
            StreamReader sr = new StreamReader("../../data/sudoku_puzzels.txt");

            sr.ReadLine();
            for (int i = 0; i < (a * 10); i++)
            {
                sr.ReadLine();
            }
            string[] input = sr.ReadLine().Split();
            int N = input.Length + 1;

            if (input.Length == 1)
            {
                N = input[0].Length;

                OriginalSudoku = new int[N, N];
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        int k = (int)(input[0][j]) - '0';
                        OriginalSudoku[i, j] = k;
                    }

                    input = sr.ReadLine().Split();
                }
            }
            return N;
        }

        //Read the board from the Console by manualling copying
        private static int readBoard()
        {
            //Read the board from input, if there are spaces go to else
            string[] input = Console.ReadLine().Split();
            int N = input.Length;

            if (input.Length == 1)
            {
                N = input[0].Length;

                OriginalSudoku = new int[N, N];
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        int k = (int)(input[0][j]) - '0';
                        OriginalSudoku[i, j] = k;
                    }
                    input = Console.ReadLine().Split();
                }
            }
            else
            {
                if (input[input.Length - 1] == "")
                    N = input.Length - 1;

                OriginalSudoku = new int[N, N];

                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        int k = int.Parse(input[j]);
                        OriginalSudoku[i, j] = k;
                    }
                    input = Console.ReadLine().Split();
                }
            }
            return N;
        }
    }
}
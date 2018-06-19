﻿using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

namespace SudokuCSP
{
    internal class Program
    {
        //Int array to save the original input board
        private static int[,] OriginalSudoku;

        private static void Main(string[] args)
        {
            //  Console.WriteLine("\t|CB|\t|CBH\t|FC|\t|FCH|");

            List<SudokuSolver> solvers = new List<SudokuSolver>();
            solvers.Add( new ChronoBacktracking());
            solvers.Add(new ChronoBacktrackingHeuristic());
            solvers.Add( new ChronoBacktrackingLokaalHeuristic());
            solvers.Add( new ForwardChecking());
            solvers.Add( new ForwardCheckingHeuristic());
            solvers.Add( new ForwardCheckingLookahead());
            solvers.Add( new ForwardCheckingLookaheadHeuristic());

            
            do
            {
                Console.Clear();
                Console.WriteLine("Type the number of the solver you want to use?");
                int i = 1;
                foreach (SudokuSolver s in solvers)
                {
                    Console.WriteLine(i + ". " + s.ToString());
                    i++;
                }

                int pickedSolver = int.Parse(Console.ReadLine()) - 1;

                Console.Clear();
                Console.WriteLine("How do you want to input the sudoku?");
                Console.WriteLine("1. Manually");
                Console.WriteLine("2. Choose one of the ten from the sudoku text file");

                int pickedInputMethod = int.Parse(Console.ReadLine()) - 1;
                Console.Clear();
                if (pickedInputMethod == 0)
                {
                    Console.WriteLine("Copy the sudoku in the console then press enter twice.");
                    readBoard();
                }
                else
                {
                    Console.WriteLine("Which sudoku do you want 1-10?");
                    readBoardFromFile(int.Parse(Console.ReadLine()) - 1);

                }

                Console.Clear();

                solvers[pickedSolver].init(OriginalSudoku);
                Stopwatch sw = new Stopwatch();
                sw.Start();
                solvers[pickedSolver].solve();
                sw.Stop();

                Console.WriteLine("Solved the sudoku with " + solvers[pickedSolver].ToString()+ " in: " + sw.Elapsed.TotalMilliseconds + "ms" + " and " + solvers[pickedSolver].it + "iterations.");
                solvers[pickedSolver].print();
                Console.WriteLine("press enter to go again");
            } while (Console.ReadLine() == "");
            Console.ReadLine();

            /*
            SudokuSolver cb = new ChronoBacktracking();
            SudokuSolver cbh = new ChronoBacktrackingHeuristic();
            SudokuSolver cbht = new ChronoBacktrackingLokaalHeuristic();
            SudokuSolver fc = new ForwardChecking();
            SudokuSolver fch = new ForwardCheckingHeuristic();
            SudokuSolver fcl = new ForwardCheckingLookahead();
            SudokuSolver fclh = new ForwardCheckingLookaheadHeuristic();

            runsudoku(cb);
            runsudoku(cbh);
            runsudoku(cbht);
            runsudoku(fc);
            runsudoku(fch);
            runsudoku(fcl);
            (fcl as ForwardCheckingLookahead).makeConsistentb = true;
            runsudoku(fcl);
            runsudoku(fclh);
            (fclh as ForwardCheckingLookaheadHeuristic).makeConsistentb = true;
            runsudoku(fclh);
            */
            //    Console.WriteLine(i + ".\t" + cb.it + "\t" + cbh.it + "\t" + fc.it + "\t" + fch.it );
            //Console.WriteLine("CB: " + cb.it + " and with heuristic in: " + cbh.it + " and with forward checking in: " + fc.it +" and with fw heuristic: " + fch.it);

        }

        static public void runsudoku(SudokuSolver s)
        {
            double time = 0;
            long totalIterations = 0;
            string nameOfSolver = s.GetType().ToString().Split('.')[1];
                for (int i = 0; i < 10; i++)
                {
                    readBoardFromFile(i);
                    for (int k = 0; k < 50; k++)
                    {
                        s.init(OriginalSudoku);
                        s.it = 0;
                        Stopwatch swatch = new Stopwatch();
                        swatch.Start();

                        s.solve();
                  
                        totalIterations += s.it;
                        swatch.Stop();

                        time += swatch.Elapsed.TotalMilliseconds;
                    }
                }
            Console.WriteLine( nameOfSolver + (new string(' ', 35- nameOfSolver.Length))  +  "\t" + string.Format("{0:0.00}", time/50) + "ms  \t" + totalIterations / 50.0 + " iterations");
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
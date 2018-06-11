﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuCSP
{
    internal class ForwardChecking : SudokuSolver
    {
        public override void solve()
        {
            domain = new int[N * N];

            //Fill all domain values with numbers 1-9
            for (int i = 0; i < N * N; i++)
            {
                domain[i] = 1022;

                //for (int j = 1; j < N + 1; j++)
                //{
                //    domain[i] |= (1 << j);
                //}
            }

            for (int i = 0; i < N * N; i++)
            {
                if (board[i] != 0)
                {
                    domain[i] = 0;

                    RemoveFromDomains(i, board[i]);
                }
            }
            solveRec(0);
        }

        public void printD()
        {
            Console.WriteLine("-------------------------");
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if ((j) % 3 == 0)
                        Console.Write("| ");

                    if (fixedboard[i * N + j] != 0) { Console.Write("---------"); }
                    else
                    {
                        for (int k = 1; k < N + 1; k++)
                            if (((domain[i * N + j] >> k) & 1) == 1)
                                Console.Write(k + "");
                            else
                                Console.Write(" ");
                    }
                    Console.Write("|");

                    if (j == N - 1)
                        Console.Write("||");
                }
                Console.WriteLine();
                if ((i) % 3 == 2)
                    Console.WriteLine("-------------------------");
            }
        }

        protected List<Tuple<int, int>> RemoveFromDomains(int place, int num)
        {
            int y = place / N;
            int x = place % N;

            List<Tuple<int, int>> changes = new List<Tuple<int, int>>();

            for (int i = 0; i < N; i++)
            {
                if (board[N * y + i] == 0)
                {
                    if (((domain[N * y + i] >> num) & 1) == 1)
                        changes.Add(Tuple.Create(N * y + i, num));
                    domain[N * y + i] &= ~(1 << num);
                }
            }
            for (int i = 0; i < N; i++)
            {
                if (board[N * i + x] == 0)
                {
                    if (((domain[N * i + x] >> num) & 1) == 1)
                        changes.Add(Tuple.Create(N * i + x, num));
                    domain[N * i + x] &= ~(1 << num);
                }
            }
            int xb = x / B;
            int yb = y / B;

            for (int i = xb * B; i < xb * B + B; i++)
            {
                for (int j = yb * B; j < yb * B + B; j++)
                {
                    if (board[j * N + i] == 0)
                    {
                        if (((domain[j * N + i] >> num) & 1) == 1)
                            changes.Add(Tuple.Create(j * N + i, num));
                        domain[j * N + i] &= ~(1 << num);
                    }
                }
            }
            return changes;
        }

        protected int[] domain;

        protected virtual bool solveRec(int start)
        {
            it++;
            if (start == N * N)
            {
                return true;
            }

            if (board[start] == 0)
            {
                for (int i = 1; i < N + 1; i++)
                {
                    if (((domain[start] >> i) & 1) == 1)
                    {
                        List<Tuple<int, int>> changes;

                        board[start] = i;
                        changes = RemoveFromDomains(start, i);
                        bool verandering = true;
                        bool gaatGoed = true;
                        int numberOfChanges = changes.Count();

                        while (verandering && gaatGoed)
                        {
                            for (int k = 0; k < N * N; k++)
                            {
                                if (domain[k] == 0 && board[k] == 0)
                                {
                                    foreach (Tuple<int, int> p in changes)
                                    {
                                        domain[p.Item1] |= (1 << p.Item2);
                                        board[p.Item1] = 0;
                                    }
                                    board[start] = 0;
                                    gaatGoed = false;
                                    break;
                                }
                            }
                            if(gaatGoed)
                            {
                                foreach(Tuple<int, int> p in changes)
                                {
                                    if(IsPowerOfTwo(domain[p.Item1]))
                                    {
                                        int optie = (int)Math.Log(domain[p.Item1], 2);
                                        board[p.Item1] = optie;
                                        changes.AddRange(RemoveFromDomains(p.Item1, optie));
                                        break;
                                    }
                                }
                            }

                            verandering = false;
                            if (changes.Count() > numberOfChanges)
                            {
                                verandering = true;
                                numberOfChanges = changes.Count();
                            }

                        }
                        if (!gaatGoed)
                            continue;

                        if(solveRec(start + 1))
                            return true;                    
                        else
                        {
                            foreach (Tuple<int, int> p in changes)
                            {
                                domain[p.Item1] |= (1 << p.Item2);
                                board[p.Item1] = 0;
                            }
                            board[start] = 0;
                        }
                    }
                }
            }
            else
            {
                return solveRec(start + 1);
            }

            return false;
        }

    }
}
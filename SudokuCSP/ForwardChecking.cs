using System;
using System.Collections.Generic;

namespace SudokuCSP
{
    public class ForwardChecking : SudokuSolver
    {
        protected int[] domain;

        public override void solve()
        {
            domain = new int[N * N];

            //Fill all domain values with numbers 1-9
            for (int i = 0; i < N * N; i++)
                domain[i] = 1022;

            //Update the domains
            for (int i = 0; i < N * N; i++)
            {
                if (board[i] != 0)
                {
                    domain[i] = 0;
                    RemoveAllFromDomains(i, board[i]);
                }
            }
            //Solve
            solveRec(0);
        }

        //Print the current domain values instead of the board
        public void printDomain()
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

        //Add a number at a certain place to the board and remove all the domains that now are impossible, return  a list of changes
        protected List<Tuple<int, int>> RemoveAllFromDomains(int place, int num)
        {
            int y = place / N;
            int x = place % N;

            List<Tuple<int, int>> changes = new List<Tuple<int, int>>();

            //check the rows
            for (int i = 0; i < N; i++)
            {
                if (board[N * y + i] == 0)
                {
                    if (((domain[N * y + i] >> num) & 1) == 1)
                        changes.Add(Tuple.Create(N * y + i, num));
                    domain[N * y + i] &= ~(1 << num);
                }
            }

            //Check the columns
            for (int i = 0; i < N; i++)
            {
                if (board[N * i + x] == 0)
                {
                    if (((domain[N * i + x] >> num) & 1) == 1)
                        changes.Add(Tuple.Create(N * i + x, num));
                    domain[N * i + x] &= ~(1 << num);
                }
            }

            //Check the block
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

        //Function to remove the variables from a domain by setting a new variable, returns a list of changes made for rollback. Only for one specific variable in a domain
        protected List<int> RemoveOneFromDomains(int place, int num)
        {
            int y = place / N;
            int x = place % N;

            List<int> changes = new List<int>();

            //Check the rows
            for (int i = 0; i < N; i++)
            {
                if (board[N * y + i] == 0)
                {
                    if (((domain[N * y + i] >> num) & 1) == 1)
                        changes.Add(N * y + i);
                    domain[N * y + i] &= ~(1 << num);
                }
            }
            //Check the columns
            for (int i = 0; i < N; i++)
            {
                if (board[N * i + x] == 0)
                {
                    if (((domain[N * i + x] >> num) & 1) == 1)
                        changes.Add(N * i + x);
                    domain[N * i + x] &= ~(1 << num);
                }
            }
            //Check the blocks
            int xb = x / B;
            int yb = y / B;

            for (int i = xb * B; i < xb * B + B; i++)
            {
                for (int j = yb * B; j < yb * B + B; j++)
                {
                    if (board[j * N + i] == 0)
                    {
                        if (((domain[j * N + i] >> num) & 1) == 1)
                            changes.Add(j * N + i);
                        domain[j * N + i] &= ~(1 << num);
                    }
                }
            }
            return changes;
        }

        //Recursive solve function
        protected virtual bool solveRec(int start)
        {
            //Stop if we are at the end and found a solution
            if (start == N * N)
            {
                return true;
            }

            if (board[start] == 0)
            {
                it++;
                //Try to fill in every number just like chronological backtracking
                for (int i = 1; i < N + 1; i++)
                {
                    //If it is possible to fill this variable go on
                    if (((domain[start] >> i) & 1) == 1)
                    {
                        List<int> changes;

                        board[start] = i;

                        //We now remove all the variables from the domains that are directly affected by this change
                        changes = RemoveOneFromDomains(start, i);
                        bool verkeerd = false;
                        //If a domain is empty we have to stop, we can no longer find a solution so go back to a previous version
                        for (int k = 0; k < N * N; k++)

                        {
                            if (domain[k] == 0 && board[k] == 0)
                            {
                                foreach (int p in changes)
                                {
                                    domain[p] |= (1 << i);
                                }
                                board[start] = 0;
                                verkeerd = true;
                                break;
                            }
                        }
                        if (verkeerd)
                            continue;
                        //Continue solving the next piece
                        if (solveRec(start + 1) == true)
                            return true;
                        else
                        {
                            //Something went wrong we have to rollback the problem
                            foreach (int k in changes)
                            {
                                domain[k] |= (1 << i);
                            }
                            board[start] = 0;
                        }
                    }
                }
            }
            else
                return solveRec(start + 1);

            return false;
        }
    }
}
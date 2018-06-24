using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuCSP
{
    internal class MAC : ForwardChecking
    {
        public override void solve()
        {
            domain = new int[N * N];

            //Fill all domain values with numbers 1-9
            for (int i = 0; i < N * N; i++)
                domain[i] = 1022;

            for (int i = 0; i < N * N; i++)
            {
                if (board[i] != 0)
                {
                    domain[i] = 0;
                    RemoveAllFromDomains(i, board[i]);
                }
            }
            if (makeConsistentb)
                makeConsistent();

            solveRec(0);
        }

        public bool makeConsistentb = false;

        //Function to make the puzzel arc consistent from the start
        protected virtual void makeConsistent()
        {
            bool foundChange = true;
            while (foundChange)
            {
                foundChange = false;
                for (int k = 0; k < N * N; k++)
                {
                    if (IsPowerOfTwo(domain[k]) && board[k] == 0)
                    {
                        foundChange = true;
                        int i = 1;
                        for (; i < N + 1; i++)
                        {
                            if (((domain[k] >> i) & 1) == 1)
                                break;
                        }
                        board[k] = i;
                        RemoveOneFromDomains(k, i);
                        break;
                    }
                }
            }
        }

        //Recursive solve function same as the others
        protected override bool solveRec(int start)
        {
            if (start == N * N)
            {
                return true;
            }

            if (board[start] == 0)
            {
                it++;

                for (int i = 1; i < N + 1; i++)
                {
                    if (((domain[start] >> i) & 1) == 1)
                    {
                        List<Tuple<int, int>> changes;

                        board[start] = i;
                        changes = RemoveAllFromDomains(start, i);
                        bool verandering = true;
                        bool gaatGoed = true;
                        int numberOfChanges = changes.Count;

                        int hoeveelToegevoegd = changes.Count;

                        //Now if we fill in a new change we go over all the affected variables and make them consistent.
                        //Then we keep going untill there are no more variables to make consinstent.

                        while (verandering && gaatGoed)
                        {
                            for (int k = 0; k < N * N; k++)
                            {
                                if (domain[k] == 0 && board[k] == 0)
                                {
                                    for (int r = 0; r < changes.Count; r++)
                                    {
                                        domain[changes[r].Item1] |= (1 << changes[r].Item2);
                                        board[changes[r].Item1] = 0;
                                    }

                                    board[start] = 0;
                                    gaatGoed = false;
                                    break;
                                }
                            }
                            if (gaatGoed)
                            {
                                List<Tuple<int, int>> NewTempChanges = new List<Tuple<int, int>>();
                                for (int r = changes.Count - hoeveelToegevoegd; r < changes.Count; r++)
                                {
                                    var p = changes[r];
                                    if (IsPowerOfTwo(domain[p.Item1]))
                                    {
                                        int k = 1;
                                        for (; k < N + 1; k++)
                                        {
                                            if (((domain[p.Item1] >> k) & 1) == 1)
                                                break;
                                        }
                                        board[p.Item1] = k;
                                        NewTempChanges.AddRange(RemoveAllFromDomains(p.Item1, k));
                                    }
                                }
                                hoeveelToegevoegd = NewTempChanges.Count;
                                changes.AddRange(NewTempChanges);
                            }

                            verandering = false;
                            if (changes.Count > numberOfChanges)
                            {
                                verandering = true;
                                numberOfChanges = changes.Count();
                            }
                        }
                        if (!gaatGoed)
                            continue;

                        if (solveRec(start + 1))
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
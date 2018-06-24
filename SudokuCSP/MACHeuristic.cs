using System;
using System.Collections.Generic;

namespace SudokuCSP
{
    internal class MACHeuristic : MAC
    {
        //Same function as in MAC only this time we first do the position with the smallest domain
        protected override bool solveRec(int startN)
        {
            if (startN == N * N)
            {
                return true;
            }

            int bestStart = 0;
            int bestsoffar = 10;
            for (int i = 0; i < domain.Length; i++)
            {
                if (board[i] == 0)
                {
                    int tot = 0;
                    for (int k = 1; k < N + 1; k++)
                    {
                        tot += (domain[i] >> k) & 1;
                    }
                    if (tot < bestsoffar)
                    {
                        bestStart = i;
                        bestsoffar = tot;
                    }
                }
            }

            int start = bestStart;
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
                                numberOfChanges = changes.Count;
                            }
                        }
                        if (!gaatGoed)
                            continue;

                        if (solveRec(startN + 1))
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
                return solveRec(startN + 1);
            }
            return false;
        }
    }
}
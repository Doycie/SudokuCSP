using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuCSP
{
    class ForwardCheckingHeuristic : ForwardChecking
    {
        public override void solve()
        {
            domain = new int[N * N];

            //Fill all domain values with numbers 1-9
            for (int i = 0; i < N * N; i++)
            {
                domain[i] = 1022;
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
           // print();
        }

        protected override bool solveRec(int startN)
        {
            it++;
            if (startN == N * N)
            {
                return true;
            }

            int bestStart = 0;
            int bestsoffar =10;
            for (int i = 0; i < domain.Length; i++)
            {
                if (board[i] == 0)
                {
                    int tot = 0;
                    for (int k =1; k < N+1; k++)
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
                            if (gaatGoed)
                            {
                                foreach (Tuple<int, int> p in changes)
                                {
                                    if (IsPowerOfTwo(domain[p.Item1]))
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

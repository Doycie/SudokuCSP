using System.Collections.Generic;

namespace SudokuCSP
{
    internal class ForwardCheckingHeuristic : ForwardChecking
    {
        //Recursive solve function

        protected override bool solveRec(int startN)
        {
            //Stop if we are at the end and found a solution

            if (startN == N * N)
            {
                return true;
            }

            //This time we make sure to first do the position that has the least variable in its domain.
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

                        if (solveRec(startN + 1))
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
            {
                return solveRec(startN + 1);
            }
            return false;
        }
    }
}
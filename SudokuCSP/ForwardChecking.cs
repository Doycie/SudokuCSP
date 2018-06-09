using System;

namespace SudokuCSP
{
    internal class ForwardChecking : SudokuSolver
    {
        public override void solve()
        {
            domain = new int[N * N];

            //Fill all domain values with numbers 1-9
            for(int i = 0; i <  N*N; i++)
            {
                domain[i] = 0;
                
                for(int j = 1; j < N + 1; j++)
                {
                    domain[i] |= (1 << j);
                }
            }

            for(int i = 0; i < N*N; i++)
            {
                if(board[i] != 0)
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

                    for (int k = 1; k < N + 1; k++)
                        if (((domain[i * N + j] >> k) & 1) == 1)
                            Console.Write(k + "");
                        else
                            Console.Write(" ");
                    Console.Write("|");
                    
                    if (j == N - 1)
                        Console.Write("||");
                }
                Console.WriteLine();
                if ((i) % 3 == 2)
                    Console.WriteLine("-------------------------");
            }
        }

        protected void RemoveFromDomains(int place, int num)
        {
            int y = place / N;
            int x = place % N;

            for (int i = 0; i < N; i++)
            {
                domain[N * y + i] &= ~(1 << num);
            }
            for (int i = 0; i < N; i++)
            {
                domain[N * i + x] &= ~(1 << num);
            }
            int xb = x / B;
            int yb = y / B;

            for (int i = xb * B; i < xb * B + B; i++)
            {
                for (int j = yb * B; j < yb * B + B; j++)
                {
                    domain[N * y + i] &= ~(1 << num);
                }
            }
        }


        int[] domain;

        public bool solveRec(int start)
        {
            Console.Clear();
            printD();
            print();
            Console.ReadLine();

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
                        if (check(start, i))
                        {
                            board[start] = i;
                            RemoveFromDomains(start, i);

                            // Console.SetCursorPosition(  2 + 2*(start%N) + (start%N/3) * 2 ,  (start / N) + 1 + (((start/N)/3)));
                            //Console.Write(i);
                        }
                        else
                        {
                            continue;
                        }
                        bool result = solveRec(start + 1);
                        if (result == true)
                        {
                            return true;
                        }
                        else
                        {
                            board[start] = 0;
                            // Console.SetCursorPosition( 2 + 2 * (start % N) + (start % N / 3) * 2, (start / N) + 1 + (((start / N) / 3)));
                            //Console.Write(0);
                        }
                    }

                }
                if (board[start] == 0)
                {
                    return false;
                }
            }
            else
            {
                bool result = solveRec(start + 1);
                return result;
            }

            return false;
        }
    }
}
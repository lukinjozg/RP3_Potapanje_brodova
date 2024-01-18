using System;
using System.Collections.Generic;
using System.Threading;

namespace Potapanjebrodova
{

    public enum State : int
    {
        AVAILABLE = 0,
        MISSED = 1,
        HIT = 2,
        SINKED = 3
    }

    public class AI
    {
        public AI()
        {

        }

        Tuple<int, int> nextMoveRandom(State[,] matrix)
        {
            Random random = new Random();

            List<int> arr = new List<int>();

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (matrix[i, j] == State.AVAILABLE) arr.Add(i * 10 + j);
                }
            }

            int n = arr.Count;

            // Generate random indices
            int index = random.Next(0, n);

            return Tuple.Create(arr[index] / 10, arr[index] % 10);
        }

        Tuple<int, int> findNextHitFieldIntermediate(State[,] matrix, Tuple<int, int> curr)
        {
            int[] sx = { 1, 0, -1, 0 };
            int[] sy = { 0, 1, 0, -1 };
            var (x, y) = curr;
            Tuple<int, int> sol = Tuple.Create(-1, -1);

            for (int i = 0; i < 4; i++)
            {
                int nx = x + sx[i];
                int ny = y + sy[i];
                if (nx > 9 || nx < 0 || ny > 9 || ny < 0 || matrix[nx, ny] != State.AVAILABLE) continue;
                sol = Tuple.Create(nx, ny);
                break;
            }

            return sol;
        }

        Tuple<int, int> nextMoveIntermediate(State[,] matrix)
        {
            Tuple<int, int> ret = Tuple.Create(-1, -1);

            bool moveFound = false;

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (matrix[i, j] == State.HIT)
                    {
                        ret = findNextHitFieldIntermediate(matrix, Tuple.Create(i, j));
                        if (ret.Item1 > -1)
                        {
                            moveFound = true;
                            break;
                        }
                    }
                }
                if (moveFound) break;
            }

            if (!moveFound)
            {
                Random random = new Random();
                List<int> arr = new List<int>();

                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if ((i + j) % 2 == 0 && matrix[i, j] == State.AVAILABLE)
                        {
                            arr.Add(10 * i + j);
                        }
                    }
                }

                int n = arr.Count;
                int index = random.Next(0, n);

                ret = Tuple.Create(arr[index] / 10, arr[index] % 10);
            }

            return ret;
        }

        int power(int k)
        {
            int ret = 1;
            for (int i = 0; i < k; i++)
            {
                ret *= 10;
            }
            return ret;
        }

        Tuple<int, int> nextMoveHard(State[,] matrix, int[] shipsRemained)
        {
            int[,] probMatrix = new int[10, 10];
            bool boatHit = false;

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (matrix[i, j] == State.HIT) boatHit = true;
                }
            }

            for (int i = 0; i < shipsRemained.Length; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    for (int k = 0; k < 10; k++)
                    {
                        //vodoravno
                        int wrong = 0;
                        int hit = 0;
                        for (int l = 0; l < shipsRemained[i]; l++)
                        {
                            if (k + l > 9 || matrix[j, k + l] == State.MISSED || matrix[j, k + l] == State.SINKED)
                            {
                                wrong = 1;
                                break;
                            }
                            if (matrix[j, k + l] == State.HIT) hit++;
                        }
                        if (wrong == 0)
                        {
                            for (int l = 0; l < shipsRemained[i]; l++)
                            {
                                if (matrix[j, k + l] == State.AVAILABLE) probMatrix[j, k + l] += power(hit);
                            }
                        }
                        //okomito
                        wrong = 0;
                        hit = 0;
                        for (int l = 0; l < shipsRemained[i]; l++)
                        {
                            if (j + l > 9 || matrix[j + l, k] == State.MISSED || matrix[j + l, k] == State.SINKED)
                            {
                                wrong = 1;
                                break;
                            }
                            if (matrix[j + l, k] == State.HIT) hit++;
                        }
                        if (wrong == 0)
                        {
                            for (int l = 0; l < shipsRemained[i]; l++)
                            {
                                if (matrix[j + l, k] == State.AVAILABLE) probMatrix[j + l, k] += power(hit);
                            }
                        }
                    }
                }
            }
            Tuple<int, int> ret = Tuple.Create(-1, -1);

            if (boatHit)
            {
                int mx = 0;
                int ind = -1;
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (probMatrix[i, j] > mx)
                        {
                            mx = probMatrix[i, j];
                            ind = i * 10 + j;
                        }
                    }
                }

                ret = Tuple.Create(ind / 10, ind % 10);
            }
            else
            {
                int mx = 0;
                int ind = -1;
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (probMatrix[i, j] > mx && (i + j) % 2 == 0)
                        {
                            mx = probMatrix[i, j];
                            ind = i * 10 + j;
                        }
                    }
                }

                ret = Tuple.Create(ind / 10, ind % 10);
            }

            return ret;
        }

        public void setBattleshipsEasy(ref int[,] matrix)
        {
            Random random = new Random();
            int[] sx = { 1, 0, -1, 0 };
            int[] sy = { 0, 1, 0, -1 };

            for (int i = 5; i > 1; i--)
            {
                bool found = false;
                while (found)
                {
                    int r1 = random.Next(100);
                    int r2 = random.Next(4);
                    int x = r1 / 10;
                    int y = r1 % 10;

                    bool good = true;

                    for (int j = 0; j < i; j++)
                    {
                        if (x < 0 || x > 9 || y < 0 || y > 9 || matrix[x, y] != 0)
                        {
                            good = false;
                            break;
                        }
                        x += sx[r2];
                        y += sy[r2];
                    }

                    if (good)
                    {
                        x = r1 / 10;
                        y = r1 % 10;
                        for (int j = 0; j < i; j++)
                        {
                            matrix[x, y] = i;
                            x += sx[r2];
                            y += sy[r2];
                        }
                        found = true;
                    }
                }
            }
        }

        bool notAvailableField(int[,] matrix, int x, int y)
        {
            int[] sx = { 1, 0, -1, 0, 1, 1, -1, -1 };
            int[] sy = { 0, 1, 0, -1, 1, -1, 1, -1 };

            int sum = 0;

            for (int i = 0; i < 8; i++)
            {
                int nx = x + sx[i];
                int ny = y + sy[i];
                if (nx < 0 || nx > 9 || ny < 0 || ny > 9) continue;
                sum += matrix[nx, ny];
            }

            return (sum > 0);
        }

        public void setBattleshipsMediumHard(ref int[,] matrix)
        {
            Random random = new Random();
            int[] sx = { 1, 0, -1, 0 };
            int[] sy = { 0, 1, 0, -1 };

            for (int i = 5; i > 1; i--)
            {
                bool found = false;
                while (!found)
                {
                    int r1 = random.Next(100);
                    int r2 = random.Next(4);
                    int x = r1 / 10;
                    int y = r1 % 10;

                    bool good = true;

                    for (int j = 0; j < i; j++)
                    {
                        if (x < 0 || x > 9 || y < 0 || y > 9 || matrix[x, y] != 0
                            || notAvailableField(matrix, x, y))
                        {
                            good = false;
                            break;
                        }
                        x += sx[r2];
                        y += sy[r2];
                    }

                    if (good)
                    {
                        x = r1 / 10;
                        y = r1 % 10;
                        for (int j = 0; j < i; j++)
                        {
                            matrix[x, y] = i;
                            x += sx[r2];
                            y += sy[r2];
                        }
                        found = true;
                    }
                }
            }
        }
    }

}
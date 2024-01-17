using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Potapanjebrodova
{
	public enum State
	{
		AVAILABLE = 0,
		MISSED = 1,
		HIT = 2,
		SINKED = 3
	}

	Tuple<int, int> nextMoveRandom(int[,] matrix)
	{
		Random random = new Random();

		List<int> arr;

		for(int i = 0; i < 10; i++)
		{
			for(int j = 0; j < 10; j++)
			{
				if (matrix[i][j] == State.AVAILABLE) arr.Add(i * 10 + j);
			}
		}

		int n = arr.Count;

		// Generate random indices
		int index = random.Next(0, n);

		return Tuple.Create(arr[index] / 10, arr[index] % 10);
	}

	Tuple<int, int> findNextHitFieldIntermediate(int[,] matrix, Tuple<int, int> curr)
	{
		int[] sx = { 1, 0, -1, 0 };
		int[] sy = { 0, 1, 0, -1 };
		var (x, y) = curr;
		Tuple<int, int> sol = Tuple.Create(-1, -1);

		for(int i = 0; i < 4; i++)
		{
			int nx = x + sx[i];
			int ny = y + sy[i];
			if (nx > 9 || nx < 0 || ny > 9 || ny < 0 || matrix[nx][ny] != State.AVAILABLE) continue;
			sol = Tuple.Create(nx, ny);
			break;
		}

		return sol;
	}

	Tuple<int, int> nextMoveIntermediate(int[,] matrix)
	{
		Tuple<int, int> ret = Tuple.Create(-1, -1);

		bool moveFound = false;

		for(int i = 0; i < 10; i++)
		{
			for(int j = 0; j < 10; j++)
			{
				if (matrix[i][j] == State.HIT)
				{
					ret = findNextHitFieldIntermediate(matrix, Tuple.Create(i, j));
					if(ret.Item1 > -1)
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
			List<int> arr;

			for (int i = 0; i < 10; i++)
			{
				for(int j = 0; j < 10; j++)
				{
					if((i+j) % 2 == 0 && matrix[i][j] == State.AVAILABLE)
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
		for(int i = 0; i < k; i++)
		{
			ret *= 10;
		}
		return ret;
	}

    Tuple<int, int> nextMoveHard(int[,] matrix, int[] shipsRemained)
	{
		int probMatrix[,] = new int[10][10];
		bool boatHit = false;

		for(int i = 0; i < 10; i++)
		{
			for(int j = 0; j < 10; j++)
			{
				if (matrix[i][j] == State.HIT) boatHit = true;
			}
		}

		for (int i = 0; i < shipsRemained.Length; i++)
		{
			for(int j = 0; j < 10; j++)
			{
				for (int k = 0; k < 10; k++)
				{
					//vodoravno
					int wrong = 0;
					int hit = 0;
					for(int l = 0; l < shipsRemained[i]; l++)
					{
						if (k + l > 9 || matrix[j][k + l] == State.MISSED || matrix[j][k + l] == State.SINKED)
						{
							wrong = 1;
							break;
						}
						if (matrix[j][k + l] == State.HIT) hit++;
					}
					if (!wrong)
					{
						for(int l = 0; l < shipsRemained[i]; l++)
						{
							if (matrix[j][k + l] == State.AVAILABLE) probMatrix[j][k + l] += power(hit);
						}
					}
					//okomito
					wrong = 0;
					hit = 0;
                    for (int l = 0; l < shipsRemained[i]; l++)
                    {
                        if (j + l > 9 || matrix[j + l][k] == State.MISSED || matrix[j + l][k] == State.SINKED)
                        {
                            wrong = 1;
                            break;
                        }
                        if (matrix[j + l][k] == State.HIT) hit++;
                    }
                    if (!wrong)
                    {
                        for (int l = 0; l < shipsRemained[i]; l++)
                        {
                            if (matrix[j + l][k] == State.AVAILABLE) probMatrix[j + l][k] += power(hit);
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
			for(int i = 0; i < 10; i++)
			{
				for(int j = 0; j < 10; j++)
				{
					if (probMatrix[i][j] > mx)
					{
						mx = probMatrix[i][j];
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
                    if (probMatrix[i][j] > mx && (i + j) % 2 == 0)
                    {
                        mx = probMatrix[i][j];
                        ind = i * 10 + j;
                    }
                }
            }

            ret = Tuple.Create(ind / 10, ind % 10);
        }

		return ret;
	}
}
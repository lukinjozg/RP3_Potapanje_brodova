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
}
using UnityEngine;
using System.Collections;

public class CelulaGen : MonoBehaviour
{
	public int seed;
	public int resolution = 65;
	[Range(0, 100)]
	public int randomFillPercent;
	public float[,] heightMap;

	// Starts the cave generation coroutine and returns it so it can be awaited
	public IEnumerator StartCaveGeneration(int seeder)
	{
		heightMap = new float[resolution, resolution];

		// Fill the map randomly with a seed
		yield return StartCoroutine(RandomFillMap(seeder));

		// Smooth the map multiple times with a delay between each pass
		for (int i = 0; i < 5; i++)
		{
			SmoothMap();
			yield return null; // Yield to wait until the next frame
		}
	}

	private IEnumerator RandomFillMap(int seeds)
	{
		System.Random pseudoRandom = new System.Random(seeds.GetHashCode());

		for (int x = 0; x < resolution; x++)
		{
			for (int y = 0; y < resolution; y++)
			{
				if (x < 3 || x >= resolution - 3 || y < 3 || y >= resolution - 3)
				{
					heightMap[x, y] = 1;
				}
				else
				{
					heightMap[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
				}
			}
			yield return null;
		}
		int holeSize = 12;
		int holeStartY = resolution / 2 - holeSize / 2;
		for (int y = holeStartY; y < holeStartY + holeSize; y++)
		{
			heightMap[2, y] = 0;
			heightMap[1, y] = 0;
		}
	}

	private void SmoothMap()
	{
		for (int x = 1; x < resolution - 1; x++)
		{
			for (int y = 1; y < resolution - 1; y++)
			{
				int neighbourWallTiles = GetSurroundingWallCount(x, y);

				if (neighbourWallTiles > 4)
					heightMap[x, y] = 1;
				else if (neighbourWallTiles < 4)
					heightMap[x, y] = 0;
			}
		}
	}

	private int GetSurroundingWallCount(int gridX, int gridY)
	{
		int wallCount = 0;
		for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
		{
			for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
			{
				if (neighbourX >= 0 && neighbourX < resolution && neighbourY >= 0 && neighbourY < resolution)
				{
					if (neighbourX != gridX || neighbourY != gridY)
					{
						wallCount += (int)heightMap[neighbourX, neighbourY];
					}
				}
				else
				{
					wallCount++;
				}
			}
		}
		return wallCount;
	}
}

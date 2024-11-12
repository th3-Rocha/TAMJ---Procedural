using UnityEngine;
using System;

public static class Noise
{


    public static float[,] GenerateNoiseMap(int seed, float scale, int octaves, float persistence = 0.5f, float lacunarity = 1.5f, Vector2 offset = default)
    {
        float[,] noiseMap = new float[VaribleGlobal.worldSize + 1, VaribleGlobal.worldSize + 1];
        System.Random prng = new System.Random(seed);

        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if (scale <= 0)
            scale = 0.0001f;

        // Generate Perlin noise values
        for (int y = 0; y < VaribleGlobal.worldSize + 1; y++)
        {
            for (int x = 0; x < VaribleGlobal.worldSize + 1; x++)
            {
                float amplitude = 1;
                float frequency = 0.5f;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x - VaribleGlobal.worldSize / 2f + octaveOffsets[i].x) / scale * frequency;
                    float sampleY = (y - VaribleGlobal.worldSize / 2f + octaveOffsets[i].y) / scale * frequency;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                noiseMap[x, y] = noiseHeight;
            }
        }

        return noiseMap;
    }
}

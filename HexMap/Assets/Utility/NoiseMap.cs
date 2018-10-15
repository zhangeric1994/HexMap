using UnityEngine;
using System.Collections;

public struct NoiseMap
{
    /// <summary>
    /// Generate a 2D Perlin noise map with a gradient map
    /// </summary>
    /// <param name="gradientMap">The gradient map used for generating noise</param>
    /// <param name="length">The length of the 2D map to be generated</param>
    /// <param name="width">The width of the 2D map to be generated</param>
    /// <param name="frequency">The frequency of the keynote</param>
    /// <param name="amplitude">The amplitude of the keynote</param>
    /// <param name="octaves">The number of noises used for combination</param>
    /// <param name="persistance">The ratio of amplitudes between different octaves</param>
    /// <param name="min">The minimum value of the generated floats</param>
    /// <param name="max">The maximum value of the generated floats</param>
    /// <returns>A 2D map of floats</returns>
    public static float[,] Generate2DPerlinNoiseMap(Vector2[,] gradientMap, int length, int width, float frequency, float amplitude, int octaves, float persistance, float min = 0, float max = 1)
    {
        if (length == 0 || width == 0)
            return null;

        int gradientMapLength = gradientMap.GetLength(0);
        int gradientMapWidth = gradientMap.GetLength(1);

        float[,] noiseMap = new float[length, width];
        for (int x = 0; x < length; x++)
            for (int y = 0; y < width; y++)
                noiseMap[x, y] = Mathf.Lerp(min, max, Generate2DPerlinNoise(gradientMap, (float)(x * (gradientMapLength - 2)) / (length - 1), (float)(y * (gradientMapWidth - 2)) / (length - 1), frequency, amplitude, octaves, persistance));

        return noiseMap;
    }

    /// <summary>
    /// Generate a 2D Perlin noise map with a random seed
    /// </summary>
    /// <param name="seed">The random seed used for generating the gradient map</param>
    /// <param name="length">The length of the 2D map</param>
    /// <param name="width">The width of the 2D map</param>
    /// <param name="frequency">The frequency of the keynote</param>
    /// <param name="amplitude">The amplitude of the keynote</param>
    /// <param name="octaves">The number of noises used for combination</param>
    /// <param name="persistance">The ratio of amplitudes between different octaves</param>
    /// <param name="min">The minimum value of the generated floats</param>
    /// <param name="max">The maximum value of the generated floats</param>
    /// <returns>A 2D map of floats</returns>
    public static float[,] Generate2DPerlinNoiseMap(int seed, int length, int width, float frequency, float amplitude, int octaves, float persistance, float min = 0, float max = 1)
    {
        return Generate2DPerlinNoiseMap(GenerateRandom2DGradients(seed, Mathf.CeilToInt(length / 8f) + 2, Mathf.CeilToInt(width / 8f) + 2), length, width, frequency, amplitude, octaves, persistance, min, max);
    }

    #region Private Helpers
    private static Vector2[,] GenerateRandom2DGradients(int seed, int length, int width)
    {
        Vector2[,] gradients = new Vector2[length, width];
        System.Random rng = new System.Random(seed);

        for (int x = 0; x < length; x++)
            for (int y = 0; y < width; y++)
                gradients[x, y] = new Vector2((float)rng.NextDouble() * 2 - 1, (float)rng.NextDouble() * 2 - 1).normalized;

        return gradients;
    }

    private static float[,,] GenerateRandom3DFloatArray(int seed, int length, int width, int height, float min = 0, float max = 1)
    {
        float[,,] floats = new float[length, width, height];
        System.Random rng = null;
        int count = 0;

        for (int x = 0; x < length; x++)
            for (int y = 0; y < width; y++)
                for (int z = 0; z < height; z++)
                {
                    if (count % 10 == 0)
                        rng = new System.Random(seed++);

                    floats[x, y, z] = min + (float)(rng.NextDouble()) * (max - min);
                    count++;
                }

        return floats;
    }

    private static float Generate2DPerlinNoise(Vector2[,] gradientMap, float x, float y, float frequency, float amplitude, int octaves, float persistance)
    {
        float noise = 0;
        float max = 0;

        for (int i = 0; i < octaves; i++)
        {
            noise += Make2DPerlinNoise(gradientMap, x * frequency, y * frequency) * amplitude;
            max += amplitude;

            frequency *= 2;
            amplitude *= persistance;
        }

        return (noise / max + 1) / 2;
    }

    private static float Make2DPerlinNoise(Vector2[,] gradients, float x, float y)
    {
        float xf = x - (int)x;
        float yf = y - (int)y;

        int xi = (int)x % (gradients.GetLength(0) - 1);
        int yi = (int)y % (gradients.GetLength(1) - 1);

        Vector2 point = new Vector2(xi + xf, yi + yf);
        Vector2[] unitPoints = new Vector2[4];
        for (int i = 0; i < 4; i++)
            unitPoints[i] = new Vector2(xi + i % 2, yi + i / 2);

        float[] dotProducts = new float[4];
        for (int j = 0; j < 4; j++)
            dotProducts[j] = Vector2.Dot(gradients[(int)unitPoints[j].x, (int)unitPoints[j].y], unitPoints[j] - point);

        float u = Fade(xf);
        float v = Fade(yf);

        return Mathf.Lerp(Mathf.Lerp(dotProducts[0], dotProducts[1], u), Mathf.Lerp(dotProducts[2], dotProducts[3], u), v);
    }

    private static float Fade(float t)
    {
        return t * t * t * (t * (t * 6 - 15) + 10); // 6t^5 - 15t^4 + 10t^3
    }
    #endregion Private Helpers
}


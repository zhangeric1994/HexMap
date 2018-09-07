using UnityEngine;
using System;

public class PerlinNoiseGenerater
{
    public enum Dimension : int
    {
        TWO_D = 2,
        THREE_D = 3,
    }

    public enum Continum : int
    {
        NONE,
        X,
        Y,
        XY,
        Z,
        XZ,
        YZ,
        XYZ
    }

    #region Variables

    private Dimension _dimension;
    private Vector3[][,,] _gradientMaps;
    private Continum _continum;

    private float _lacunarity;
    private int _octaves;
    private float _persistance;

    #endregion Variables
    #region Accessors

    public int length
    {
        get
        {
            return _gradientMaps[0].GetLength(0);
        }
    }

    public int width
    {
        get
        {
            return _gradientMaps[0].GetLength(1);
        }
    }

    public int height
    {
        get
        {
            return _gradientMaps[0].GetLength(2);
        }
    }

    public Continum continum
    {
        get
        {
            return _continum;
        }
    }

    private static readonly float SQRT2 = MathUtility.SQRT2;

    #endregion Accessors
    #region Constructors

    public PerlinNoiseGenerater(int seed, Continum continum, int length, int width) : this(seed, continum, length, width, 1, 1, 1, 1)
    {
    }

    public PerlinNoiseGenerater(int seed, Continum continum, int length, int width, int octaves, float lacunarity, float persistance) : this(seed, continum, length, width, 1, octaves, lacunarity, persistance)
    {
    }

    public PerlinNoiseGenerater(int seed, Continum continum, int length, int width, int height, int octaves, float lacunarity, float persistance)
    {
        _dimension = height <= 1 ? Dimension.TWO_D : Dimension.THREE_D;

        _gradientMaps = GenerateRandomizedGradientMaps(seed, length, width, height, continum, octaves, lacunarity);
        _continum = continum;

        _lacunarity = lacunarity;
        _octaves = octaves;
        _persistance = persistance;
    }

    #endregion Constructors
    #region Public Non-static Functions

    public float SampleAt(float x, float y, float min = 0, float max = 1)
    {
        return Mathf.Lerp(min, max, SampleAt(_gradientMaps, _dimension, x, y, _octaves, _persistance));
    }

    #endregion Public Non-static Functions
    #region Public Static Functions

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
    //public static float[,] Generate2DNoiseMap(Vector3[,,] gradientMap, int length, int width, float frequency, float amplitude, int octaves, float lacunarity, float persistance, WrapMode wrapMode, float min = 0, float max = 1)
    //{
    //    if (length == 0 || width == 0)
    //        return null;

    //    int gradientMapLength = gradientMap.GetLength(0);
    //    int gradientMapWidth = gradientMap.GetLength(1);

    //    float[,] noiseMap = new float[length, width];
    //    for (int x = 0; x < length; x++)
    //        for (int y = 0; y < width; y++)
    //            noiseMap[x, y] = Mathf.Lerp(min, max, Generate2DPerlinNoise(gradientMap, (float)(x * (gradientMapLength - 2)) / (length - 1), (float)(y * (gradientMapWidth - 2)) / (length - 1), frequency, amplitude, octaves, lacunarity, persistance, wrapMode));

    //    return noiseMap;
    //}

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
    //public static float[,] Generate2DNoiseMap(int seed, int length, int width, float frequency, float amplitude, int octaves, float lacunarity, float persistance, WrapMode wrapMode, float min = 0, float max = 1)
    //{
    //    return Generate2DNoiseMap(GenerateRandomizedGradientMap(seed, Mathf.CeilToInt(length / 8f) + 2, Mathf.CeilToInt(width / 8f) + 2, 1), length, width, frequency, amplitude, octaves, lacunarity, persistance, wrapMode, min, max);
    //}

    #endregion Public Static Functions
    #region Private Helpers

    private static Vector3[][,,] GenerateRandomizedGradientMaps(int seed, int length, int width, int height, Continum continum, int octaves, float lacunarity)
    {
        RandomNumberGenerator rng = new RandomNumberGenerator(seed);

        Vector3[] gradients = new Vector3[16];
        for (int i = 0; i < 16; i++)
            gradients[i] = new Vector3(rng.GenerateRandomFloat() * 2 - 1, rng.GenerateRandomFloat() * 2 - 1, rng.GenerateRandomFloat() * 2 - 1).normalized;

        Vector3[][,,]  gradientMaps = new Vector3[octaves][,,];
        for (int i = 0; i < octaves; i++)
            gradientMaps[i] = GenerateRandomizedGradientMap(seed, gradients, Mathf.FloorToInt(length * Mathf.Pow(lacunarity, i)), Mathf.FloorToInt(width * Mathf.Pow(lacunarity, i)), height == 1 ? 1 : Mathf.FloorToInt(height * Mathf.Pow(lacunarity, i)), continum);

        return gradientMaps;
    }

    //private static Vector3[,,] GenerateRandomizedGradientMap(int seed, int length, int width, int height)
    //{
    //    Vector3[,,] gradients = new Vector3[length, width, height];
    //    RandomNumberGenerator rng = new RandomNumberGenerator(seed);

    //    for (int x = 0; x < length; x++)
    //        for (int y = 0; y < width; y++)
    //            for (int z = 0; z < Math.Max(1, height); z++)
    //                gradients[x, y, z] = new Vector3(rng.GenerateRandomFloat() * 2 - 1, (float)rng.GenerateRandomFloat() * 2 - 1, (float)rng.GenerateRandomFloat() * 2 - 1).normalized;

    //    return gradients;
    //}

    private static Vector3[,,] GenerateRandomizedGradientMap(int seed, Vector3[] gradients, int length, int width, int height, Continum continum)
    {
        Vector3[,,] gradientMap = new Vector3[length, width, height];
        RandomNumberGenerator rng = new RandomNumberGenerator(seed);

        bool isContinuousOnX = ((int)continum & 1) == 1;
        bool isContinuousOnY = ((int)continum & 2) == 1;
        bool isContinuousOnZ = ((int)continum & 4) == 1;

        for (int z = 0; z < height; z++)
            for (int y = 0; y < width; y++)
                for (int x = 0; x < length; x++)
                {
                    int i = rng.GenerateRandomInt(0, 15);
                    gradientMap[x, y, z] = gradients[i];

                    int xi = isContinuousOnX && x == length - 1 ? 0 : x;
                    int yi = isContinuousOnY && y == width - 1 ? 0 : y;
                    int zi = isContinuousOnZ && z == height - 1 ? 0 : z;

                    gradientMap[x, y, z] = gradientMap[xi, yi, zi];
                }
                    
        return gradientMap;
    }

    private static float SampleAt(Vector3[][,,] gradientMaps, Dimension dimension, float x, float y, int octaves, float persistance)
    {
        x -= Mathf.FloorToInt(x);
        y -= Mathf.FloorToInt(y);

        float sumNoise = 0;
        float maxSum = 0;
        float amplitude = 1;

        for (int i = 0; i < octaves; i++)
        {
            sumNoise += Make2DPerlinNoise(gradientMaps[i], x, y) * amplitude;
            maxSum += amplitude;

            amplitude *= persistance;
        }

        return (sumNoise / maxSum + 1) / 2;
    }

    private static float Make2DPerlinNoise(Vector3[,,] gradientMap, float x, float y)
    {
        int length = gradientMap.GetLength(0);
        if (length < 2)
            throw new ArgumentException("[NoiseGenerator] Degraded gradient map (x dimension)");

        int width = gradientMap.GetLength(1);
        if (width < 2)
            throw new ArgumentException("[NoiseGenerator] Degraded gradient map (y dimension)");

        x *= length - 1;
        y *= width - 1;

        int xi = Mathf.FloorToInt(x);
        int yi = Mathf.FloorToInt(y);

        float[] dotProducts = new float[4];
        for (int i = 0; i < 4; i++)
        {
            int xu = xi + (i & 1);
            int yu = yi + (i >> 1);

            dotProducts[i] = Vector2.Dot(gradientMap[xu, yu, 0], new Vector2(x - xu, y - yu));
        }

        float u = Fade(x - xi);
        float v = Fade(y - yi);

        return Mathf.Lerp(Mathf.Lerp(dotProducts[0], dotProducts[1], u), Mathf.Lerp(dotProducts[2], dotProducts[3], u), v) * SQRT2;
    }

    private static float Make3DPerlinNoise(Vector3[,,] gradientMap, float x, float y, float z, Continum continum)
    {
        int length = gradientMap.GetLength(0);
        if (length < 2)
            throw new ArgumentException("[NoiseGenerator] Degraded gradient map (x dimension)");

        int width = gradientMap.GetLength(1);
        if (width < 2)
            throw new ArgumentException("[NoiseGenerator] Degraded gradient map (y dimension)");

        int height = gradientMap.GetLength(2);
        if (height < 2)
            throw new ArgumentException("[NoiseGenerator] Degraded gradient map (z dimension)");

        bool isContinuousOnX = ((int)continum & 1) == 1;
        bool isContinuousOnY = ((int)continum & 2) == 1;
        bool isContinuousOnZ = ((int)continum & 4) == 1;

        x *= isContinuousOnX ? length : length - 1;
        y *= isContinuousOnY ? width : width - 1;
        z *= isContinuousOnZ ? height : height - 1;

        int xi = Mathf.FloorToInt(x);
        int yi = Mathf.FloorToInt(y);
        int zi = Mathf.FloorToInt(z);

        float xf = x - xi;
        float yf = y - yi;
        float zf = z - zi;

        Vector3 point = new Vector3(x, y, z);
        float[] dotProducts = new float[8];
        for (int i = 0; i < 8; i++)
        {
            int xu = xi + (i & 1);
            int yu = yi + (i >> 1);
            int zu = zi + (i >> 2);

            dotProducts[i] = Vector3.Dot(gradientMap[isContinuousOnX ? xu % length : xu, isContinuousOnY ? yu % width : yu, isContinuousOnZ ? zu % height : zu], new Vector3(xu, yu, zu) - point);
        }

        float u = Fade(xf);
        float v = Fade(yf);
        float w = Fade(zf);

        return Mathf.Lerp(Mathf.Lerp(Mathf.Lerp(dotProducts[0], dotProducts[1], u), Mathf.Lerp(dotProducts[2], dotProducts[3], u), v), Mathf.Lerp(Mathf.Lerp(dotProducts[4], dotProducts[5], u), Mathf.Lerp(dotProducts[6], dotProducts[7], u), v), w);
    }

    private static float Fade(float t)
    {
        return t * t * t * (t * (t * 6 - 15) + 10); // 6t^5 - 15t^4 + 10t^3
    }

    #endregion Private Helpers
}

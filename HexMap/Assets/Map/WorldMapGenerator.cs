using UnityEngine;
using System;
using System.Collections;

public class WorldMapGenerator
{
    private RandomNumberGenerator rng;

    [SerializeField] private static readonly int _tileSize = 4;

    private static readonly WorldMapGenerator uniqueInstance = new WorldMapGenerator();

    private WorldMapGenerator()
    {
        rng = new RandomNumberGenerator();
    }

    public static WorldMapGenerator instance
    {
        get
        {
            return uniqueInstance;
        }
    }

    public WorldMap GenerateNewMap(WorldMapConfig config)
    {
        int length = config.length;
        int width = config.width;

        if (length < 1)
            throw new ArgumentOutOfRangeException(string.Format("Invalid length ({0})", length));

        if (width < 1)
            throw new ArgumentOutOfRangeException(string.Format("Invalid width ({0})", width));

        rng.Reset(config.seed);

        HexGrid<float> heightMap = GenerateHeightMap(length, width, config.continents, config.seaLevel * 0.5f, config.terrainFrequency, config.terrainOctaves);
        HexGrid<float> biomeMap = GenerateBiomeMap(length, width, config.temperature, config.surfaceFrequency, config.surfaceOctaves, config.forestFrequency, config.forestOctaves);

        WorldMap map = new WorldMap(length, width);
        map.For(delegate (int x, int y)
        {
            map[x, y] = new WorldMapTile(x, y, heightMap[x, y], biomeMap[x, y]);
        });

        return map;
    }

    private HexGrid<float> GenerateHeightMap(int length, int width, int continents, float seaLevel, float frequency, int octaves)
    {
        HexGrid<float> heightMap = new HexGrid<float>(length, width);

        PerlinNoiseGenerater oceanGenerator = new PerlinNoiseGenerater(rng.GenerateRandomInt(), PerlinNoiseGenerater.Continum.X, 2 + continents, 2 + continents);
        PerlinNoiseGenerater heightGenerator = new PerlinNoiseGenerater(rng.GenerateRandomInt(), PerlinNoiseGenerater.Continum.X, Mathf.CeilToInt(length * frequency), Mathf.CeilToInt(width * frequency), octaves, 2, 0.5f);

        float maxX = Mathf.Sqrt(3) * _tileSize * (length - 1);
        float maxZ = 1.5f * _tileSize * (width - 1);

        heightMap.For(delegate (int x, int y)
        {
            Vector3 worldCoordinates = WorldMap.ToWorldCoordinates(x, y, _tileSize);
            float randomValue = oceanGenerator.SampleAt(Mathf.InverseLerp(0, maxX, worldCoordinates.x), Mathf.InverseLerp(0, maxZ, worldCoordinates.z), 0, 100);
            heightMap[x, y] = (randomValue > 45 - seaLevel && randomValue < 55 + seaLevel ? -1 : 1) * heightGenerator.SampleAt(Mathf.InverseLerp(0, Mathf.Sqrt(3) * _tileSize * (length - 1), worldCoordinates.x), Mathf.InverseLerp(0, 1.5f * _tileSize * (width - 1), worldCoordinates.z), 0, 100);
        });

        return heightMap;
    }

    private HexGrid<float> GenerateBiomeMap(int length, int width, float temperature, float surfaceFrequency, int surfaceOctaves, float forestFrequency, int forestOctaves)
    {
        HexGrid<float> biomeMap = new HexGrid<float>(length, width);

        float maxX = Mathf.Sqrt(3) * _tileSize * (length - 1);
        float maxZ = 1.5f * _tileSize * (width - 1);

        PerlinNoiseGenerater surfaceGenerator = new PerlinNoiseGenerater(rng.GenerateRandomInt(), PerlinNoiseGenerater.Continum.X, Mathf.CeilToInt(length * surfaceFrequency), Mathf.CeilToInt(width * surfaceFrequency), surfaceOctaves, 2, 0.5f);
        PerlinNoiseGenerater forestGenerator = new PerlinNoiseGenerater(rng.GenerateRandomInt(), PerlinNoiseGenerater.Continum.X, Mathf.CeilToInt(length * forestFrequency), Mathf.CeilToInt(width * forestFrequency), forestOctaves, 2, 0.5f);

        biomeMap.For(delegate (int x, int y)
        {
            Vector3 worldCoordinates = WorldMap.ToWorldCoordinates(x, y, _tileSize);

            float latitude = Math.Abs(WorldMap.ToGeographicCoordinates(x, y, length, width).y * 2f / width);

            float randomValue = surfaceGenerator.SampleAt(Mathf.InverseLerp(0, maxX, worldCoordinates.x), Mathf.InverseLerp(0, maxZ, worldCoordinates.z), 0, 100);
            biomeMap[x, y] = randomValue > Mathf.Lerp(50 - temperature, 80, MathUtility.CubicCurve2(latitude)) ? 0 : (randomValue < Mathf.Lerp(20 - 2 * temperature, 100, MathUtility.CubicCurve1(latitude)) ? -1 : 1) * forestGenerator.SampleAt(Mathf.InverseLerp(0, maxX, worldCoordinates.x), Mathf.InverseLerp(0, maxZ, worldCoordinates.z), 0, 100);
        });

        return biomeMap;
    }
}

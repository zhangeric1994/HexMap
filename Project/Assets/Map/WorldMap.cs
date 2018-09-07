using UnityEngine;
using System;

[Serializable] public struct WorldMapConfig
{
    [SerializeField] private readonly int _length;
    [SerializeField] private readonly int _width;
    [SerializeField] private readonly int _continents;
    [SerializeField] private readonly float _seaLevel;
    [SerializeField] private readonly float _rainfall;
    [SerializeField] private readonly float _temperature;
    [SerializeField] private readonly float _roughness;

    [SerializeField] private readonly int _seed;
    [SerializeField] private readonly float _terrainFrequency;
    [SerializeField] private readonly int _terrainOctaves;
    [SerializeField] private readonly float _surfaceFrequency;
    [SerializeField] private readonly int _surfaceOctaves;
    [SerializeField] private readonly float _forestFrequency;
    [SerializeField] private readonly int _forestOctaves;

    public int length
    {
        get
        {
            return _length;
        }
    }

    public int width
    {
        get
        {
            return _width;
        }
    }

    public int continents
    {
        get
        {
            return _continents;
        }
    }

    public float seaLevel
    {
        get
        {
            return _seaLevel;
        }
    }

    public float rainfall
    {
        get
        {
            return _rainfall;
        }
    }

    public float temperature
    {
        get
        {
            return _temperature;
        }
    }

    public float roughness
    {
        get
        {
            return _roughness;
        }
    }

    public int seed
    {
        get
        {
            return _seed;
        }
    }

    public float terrainFrequency
    {
        get
        {
            return _terrainFrequency;
        }
    }

    public int terrainOctaves
    {
        get
        {
            return _terrainOctaves;
        }
    }

    public float surfaceFrequency
    {
        get
        {
            return _surfaceFrequency;
        }
    }

    public int surfaceOctaves
    {
        get
        {
            return _surfaceOctaves;
        }
    }

    public float forestFrequency
    {
        get
        {
            return _forestFrequency;
        }
    }

    public int forestOctaves
    {
        get
        {
            return _forestOctaves;
        }
    }

    public WorldMapConfig(int length, int width, int continents, float seaLevel, float rainfall, float temperature, float roughness, int seed, float terrainFrequency, int terrainOctaves, float surfaceFrequency, int surfaceOctaves, float forestFrequency, int forestOctaves)
    {
        _length = length;
        _width = width;
        _continents = continents;
        _seaLevel = seaLevel;
        _rainfall = rainfall;
        _temperature = temperature;
        _roughness = roughness;

        _seed = seed;
        _terrainFrequency = terrainFrequency;
        _terrainOctaves = terrainOctaves;
        _surfaceFrequency = surfaceFrequency;
        _surfaceOctaves = surfaceOctaves;
        _forestFrequency = forestFrequency;
        _forestOctaves = forestOctaves;
    }
}

public class WorldMap : HexGrid<WorldMapTile>
{
    #region Variables

    private WorldMapConfig _config;

    #endregion Variables
    #region Constructors

    public WorldMap(int length, int width) : base(length, width)
    {
    }

    public WorldMap(WorldMapConfig config, HexGrid<float> heightMap) : base(config.length, config.width)
    {
        _config = config;
    }

    #endregion Constructors
    #region Public Non-static Functions

    public IntVector2 ToGeographicCoordinates(int x, int y)
    {
        return ToGeographicCoordinates(x, y, length, width);
    }

    public IntVector2 ToGeographicCoordinates(IntVector2 axialCoordinates)
    {
        return ToGeographicCoordinates(axialCoordinates, length, width);
    }

    #endregion Public Non-static Functions
    #region Public Static Functions

    public static Vector3 ToWorldCoordinates(int x, int y, float tileSize)
    {
        return new Vector3(Mathf.Sqrt(3) * tileSize * (x + y / 2f), 0, 1.5f * tileSize * y);
    }

    public static Vector3 ToWorldCoordinates(IntVector2 axialCoordinates, float tileSize)
    {
        return ToWorldCoordinates(axialCoordinates.x, axialCoordinates.y, tileSize);
    }

    public static IntVector2 ToGeographicCoordinates(int x, int y, int worldLength, int worldWidth)
    {
        x = x + y / 2 - worldLength / 2;
        y -= worldWidth / 2;

        return new IntVector2(x, y);
    }

    public static IntVector2 ToGeographicCoordinates(IntVector2 axialCoordinates, int worldLength, int worldWidth)
    {
        return ToGeographicCoordinates(axialCoordinates.x, axialCoordinates.y, worldLength, worldWidth);
    }

    public static IntVector2 ToGeographicCoordinates(WorldMapTile tile, int worldLength, int worldWidth)
    {
        return ToGeographicCoordinates(tile.u, tile.v, worldLength, worldWidth);
    }

    public int Distance(WorldMapTile A, WorldMapTile B)
    {
        return MathUtility.ChebyshevDistance(A.u, A.v, B.u, B.v);
    }

    public int Distance(WorldMapTile A, int u, int v)
    {
        return MathUtility.ChebyshevDistance(A.u, A.v, u, v);
    }

    public int Distance(WorldMapTile A, int x, int y, int z)
    {
        return MathUtility.ChebyshevDistance(A.x, A.y, A.z, x, y, z);
    }

    #endregion Public Static Functions
}

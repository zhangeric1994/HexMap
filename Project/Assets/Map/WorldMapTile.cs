using UnityEngine;
using System;
using System.Collections;

[Serializable] public class WorldMapTile
{
    public enum Landform : int
    {
        GLACIER,
        OCEAN,
        LAKE,
        MOUNTAIN,
        HILL,
        PLAIN,
        count
    }

    public enum Biome : int
    {
        TUNDRA,
        DESERT,
        GRASSLAND,
        FOREST,
        count
    }

    #region Variables

    private IntVector2 _axialCoordinates;
    private float _altitude;
    private Landform _landform;
    private float _biome;

    #endregion Variables
    #region Accessors and Mutators

    public IntVector2 axialCoordinates
    {
        get
        {
            return new IntVector2(_axialCoordinates);
        }
    }

    public IntVector3 cubeCoordinates
    {
        get
        {
            return new IntVector3(x, y, z);
        }
    }

    public int u
    {
        get
        {
            return _axialCoordinates.x;
        }
    }

    public int v
    {
        get
        {
            return _axialCoordinates.y;
        }
    }

    public int x
    {
        get
        {
            return _axialCoordinates.x;
        }
    }

    public int y
    {
        get
        {
            return -x - z;
        }
    }

    public int z
    {
        get
        {
            return _axialCoordinates.y;
        }
    }

    public float altitude
    {
        get
        {
            return _altitude;
        }
    }

    public float biome
    {
        get
        {
            return _biome;
        }
    }

    #endregion Accessors and Mutators
    #region Constructors

    public WorldMapTile(int u, int v, float altitude, float biome)
    {
        _axialCoordinates = new IntVector2(u, v);
        _altitude = altitude;

        _biome = biome;
        //if (biome < 0)
        //    _biome = Biome.TUNDRA;
        //else if (biome == 0)
        //    _biome = Biome.DESERT;
        //else if (biome > 60 - config.rainfall)
        //    _biome = Biome.FOREST;
        //else
        //    _biome = Biome.GRASSLAND;
    }

    public WorldMapTile(int x, int y, int z, float altitude, float biome) : this(x, z, altitude, biome)
    {
    }

    public WorldMapTile(WorldMapTile other)
    {
        _axialCoordinates = new IntVector2(other._axialCoordinates);
        _altitude = other.altitude;
    }

    #endregion Constructors
    #region Operators

    public static int operator -(WorldMapTile left, WorldMapTile right)
    {
        return IntVector3.ChebyshevDistance(left.cubeCoordinates, right.cubeCoordinates);
    }

    public static implicit operator IntVector2(WorldMapTile tile)
    {
        return tile.axialCoordinates;
    }

    public static implicit operator IntVector3(WorldMapTile tile)
    {
        return tile.cubeCoordinates;
    }

    #endregion Operators
    #region Public Non-static Functions

    override public string ToString()
    {
        return string.Format("({0}, {1})", u, v);
    }

    #endregion Public Non-static Functions
}

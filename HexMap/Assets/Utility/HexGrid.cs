using UnityEngine;
using System;
using System.Collections.Generic;

public enum HexGridType : int
{
    POINTY_TOPPED,
    FLAT_TOPPED
}

public enum HexGridShape : int
{
    HEXAGON,
    RECTANGLE,
    RHOMBUS,
    //TRIANGlE
}

public class HexGrid<T>
{
    #region Variables

    [SerializeField] protected HexGridShape _shape;
    [SerializeField] protected HexGridType _type;
    [SerializeField] protected IntVector2 _size;
    [SerializeField] protected T[,] _cells;

    #endregion Variables
    #region Constants

    private static readonly IntVector2[] _axialDirections = { new IntVector2( 1,  0),
                                                              new IntVector2( 1, -1),
                                                              new IntVector2( 0, -1),
                                                              new IntVector2(-1,  0),
                                                              new IntVector2(-1,  1),
                                                              new IntVector2( 0,  1) };
    private static readonly IntVector3[] _cubeDirections = { new IntVector3( 1, -1,  0),
                                                             new IntVector3( 1,  0, -1),
                                                             new IntVector3( 0,  1, -1),
                                                             new IntVector3(-1,  1,  0),
                                                             new IntVector3(-1,  0,  1),
                                                             new IntVector3( 0, -1,  1) };

    #endregion Constants
    #region Accessors and Mutators

    public T this[int x, int y]
    {
        get
        {
            return GetCell(x, y);
        }

        set
        {
            SetCell(x, y, value);
        }
    }

    public T this[IntVector2 axialCoordinates]
    {
        get
        {
            return GetCell(axialCoordinates.x, axialCoordinates.y);
        }

        set
        {
            SetCell(axialCoordinates.x, axialCoordinates.y, value);
        }
    }

    public T this[int x, int y, int z]
    {
        get
        {
            return GetCell(x, z);
        }

        set
        {
            SetCell(x, z, value);
        }
    }

    public T this[IntVector3 cubeCoordinates]
    {
        get
        {
            return GetCell(cubeCoordinates.x, cubeCoordinates.z);
        }

        set
        {
            SetCell(cubeCoordinates.x, cubeCoordinates.z, value);
        }
    }

    public int length
    {
        get
        {
            return _size.x;
        }
    }

    public int width
    {
        get
        {
            return _size.y;
        }
    }

    public int count
    {
        get
        {
            return width * width + (2 * width - 1) * (length - 1);
        }
    }

    public IntVector2[] axialDirections
    {
        get
        {
            return _axialDirections;
        }
    }

    public IntVector3[] cubeDirections
    {
        get
        {
            return _cubeDirections;
        }
    }

    #endregion Accessors and Mutators
    #region Constructors

    protected HexGrid()
    {
    }

    public HexGrid(IntVector2 size) : this(size, HexGridShape.RECTANGLE, HexGridType.POINTY_TOPPED)
    {
    }

    public HexGrid(int length, int width) : this(length, width, HexGridShape.RECTANGLE, HexGridType.POINTY_TOPPED)
    {
    }

    public HexGrid(IntVector2 size, HexGridShape shape, HexGridType type)
    {
        _shape = shape;
        _type = type;
        _size = size;

        switch (shape)
        {
            case HexGridShape.HEXAGON:
                _cells = new T[size.y, 2 * size.x + size.y - 2];
                break;
            //case HexagonGridShape.TRIANGlE:
            //    _cells = new T[size.x - (size.y & 1) + 1, (size.y >> 1) + (size.y & 1)];
            //    break;
            default:
                _cells = new T[size.y, size.x];
                break;
        }
    }

    public HexGrid(int length, int width, HexGridShape shape, HexGridType type) : this(new IntVector2(length, width), shape, type)
    {
    }

    public HexGrid(T[,] data, HexGridShape shape, HexGridType type)
    {
        _shape = shape;
        _type = type;

        switch (shape)
        {
            case HexGridShape.HEXAGON:
                int width = data.GetLength(0);
                int length = (data.GetLength(1) - width + 2) / 2;
                _size = new IntVector2(length, width);
                break;
            //case HexagonGridShape.TRIANGlE:
            //    _cells = new T[size.x - (size.y & 1) + 1, (size.y >> 1) + (size.y & 1)];
            //    int width = data.GetLength(1);
            //    int length = (data.GetLength(0) - width + 2) / 2;
            //    _size = new Int2D(length, width);
            //    break;
            default:
                _size = new IntVector2(data.GetLength(1), data.GetLength(0));
                break;
        }

        _cells = data;
    }

    #endregion Constructors
    #region Public Non-static Functions

    public bool HasNeighbor(int x, int y, int direction)
    {
        IntVector2 axialDirection = _axialDirections[direction];
        return IsValid(x + axialDirection.x, y + axialDirection.y);
    }

    public bool HasNeighbor(IntVector2 axialCoordinates, int direction)
    {
        return HasNeighbor(axialCoordinates.x, axialCoordinates.y, direction);
    }

    public bool HasNeighbor(int x, int y, int z, int direction)
    { 
        return HasNeighbor(x, z, direction);
    }

    public bool HasNeighbor(IntVector3 cubeCoordinates, int direction)
    {
        return HasNeighbor(cubeCoordinates.x, cubeCoordinates.z, direction);
    }

    public T GetNeighbor(int x, int y, int direction)
    {
        IntVector2 axialDirection = _axialDirections[direction];
        return GetCell(x + axialDirection.x, y + axialDirection.y);
    }

    public T GetNeighbor(IntVector2 axialCoordinates, int direction)
    {
        return GetNeighbor(axialCoordinates.x, axialCoordinates.y, direction);
    }

    public T GetNeighbor(int x, int y, int z, int direction)
    {
        return GetNeighbor(x, z, direction);
    }

    public T GetNeighbor(IntVector3 cubeCoordinates, int direction)
    {
        return GetNeighbor(cubeCoordinates.x, cubeCoordinates.z, direction);
    }

    public T[] GetNeighbors(int x, int y)
    {
        List<int> availableDirections = new List<int>();
        for (int direction = 0; direction < 6; direction++)
        {
            IntVector2 axialDirection = _axialDirections[direction];
            if (IsValid(x + axialDirection.x, y + axialDirection.y))
                availableDirections.Add(direction);
        }

        T[] neighbors = new T[availableDirections.Count];
        for (int i = 0; i < neighbors.Length; i++)
            neighbors[i] = GetNeighbor(x, y, availableDirections[i]);

        return neighbors;
    }

    public T[] GetNeighbors(IntVector2 axialCoordinates)
    {
        return GetNeighbors(axialCoordinates.x, axialCoordinates.y);
    }

    public T[] GetNeighbors(int x, int y, int z)
    {
        return GetNeighbors(x, z);
    }

    public T[] GetNeighbors(IntVector3 cubeCoordinates)
    {
        return GetNeighbors(cubeCoordinates.x, cubeCoordinates.z);
    }

    public void Foreach(Action<T> F)
    {
        switch (_shape)
        {
            case HexGridShape.HEXAGON:
                for (int y = -width + 1; y < width; y++)
                    for (int x = -width + 1 - Math.Min(0, y); x < length - Math.Max(0, y); x++)
                        F(GetCell(x, y));
                break;
            default:
                for (int y = 0; y < _cells.GetLength(0); y++)
                    for (int x = 0; x < _cells.GetLength(1); x++)
                        F(_cells[y, x]);
                break;
        }
    }

    public void For(Action<int, int> F)
    {
        switch (_shape)
        {
            case HexGridShape.HEXAGON:
                for (int y = -width + 1; y < width; y++)
                    for (int x = -width + 1 - Math.Min(0, y); x < length - Math.Max(0, y); x++)
                        F(x, y);
                break;
            case HexGridShape.RECTANGLE:
                for (int y = 0; y < width; y++)
                    for (int x = -(y / 2); x < length - (y / 2); x++)
                        F(x, y);
                break;
            default:
                for (int y = 0; y < width; y++)
                    for (int x = 0; x < length; x++)
                        F(x, y);
                break;
        }
    }

    #endregion Public Non-static Functions
    #region Public Static Functions

    public static IntVector3 ToCubeCoordinates(int x, int y)
    {
        return new IntVector3(x, -x - y, y);
    }

    public static IntVector3 ToCubeCoordinates(IntVector2 axialCoordinates)
    {
        return ToCubeCoordinates(axialCoordinates.x, axialCoordinates.y);
    }

    public static IntVector2 ToAxialCoordinates(int x, int y, int z)
    {
        return new IntVector2(x, z);
    }

    public static IntVector2 ToAxialCoordinates(IntVector3 cubeCoordinates)
    {
        return ToAxialCoordinates(cubeCoordinates.x, cubeCoordinates.y, cubeCoordinates.z);
    }

    public static IntVector2 GetAxialDirection(int direction)
    {
        return _axialDirections[direction];
    }

    public static IntVector3 GetCubeDirection(int direction)
    {
        return _cubeDirections[direction];
    }

    public static int Distance(IntVector3 A, IntVector3 B)
    {
        return IntVector3.ChebyshevDistance(A, B);
    }

    public static int Distance(IntVector2 A, IntVector2 B)
    {
        return MathUtility.ChebyshevDistance(A.x, -A.x - A.y, A.y, B.x, -B.x - B.y, B.y);
    }

    public static int Distance(IntVector3 A, int x, int y, int z)
    {
        return MathUtility.ChebyshevDistance(A.x, A.y, A.z, x, y, z);
    }

    public static int Distance(IntVector2 A, int x, int y)
    {
        return MathUtility.ChebyshevDistance(A.x, A.y, x, y);
    }

    public static int Distance(int xA, int yA, int zA, int xB, int yB, int zB)
    {
        return MathUtility.ChebyshevDistance(xA, yA, zA, xB, yB, zB);
    }

    public static int Distance(int xA, int yA, int xB, int yB)
    {
        return MathUtility.ChebyshevDistance(xA, -xA - yA, yA, xB, -xB - yB, yB);
    }

    #endregion Public Static Functions
    #region Private Helper Functions

    private bool IsValid(int x, int y)
    {
        switch (_shape)
        {
            case HexGridShape.HEXAGON:
                return Math.Abs(y) < width && x > -width - Math.Min(0, y) && x < length - Math.Max(0, y);
            case HexGridShape.RECTANGLE:
                return y >= 0 && y < width && x >= 0 - y / 2 && x < length - y / 2;
            default:
                return x >= 0 && x < length && y >= 0 && y < width;
        }
    }

    private T GetCell(int x, int y)
    {
        if (!IsValid(x, y))
            throw new IndexOutOfRangeException(string.Format("[HexagonGrid] Invalid coordinates ({0}, {1})", x, y));

        switch (_shape)
        {
            case HexGridShape.HEXAGON:
                if (y < 0)
                    return _cells[width + y, length + width - 2 + x];
                return _cells[y, width - 1 + x];
            case HexGridShape.RECTANGLE:
                return _cells[y, x + y / 2];
            //case HexagonGridShape.TRIANGlE:
            //    if (y < (width >> 1) + (width & 1))
            //        return _cells[x, y];
            //    return _cells[length - 1 - x, width + (width & 1) - 1 - y];
            default:
                return _cells[y, x];
        }
    }

    private void SetCell(int x, int y, T value)
    {
        if (!IsValid(x, y))
            throw new IndexOutOfRangeException(string.Format("[HexagonGrid] Invalid coordinates ({0}, {1})", x, y));

        switch (_shape)
        {
            case HexGridShape.HEXAGON:
                if (y < 0)
                    _cells[width + y, length + width - 2 + x] = value;
                else
                    _cells[y, width - 1 + x] = value;
                break;
            case HexGridShape.RECTANGLE:
                _cells[y, x + y / 2] = value;
                break;
            //case HexagonGridShape.TRIANGlE:
            //    if (y < (width >> 1) + (width & 1))
            //        return _cells[x, y];
            //    return _cells[length - 1 - x, width + (width & 1) - 1 - y];
            default:
                _cells[y, x] = value;
                break;
        }
    }

    #endregion Private Helper Functions
}


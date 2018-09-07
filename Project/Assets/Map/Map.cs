using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Diagnostics;
using System;

public enum MapComponentType : int
{
    TARGET_POINT = 0,
    BIRTH_POINT,
    PATH,
}

public class MapComponent : ScriptableObject
{
    private MapComponentType _type;
    public MapComponentType type
    {
        get
        {
            return _type;
        }

        set
        {
            _type = value;
        }
    }

    private int[] _pos;
    public int[] pos
    {
        get
        {
            return _pos;
        }
    }
    public int x
    {
        get
        {
            return _pos[0];
        }
    }
    public int y
    {
        get
        {
            return _pos[1];
        }
    }
    public int xc
    {
        get
        {
            return _pos[0] + _size[0] / 2;
        }
    }
    public int yc
    {
        get
        {
            return _pos[1] + _size[1] / 2;
        }
    }
    public int[] center
    {
        get
        {
            return new int[2] { xc, yc };
        }
    }

    private int[] _size;
    public int[] size
    {
        get
        {
            return _size;
        }
    }
    public int width
    {
        get
        {
            return _size[0];
        }
    }
    public int height
    {
        get
        {
            return _size[1];
        }
    }

    public MapComponent(MapComponentType type, int[] pos, int[] size)
    {
        _type = type;
        _pos = pos;
        _size = size;
    }

    public MapComponent(MapComponentType type, int[] pos, int width, int height) : this(type, pos, new int[2] { width, height })
    {
    }

    public MapComponent(MapComponentType type, int x, int y, int[] size) : this(type, new int[2] { x, y }, size)
    {
    }

    public MapComponent(MapComponentType type, int x, int y, int width, int height) : this(type, new int[2] { x, y }, new int[2] { width, height })
    {
    }
}

public enum MapType : int
{
    A = 0,
}

public class MapConfig
{
    /// <summary>
    /// 
    /// </summary>
    private MapType _type;
    public MapType type
    {
        get
        {
            return _type;
        }

        set
        {
            _type = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private int[] _size;
    public int[] size
    {
        get
        {
            return _size;
        }

        set
        {
            _size = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private int _seed;
    public int seed
    {
        get
        {
            return _seed;
        }

        set
        {
            _seed = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private float _frequency;
    public float frequency
    {
        get
        {
            return _frequency;
        }

        set
        {
            _frequency = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private float _amplitude;
    public float amplitude
    {
        get
        {
            return _amplitude;
        }

        set
        {
            _amplitude = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private float _persistance;
    public float persistance
    {
        get
        {
            return _persistance;
        }

        set
        {
            _persistance = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private int _octaves;
    public int octaves
    {
        get
        {
            return _octaves;
        }

        set
        {
            _octaves = value;
        }
    }

    private int _bushLevel;
    public int bushLevel
    {
        get
        {
            return _bushLevel;
        }

        set
        {
            _bushLevel = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private int[] _targetPointSize;
    public int[] targetPointSize
    {
        get
        {
            return _targetPointSize;
        }

        set
        {
            _targetPointSize = value;
        }
    }
    public int targetPointWidth
    {
        get
        {
            return _targetPointSize[0];
        }

        set
        {
            _targetPointSize[0] = value;
        }
    }
    public int targetPointHeight
    {
        get
        {
            return _targetPointSize[1];
        }

        set
        {
            _targetPointSize[1] = value;
        }
    }

    private int _numBirthPoints;
    public int numBirthPoints
    {
        get
        {
            return _numBirthPoints;
        }

        set
        {
            _numBirthPoints = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private int[] _birthPointSize;
    public int[] birthPointSize
    {
        get
        {
            return _birthPointSize;
        }

        set
        {
            _birthPointSize = value;
        }
    }
    public int birthPointWidth
    {
        get
        {
            return _birthPointSize[0];
        }

        set
        {
            _birthPointSize[0] = value;
        }
    }
    public int birthPointHeight
    {
        get
        {
            return _birthPointSize[1];
        }

        set
        {
            _birthPointSize[1] = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private int _pathBreadth;
    public int pathBreadth
    {
        get
        {
            return _pathBreadth;
        }

        set
        {
            _pathBreadth = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private float _pathCurvity;
    public float pathCurvity
    {
        get
        {
            return _pathCurvity;
        }

        set
        {
            _pathCurvity = value;
        }
    }

    public MapConfig()
    {
        _type = MapType.A;
        _size = new int[2] { 127, 127 };
        _seed = (int)Decimal.Divide(DateTime.UtcNow.Ticks - 621355968000000000, 10000000);

        _frequency = 10;
        _amplitude = 5;
        _persistance = 5;
        _octaves = 4;

        _targetPointSize = new int[2] { 5, 5 };

        _numBirthPoints = 2;
        _birthPointSize = new int[2] { 5, 5 };

        _pathBreadth = 3;
    }
}

public class Map
{
    [SerializeField] private MapConfig _config;
    public MapType type
    {
        get
        {
            return _config.type;
        }
    }
    public int[] size
    {
        get
        {
            return _config.size;
        }
    }
    public int seed
    {
        get
        {
            return _config.seed;
        }
    }
    public int width
    {
        get
        {
            return _config.size[0];
        }
    }
    public int height
    {
        get
        {
            return _config.size[1];
        }
    }
    public float frequency
    {
        get
        {
            return _config.frequency;
        }
    }
    public float amplitude
    {
        get
        {
            return _config.amplitude;
        }
    }
    public float persistance
    {
        get
        {
            return _config.persistance;
        }
    }
    public int octaves
    {
        get
        {
            return _config.octaves;
        }
    }
    public int bushLevel
    {
        get
        {
            return _config.bushLevel;
        }
    }
    public int[] targetPointSize
    {
        get
        {
            return _config.targetPointSize;
        }
    }
    public int numBirthPoints
    {
        get
        {
            return _config.numBirthPoints;
        }
    }
    public int[] birthPointSize
    {
        get
        {
            return _config.birthPointSize;
        }
    }
    public int pathBreadth
    {
        get
        {
            return _config.pathBreadth;
        }
    }
    public float pathCurvity
    {
        get
        {
            return _config.pathCurvity;
        }
    }

    private Dictionary<MapComponentType, List<MapComponent>> _components;
    private MapComponent[,] _map;
    [SerializeField] private float[,] _altitude;

    private System.Random rng = null;
    private int rngCount = 0;

    public Map()
    {
        _config = new MapConfig();

        InitializeRNG();
    }

    public Map(MapConfig config)
    {
        _config = config;

        InitializeMapComponents();
    }

    public void DrawMap(Renderer canvas, int tileSize, int bushLevel)
    {
        Color[] colorMap = new Color[width * height];

        UnityEngine.Debug.Log("Drawing ...\n");
        Stopwatch timer = new Stopwatch();
        timer.Start();

        DrawTerrain(colorMap);
        DrawPath(colorMap);
        DrawBirthPoint(colorMap);
        DrawTargetpoint(colorMap);

        timer.Stop();
        UnityEngine.Debug.Log(string.Format("Finished in {0} ms\n", timer.ElapsedMilliseconds));

        Color[] hdColorMap = new Color[width * tileSize * height * tileSize];
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                for (int dx = 0; dx < tileSize; dx++)
                    for (int dy = 0; dy < tileSize; dy++)
                        hdColorMap[x * tileSize + dx + (y * tileSize + dy) * width * tileSize] = colorMap[x + y * width];

        Texture2D texture = new Texture2D(width * tileSize, height * tileSize);
        texture.SetPixels(hdColorMap);
        texture.Apply();

        canvas.sharedMaterial.mainTexture = texture;
    }

    public int GenerateRandomInt(int min, int max)
    {
        if (rngCount % 20 == 0)
            rng = new System.Random(seed + rngCount / 20);

        rngCount++;

        return rng.Next(min, max);
    }

    public float GenerateRandomFloat(float min, float max)
    {
        if (rngCount % 20 == 0)
            rng = new System.Random(seed + rngCount / 20);

        rngCount++;
        float x = (float)rng.NextDouble();

        return min + x * (max - min);
    }

    private void DrawSquaredComponent(Color[] colorMap, Color color, MapComponentType type)
    {
        foreach (MapComponent component in _components[type])
            for (int dx = 0; dx < component.width; dx++)
                for (int dy = 0; dy < component.height; dy++)
                {
                    int x = component.x + dx;
                    int y = component.y + dy;
                    if (x >= 0 && x < width && y >= 0 && y < height)
                        colorMap[x + y * width] = color;
                }
    }

    private void DrawTerrain(Color[] colorMap)
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                colorMap[x + y * width] = _altitude[x, y] > (1 - bushLevel * 0.01) ? Color.green : Color.black;
    }

    private void DrawPath(Color[] colorMap)
    {
        DrawSquaredComponent(colorMap, Color.grey, MapComponentType.PATH);
    }

    private void DrawBirthPoint(Color[] colorMap)
    {
        DrawSquaredComponent(colorMap, Color.magenta, MapComponentType.BIRTH_POINT);
    }

    private void DrawTargetpoint(Color[] colorMap)
    {
        DrawSquaredComponent(colorMap, Color.red, MapComponentType.TARGET_POINT);
    }

    private void InitializeMapComponents()
    {
        _map = new MapComponent[width, height];
        _components = new Dictionary<MapComponentType, List<MapComponent>>();

        InitializeRNG();

        UnityEngine.Debug.Log("Initializing ...\n");
        Stopwatch timer = new Stopwatch();

        timer.Start();
        InitializeAltitude(seed, frequency, amplitude, octaves, persistance);
        timer.Stop();
        UnityEngine.Debug.Log(string.Format("Initialize Altitude: {0} ms\n", timer.ElapsedMilliseconds));

        timer.Reset();
        timer.Start();
        InitializeTargetPoint(targetPointSize);
        timer.Stop();
        UnityEngine.Debug.Log(string.Format("Initialize Target Point: {0} ms\n", timer.ElapsedMilliseconds));

        timer.Reset();
        timer.Start();
        InitializeBirthPoint(numBirthPoints, birthPointSize);
        timer.Stop();
        UnityEngine.Debug.Log(string.Format("Initialize Birth Point: {0} ms\n", timer.ElapsedMilliseconds));

        timer.Reset();
        timer.Start();
        InitializePath(pathBreadth, pathCurvity);
        timer.Stop();
        UnityEngine.Debug.Log(string.Format("Initialize Path: {0} ms\n", timer.ElapsedMilliseconds));
    }

    private void InitializeAltitude(int seed, float frequency, float amplitude, int octaves, float persistance)
    {
        _altitude = NoiseMap.Generate2DPerlinNoiseMap(seed, width, height, frequency, amplitude, octaves, persistance);
    }

    private void InitializeTargetPoint(int[] componentSize)
    {
        _components[MapComponentType.TARGET_POINT] = new List<MapComponent>();
        _components[MapComponentType.TARGET_POINT].Add(new MapComponent(MapComponentType.TARGET_POINT, width / 2 - (componentSize[0] - 1) / 2, height / 2 - (componentSize[1] - 1) / 2, componentSize));
    }

    private void InitializeBirthPoint(int num, int[] componentSize)
    {
        _components[MapComponentType.BIRTH_POINT] = new List<MapComponent>();

        int[,][] possiblePoints = new int[3, 3][];
        for (int x = 0; x < 3; x++)
            for (int y = 0; y < 3; y++)
                possiblePoints[x, y] = new int[2] { ((width - 1) * x) / 2 - ((componentSize[0] - 1) * x) / 2, ((height - 1) * y) / 2 - ((componentSize[1] - 1) * y) / 2 };

        List<int> points = new List<int>();
        while (points.Count < num)
        {
            int point = 4;
            while (point == 4 || points.Contains(point))
                point = GenerateRandomInt(0, 8);

            bool isValid = true;
            foreach (int previousPoint in points)
            {
                int diffX = Mathf.Abs((previousPoint % 3) - (point % 3));
                int diffY = Mathf.Abs(previousPoint - point) / 3;
                if (diffY == 1 || (diffY == 0 && diffX == 1))
                {
                    isValid = false;
                    break;
                }
            }

            if (isValid)
            {
                int[] position = possiblePoints[point % 3, point / 3];
                MapComponent birthPoint = new MapComponent(MapComponentType.BIRTH_POINT, position[0], position[1], componentSize);

                points.Add(point);
                _components[MapComponentType.BIRTH_POINT].Add(birthPoint);
            }
        }
    }

    private void InitializePath(int breadth, float curvity)
    {
        _components[MapComponentType.PATH] = new List<MapComponent>();

        MapComponent targetPoint = _components[MapComponentType.TARGET_POINT][0];

        int[] center = targetPoint.center;
        int[] centerSize = targetPoint.size;

        int[,][] endPoints = new int[3, 3][];
        for (int x = 0; x < 3; x++)
            for (int y = 0; y < 3; y++)
                endPoints[x, y] = new int[2] { center[0] + (x - 1) * (centerSize[0] / 2), center[1] + (y - 1) * (centerSize[1] / 2) };

        for (int x = 0; x < 3; x++)
            for (int y = 0; y < 3; y++)
                if (x != 1 || y != 1)
                {
                    int[] startPoint = new int[2] { ((width - 1) * x) / 2, ((height - 1) * y) / 2 };

                    //MapComponent pathStart = new MapComponent(MapComponentType.PATH, startPoint[0] - breadth / 2, startPoint[1] - breadth / 2, breadth, breadth);
                    //MapComponent pathEnd = new MapComponent(MapComponentType.PATH, endPoints[x, y], breadth, breadth);

                    //_components[MapComponentType.PATH].Add(pathStart);
                    //_components[MapComponentType.PATH].Add(pathEnd);

                    MakePath(startPoint, endPoints[x, y], breadth, curvity);
                }
    }

    private void MakePath(int[] startPoint, int[] endPoint, int breadth, float curvity)
    {
        int[] min = new int[2] { Mathf.Min(startPoint[0], endPoint[0]), Mathf.Min(startPoint[1], endPoint[1]) };

        int length = Mathf.Abs(startPoint[0] - endPoint[0]);
        int width = Mathf.Abs(startPoint[1] - endPoint[1]);
        int[,] distance = new int[length, width];

        for (int xi = 0; xi < length; xi++)
            for (int yi = 0; yi < width; yi++)
            {
                int x = xi + min[0];
                int y = yi + min[1];
                distance[xi, yi] = _altitude[x, y] > bushLevel ? -1 : Mathf.Abs(x - endPoint[0]) + Mathf.Abs(y - endPoint[1]);
            }
        
        

        //int[] diff = new int[2] { Mathf.Abs(startPoint[0] - endPoint[0]), Mathf.Abs(startPoint[1] - endPoint[1]) };
        //if (diff[0] < 2 && diff[1] < 2)
        //    return;

        //int i = diff[0] >= diff[1] ? 0 : 1;
        //int j = (i + 1) % 2;

        //float noise = Mathf.Max(0.75f, diff[j] * 0.05f * curvity);

        //int[] midPoint = new int[2];
        //midPoint[i] = (startPoint[i] + endPoint[i]) / 2;
        //midPoint[j] = (startPoint[j] + endPoint[j]) / 2 + Mathf.RoundToInt(GenerateRandomFloat(-noise, noise));

        //MakePath(startPoint, midPoint, breadth, curvity * 0.7f);
        //MakePath(midPoint, endPoint, breadth, curvity * 0.7f);

        //MapComponent path = new MapComponent(MapComponentType.PATH, midPoint[0] - breadth / 2, midPoint[1] - breadth / 2, breadth, breadth);
        //_components[MapComponentType.PATH].Add(path);
    }

    //private void DiamondSquare(int xmin, int ymin, int size, float roughness)
    //{
    //    if (size == 1)
    //        return;

    //    int xmid = xmin + size / 2;
    //    int xmax = xmin + size;
    //    int ymid = ymin + size / 2;
    //    int ymax = ymin + size;

    //    float nw = _altitude[xmin, ymin];
    //    float ne = _altitude[xmin, ymax];
    //    float sw = _altitude[xmax, ymin];
    //    float se = _altitude[xmax, ymax];

    //    _altitude[xmid, ymid] = (nw + ne + sw + se) / 4f + Random.Range(-0.05f, 0.05f) * roughness;

    //    Square(xmid, ymin, size / 2, roughness);
    //    Square(xmid, ymax, size / 2, roughness);
    //    Square(xmin, ymid, size / 2, roughness);
    //    Square(xmax, ymid, size / 2, roughness);

    //    DiamondSquare(xmin, ymin, size / 2, roughness);
    //    DiamondSquare(xmin, ymid, size / 2, roughness);
    //    DiamondSquare(xmid, ymin, size / 2, roughness);
    //    DiamondSquare(xmid, ymid, size / 2, roughness);
    //}

    //private void Square(int x, int y, int size, float roughness)
    //{
    //    int num = 0;
    //    float sum = 0;

    //    if (x - size >= 0)
    //    {
    //        sum += _altitude[x - size, y];
    //        num++;
    //    }

    //    if (x + size < width)
    //    {
    //        sum += _altitude[x + size, y];
    //        num++;
    //    }

    //    if (y - size >= 0)
    //    {
    //        sum += _altitude[x, y - size];
    //        num++;
    //    }

    //    if (y + size < height)
    //    {
    //        sum += _altitude[x, y + size];
    //        num++;
    //    }

    //    _altitude[x, y] = sum / num + Random.Range(-0.05f, 0.05f) * roughness;
    //}

    private void InitializeRNG()
    {
        rngCount = 0;
    }
}


using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

public class WorldMapGeneratorEditor : EditorWindow
{
    public static WorldMapGenerator worldMapGenerator = WorldMapGenerator.instance;

    private WorldMap map;
    private WorldMapConfig config;
    public int length;
    public int width;
    public int seed;
    public int continents;
    public float seaLevel;
    public float rainfall;
    public float temperature;
    public float roughness;

    public float terrainFrequency;
    public int terrainOctaves;
    [Range(1, 10)] public float terrainLacunarity;
    [Range(0, 1)] public float terrainPersistance;
    public float surfaceFrequency;
    public int surfaceOctaves;
    public float forestFrequency;
    public int forestOctaves;

    private GameObject canvas;
    private WorldMapDrawer mapDrawer;
    public float tileSize;
    private MeshRenderer[][] tilePrefabs;
    public MeshRenderer glacierPrefab;
    public MeshRenderer oceanPrefab;
    //public MeshRenderer lakePrefab;
    public MeshRenderer[] lakePrefabs;
    public MeshRenderer forestPrefab;
    public MeshRenderer[] mountainPrefabs;
    public MeshRenderer[] hillPrefabs;
    public MeshRenderer[] plainPrefabs;

    private SerializedObject serializedObject;

    [MenuItem("Tools/生成WorldMap")]
    public static WorldMapGeneratorEditor Init()
    {
        WorldMapGeneratorEditor window = (WorldMapGeneratorEditor)EditorWindow.GetWindow(typeof(WorldMapGeneratorEditor));
        window.Reset();
        window.Show();
        return window;
    }

    private void OnEnable()
    {
        serializedObject = new SerializedObject(this);

        tilePrefabs = new MeshRenderer[(int)WorldMapTile.Landform.count][];
        tilePrefabs[(int)WorldMapTile.Landform.GLACIER] = new MeshRenderer[1] { glacierPrefab };
        tilePrefabs[(int)WorldMapTile.Landform.OCEAN] = new MeshRenderer[1] { oceanPrefab };
        // tilePrefabs[(int)WorldMapTile.Landform.LAKE] = new MeshRenderer[1] { lakePrefab };
        tilePrefabs[(int)WorldMapTile.Landform.LAKE] = lakePrefabs;
        tilePrefabs[(int)WorldMapTile.Landform.MOUNTAIN] = mountainPrefabs;
        tilePrefabs[(int)WorldMapTile.Landform.HILL] = hillPrefabs;
        tilePrefabs[(int)WorldMapTile.Landform.PLAIN] = plainPrefabs;
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();

        EditorGUILayout.Separator();

        length = EditorGUILayout.IntField("Length", length);
        width = EditorGUILayout.IntField("Width", width);
        continents = EditorGUILayout.IntSlider("Continent", continents, 0, 4);
        seaLevel = EditorGUILayout.Slider("Sea Level", seaLevel, 0, 10);
        rainfall = EditorGUILayout.Slider("Rainfall", rainfall, 0, 10);
        temperature = EditorGUILayout.Slider("Temperature", temperature, 0, 10);
        roughness = EditorGUILayout.Slider("Roughness", roughness, 0, 10);

        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Randomization", EditorStyles.boldLabel);

        seed = EditorGUILayout.IntField("Seed", seed);
        if (GUILayout.Button("Randomize"))
            RandomizeNewSeed();

        EditorGUILayout.LabelField("Terrain", EditorStyles.boldLabel);
        terrainFrequency = EditorGUILayout.Slider("Frequency", terrainFrequency, 0.001f, 0.2f);
        terrainOctaves = EditorGUILayout.IntSlider("Octaves", terrainOctaves, 1, 4);
        terrainLacunarity = EditorGUILayout.Slider("Lacunarity", terrainLacunarity, 1, 10);
        terrainPersistance = EditorGUILayout.Slider("Persistance", terrainPersistance, 0, 1);

        EditorGUILayout.LabelField("Surface", EditorStyles.boldLabel);
        surfaceFrequency = EditorGUILayout.Slider("Frequency", surfaceFrequency, 0.001f, 0.2f);
        surfaceOctaves = EditorGUILayout.IntSlider("Octaves", surfaceOctaves, 1, 4);

        EditorGUILayout.LabelField("Forest", EditorStyles.boldLabel);
        forestFrequency = EditorGUILayout.Slider("Frequency", forestFrequency, 0.001f, 0.2f);
        forestOctaves = EditorGUILayout.IntSlider("Octaves", forestOctaves, 1, 4);

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Preview", EditorStyles.largeLabel);

        mapDrawer = (WorldMapDrawer)EditorGUILayout.ObjectField("Map Drawer", mapDrawer, typeof(WorldMapDrawer), true);

        tileSize = EditorGUILayout.Slider("Tile Size", tileSize, 1, 32);

        glacierPrefab = (MeshRenderer)EditorGUILayout.ObjectField("Glacier", glacierPrefab, typeof(MeshRenderer), true);
        oceanPrefab = (MeshRenderer)EditorGUILayout.ObjectField("Ocean", oceanPrefab, typeof(MeshRenderer), true);
        //lakePrefab = (MeshRenderer)EditorGUILayout.ObjectField("Lake", lakePrefab, typeof(MeshRenderer), true);
        forestPrefab = (MeshRenderer)EditorGUILayout.ObjectField("Forest", forestPrefab, typeof(MeshRenderer), true);

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("lakePrefabs"), new GUIContent("Lake"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("mountainPrefabs"), new GUIContent("Mountain"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("hillPrefabs"), new GUIContent("Hill"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("plainPrefabs"), new GUIContent("Plain"), true);
        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        if (GUILayout.Button("Generate"))
        {
            Generate();
            Preview();
        }

        if (GUILayout.Button("Generate Random Map"))
        {
            RandomizeNewSeed();
            Generate();
            Preview();
        }

        //if (GUILayout.Button("预览"))
        //    Preview(map);

        //if (GUILayout.Button("生成并预览"))
        //    Preview(Generate());

        if (GUILayout.Button("Reset to Default"))
            Reset();
    }

    public void OnInspectorUpdate()
    {
        Repaint();
    }

    private void Generate()
    {
        config = new WorldMapConfig(length, width, continents, seaLevel, rainfall, temperature, roughness, seed, terrainFrequency, terrainOctaves, surfaceFrequency, surfaceOctaves, forestFrequency, forestOctaves);
        map = worldMapGenerator.GenerateNewMap(config);
    }

    private void Preview()
    {
        if (map == null)
            return;

        if (canvas != null && !canvas.Equals(null))
            DestroyImmediate(canvas);

        UnityEngine.Random.InitState(seed);

        canvas = new GameObject();
        canvas.name = "Canvas";

        //map.For(delegate (int x, int y)
        //{
        //    DrawTilePreview(x, y);
        //});

        if (mapDrawer)
        {
            mapDrawer.Initialize();
            mapDrawer.Draw(map, seed, terrainOctaves, terrainLacunarity, terrainPersistance);
        }

        //int L = length + width - 1;
        //int W = 2 * width - 1;

        //PerlinNoiseGenerater noise = new PerlinNoiseGenerater(Mathf.CeilToInt(L / 4f) + 2, Mathf.CeilToInt(W / 4f) + 2, 1, seed);

        //MeshRenderer canvas = GameObject.CreatePrimitive(PrimitiveType.Plane).GetComponent<MeshRenderer>();
        //Color[] colors = new Color[L * W];
        //for (int y = 0; y < W; y++)
        //    for (int x = 0; x < L; x++)
        //    {
        //        colors[x + y * L] = Color.Lerp(Color.white, Color.black, noise.SampleAt((float)x / (L - 1), (float)y / (W - 1), frequency, amplitude, octaves, persistance));
        //        Debug.Log(colors[x + y * L]);
        //    }

        //Texture2D texture = new Texture2D(L, W);
        //texture.SetPixels(colors);
        //texture.Apply();

        //canvas.sharedMaterial.mainTexture = texture;
        //canvas.transform.localScale = new Vector3(L * tileSize, 0, W * tileSize);
    }

    private void Reset()
    {
        length = 400;
        width = 300;
        continents = 2;
        seaLevel = 5;
        rainfall = 5;
        temperature = 5;
        roughness = 5;

        RandomizeNewSeed();
        terrainFrequency = 0.05f;
        terrainOctaves = 3;
        surfaceFrequency = 0.05f;
        surfaceOctaves = 2;
        forestFrequency = 0.1f;
        forestOctaves = 2;

        tileSize = 1;
    }

    private void RandomizeNewSeed()
    {
        UnityEngine.Random.InitState(TimeUtility.localTime);
        seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
    }

    private int ReadTileType(WorldMapTile tile)
    {
        int tileType = 0;

        float latitude = Math.Abs(WorldMap.ToGeographicCoordinates(tile, length, width).y * 2f / width);

        int numLandform = (int)WorldMapTile.Landform.count;
        if (tile.v < 2 || tile.v > width - 3)
        {
            BitOperationUtility.WriteBit(ref tileType, (int)WorldMapTile.Landform.GLACIER, 1);
            tileType += (int)WorldMapTile.Landform.GLACIER << 16;
        }
        else if (tile.altitude < 0)
        {
            BitOperationUtility.WriteBit(ref tileType, (int)WorldMapTile.Landform.OCEAN, 1);
            tileType += (int)WorldMapTile.Landform.OCEAN << 16;
        }
        else if (tile.altitude < 25 + rainfall * 2)
        {
            BitOperationUtility.WriteBit(ref tileType, (int)WorldMapTile.Landform.LAKE, 1);
            tileType += (int)WorldMapTile.Landform.LAKE << 16;
        }
        else if (tile.altitude > 65 - roughness)
        {
            BitOperationUtility.WriteBit(ref tileType, (int)WorldMapTile.Landform.MOUNTAIN, 1);
            tileType += (int)WorldMapTile.Landform.MOUNTAIN << 16;
        }
        else if (tile.altitude > 60 - roughness)
        {
            BitOperationUtility.WriteBit(ref tileType, (int)WorldMapTile.Landform.HILL, 1);
            tileType += (int)WorldMapTile.Landform.HILL << 16;
        }
        else
        {
            BitOperationUtility.WriteBit(ref tileType, (int)WorldMapTile.Landform.PLAIN, 1);
            tileType += (int)WorldMapTile.Landform.PLAIN << 16;
        }

        if (tile.biome < 0 && latitude > 0.55f + temperature * 0.025f)
        {
            BitOperationUtility.WriteBit(ref tileType, numLandform + (int)WorldMapTile.Biome.TUNDRA, 1);
            tileType += (int)WorldMapTile.Biome.TUNDRA << 19;
        }
        else if (tile.biome == 0 && latitude < 0.35f + temperature * 0.025f)
        {
            BitOperationUtility.WriteBit(ref tileType, numLandform + (int)WorldMapTile.Biome.DESERT, 1);
            tileType += (int)WorldMapTile.Biome.DESERT << 19;
        }
        else if (tile.biome > 60 - rainfall)
        {
            BitOperationUtility.WriteBit(ref tileType, numLandform + (int)WorldMapTile.Biome.FOREST, 1);
            tileType += (int)WorldMapTile.Biome.FOREST << 19;
        }
        else
        {
            BitOperationUtility.WriteBit(ref tileType, numLandform + (int)WorldMapTile.Biome.GRASSLAND, 1);
            tileType += (int)WorldMapTile.Biome.GRASSLAND << 19;
        }

        return tileType;
    }

    private void DrawTilePreview(int x, int y)
    {
        WorldMapTile tile = map[x, y];
        int tileType = ReadTileType(tile);

        WorldMapTile.Landform landform = (WorldMapTile.Landform)((tileType >> 16) & 7);
        WorldMapTile.Biome biome = (WorldMapTile.Biome)(tileType >> 19);

        MeshRenderer[] landformPrefabs = tilePrefabs[(int)landform];
        MeshRenderer tilePrefab = landformPrefabs[Math.Min(landformPrefabs.Length - 1, (int)biome)];

        Transform cell = Instantiate(tilePrefab).transform;
        cell.parent = canvas.transform;
        cell.localPosition = WorldMap.ToWorldCoordinates(x, y, tileSize);
        cell.localScale *= tileSize;
        cell.gameObject.name = tile.ToString();

        if (biome == WorldMapTile.Biome.FOREST)
        {
            if (landform == WorldMapTile.Landform.PLAIN)
            {
                Transform forest = Instantiate(forestPrefab).transform;
                forest.parent = cell;
                forest.localPosition = Vector3.zero;
                forest.localScale = Vector3.one;
                forest.Rotate(0, 0, UnityEngine.Random.Range(0, 360));
                forest.gameObject.name = "Forest";
            }
            else if (landform == WorldMapTile.Landform.HILL)
            {
                Transform forest = Instantiate(forestPrefab).transform;
                forest.parent = cell;
                forest.localPosition = new Vector3(0, 0, 0.2f);
                forest.localScale = 0.9f * Vector3.one;
                forest.Rotate(0, 0, UnityEngine.Random.Range(0, 360));
                forest.gameObject.name = "Forest";
            }
        }

        //Transform wrappedTile = Instantiate(tilePrefab).transform;
        //wrappedTile.parent = canvas.transform;
        //wrappedTile.localPosition = WorldMap.ToWorldCoordinates(x + length, y, tileSize) + new Vector3(tileSize, 0, 0);
        //wrappedTile.localScale *= tileSize;
        //wrappedTile.gameObject.name = "~" + tile.ToString();
    }
}

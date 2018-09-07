using UnityEngine;
using UnityEditor;
using System.Collections;

public class MapGenerator : EditorWindow
{
    public Vector2 size;

    public int seed;

    public float frequency;
    public float amplitude;
    public float persistance;
    public int octaves;
    public int bushLevel;

    public Vector2 targetPointSize;

    public int numBirthPoints;
    public Vector2 birthPointSize;

    public int pathBreadth;
    public float pathCurvity;

    public int tileSize;
    public Renderer canvas;

    private Map map;

    [MenuItem("Tools/Scene 生成")]
    public static MapGenerator Init()
    {
        MapGenerator window = (MapGenerator)EditorWindow.GetWindow(typeof(MapGenerator));
        window.Reset();
        window.Show();
        return window;
    }

    void OnGUI()
    {
        EditorGUILayout.BeginVertical();

        EditorGUILayout.LabelField("地图生成工具");

        EditorGUILayout.LabelField("");
        size = EditorGUILayout.Vector2Field("地图大小", size);
        seed = EditorGUILayout.IntField("地图种子", seed);

        EditorGUILayout.LabelField("");
        EditorGUILayout.LabelField("地形");
        frequency = EditorGUILayout.Slider("frequency", frequency, 0.05f, 1);
        amplitude = EditorGUILayout.Slider("amplitude", amplitude, 0, 10);
        persistance = EditorGUILayout.Slider("persistance", persistance, 0.5f, 2);
        octaves = EditorGUILayout.IntSlider("octaves", octaves, 1, 6);
        bushLevel = EditorGUILayout.IntSlider("草丛", bushLevel, 10, 50);

        EditorGUILayout.LabelField("");
        EditorGUILayout.LabelField("目标点");
        targetPointSize = EditorGUILayout.Vector2Field("目标点大小", targetPointSize);

        EditorGUILayout.LabelField("");
        EditorGUILayout.LabelField("出生点");
        numBirthPoints = EditorGUILayout.IntField("出生点个数", numBirthPoints);
        birthPointSize = EditorGUILayout.Vector2Field("出生点大小", birthPointSize);

        EditorGUILayout.LabelField("");
        EditorGUILayout.LabelField("道路");
        pathBreadth = EditorGUILayout.IntField("道路宽度", pathBreadth);
        pathCurvity = EditorGUILayout.Slider("道路曲度", pathCurvity, 0f, 10f);

        EditorGUILayout.LabelField("");
        EditorGUILayout.LabelField("预览");
        tileSize = EditorGUILayout.IntSlider("分辨率等级", tileSize, 1, 32);

        EditorGUILayout.LabelField("");
        EditorGUILayout.LabelField("");
        EditorGUILayout.LabelField("");

        if (GUILayout.Button("生成"))
            Generate();

        if (GUILayout.Button("预览"))
            Preview(map);

        if (GUILayout.Button("生成并预览"))
            Preview(Generate());

        if (GUILayout.Button("恢复预设"))
            Reset();
    }

    private Map Generate()
    {
        MapConfig config = new MapConfig();
        config.type = MapType.A;
        config.size = new int[2] { (int)size.x, (int)size.y };
        config.seed = seed;
        config.frequency = frequency;
        config.amplitude = amplitude;
        config.persistance = persistance;
        config.octaves = octaves;
        config.bushLevel = bushLevel;
        config.targetPointWidth = (int)targetPointSize.x;
        config.targetPointHeight = (int)targetPointSize.y;
        config.numBirthPoints = numBirthPoints;
        config.birthPointWidth = (int)birthPointSize.x;
        config.birthPointHeight = (int)birthPointSize.y;
        config.pathBreadth = pathBreadth;
        config.pathCurvity = pathCurvity;

        map =  new Map(config);
        return map;
    }

    private void Preview(Map map)
    {
        if (map == null)
            return;

        if (!canvas)
        {
            canvas = GameObject.CreatePrimitive(PrimitiveType.Plane).GetComponent<MeshRenderer>();
            canvas.transform.forward = new Vector3(0, 1, 0);
        }

        map.DrawMap(canvas, tileSize, bushLevel);
        canvas.transform.localScale = new Vector3(size.x, 1, size.y);
    }

    private void Reset()
    {
        size.x = 127;
        size.y = 127;

        seed = 0;

        frequency = 0.5f;
        amplitude = 2;
        persistance = 0.8f;
        octaves = 1;
        bushLevel = 20;

        targetPointSize.x = 5;
        targetPointSize.y = 5;

        numBirthPoints = 2;
        birthPointSize.x = 5;
        birthPointSize.y = 5;

        pathBreadth = 3;
        pathCurvity = 5f;

        tileSize = 16;
    }
}

using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

public class NoiseGeneratorEditor : EditorWindow
{
    [SerializeField] int _size;

    [SerializeField] int _seed;
    [SerializeField] int _length;
    [SerializeField] int _width;
    [SerializeField] int _octaves;
    [SerializeField] float _lacunarity;
    [SerializeField] float _persistance;

    private SerializedObject _serializedObject;

    private Renderer _renderer;

    [MenuItem("Tools/Noise Generator")]
    public static NoiseGeneratorEditor Init()
    {
        NoiseGeneratorEditor window = (NoiseGeneratorEditor)EditorWindow.GetWindow(typeof(NoiseGeneratorEditor));
        window.Show();
        return window;
    }

    private void OnEnable()
    {
        _serializedObject = new SerializedObject(this);
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();

        EditorGUILayout.Separator();

        _size = EditorGUILayout.IntField("Size", _size);

        _seed = EditorGUILayout.IntField("Random Seed", _seed);
        _length = EditorGUILayout.IntSlider("Length", _length, 2, 100);
        _width = EditorGUILayout.IntSlider("Width", _width, 2, 100);
        _octaves = EditorGUILayout.IntSlider("Octaves", _octaves, 1, 6);
        _lacunarity = EditorGUILayout.Slider("Lacunarity", _lacunarity, 0, 4);
        _persistance = EditorGUILayout.Slider("Persistance", _persistance, 0, 1);

        EditorGUILayout.Separator();

        if (GUILayout.Button("Generate"))
            Generate2DPerlinNoise(_seed);

        if (GUILayout.Button("Generate Random 2D Perlin Noise"))
        {
            GenerateRandomSeed();
            Generate2DPerlinNoise(_seed);
        }
    }

    public void OnInspectorUpdate()
    {
        Repaint();
    }

    private void GenerateRandomSeed()
    {
        _seed = TimeUtility.localTime;
    }

    private void Generate2DPerlinNoise(int seed)
    {
        PerlinNoiseGenerater noiseGenerator = new PerlinNoiseGenerater(seed, PerlinNoiseGenerater.Continum.X, _length, _width, _octaves, _lacunarity, _persistance);

        Color[] colors = new Color[_size * _size];
        for (int y = 0; y < _size; y++)
            for (int x = 0; x < _size; x++)
                colors[x + y * _size] = Color.Lerp(Color.white, Color.black, noiseGenerator.SampleAt(x / (float)_size, y / (float)_size));

        if (!_renderer)
            _renderer = GameObject.CreatePrimitive(PrimitiveType.Plane).GetComponent<MeshRenderer>();

        Texture2D texture = new Texture2D(_size, _size);
        texture.SetPixels(colors);
        texture.Apply();

        _renderer.transform.position = Vector3.zero;
        _renderer.transform.localRotation = Quaternion.Euler(-90, 0, 0);
        _renderer.transform.localScale = new Vector3(1000, 1, 1000);

        _renderer.sharedMaterial.mainTexture = texture;
    }
}

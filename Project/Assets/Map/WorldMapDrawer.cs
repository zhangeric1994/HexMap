using UnityEngine;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class WorldMapDrawer : MonoBehaviour
{
    private Mesh _mesh;
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;

    private List<Vector3> _vertices;
    private List<Vector2> _uvs;
    private List<Color> _colors;
    private List<int> _indices;

    private WorldMap _worldMap;

    int length = 38;
    int width = 22;

    private const float DEG_10 = Mathf.PI / 18;
    private const float DEG_20 = Mathf.PI / 9;
    private const float DEG_30 = Mathf.PI / 6;
    private const float DEG_40 = Mathf.PI * 2 / 9;
    private const float DEG_60 = Mathf.PI / 3;

    private void Awake()
    {
        Initialize();
    }

    public void Draw(WorldMap worldMap, int seed, int ocataves, float lacunarity, float persistance)
    {
        _worldMap = worldMap;

        float _tileSize = 10;

        //worldMap.Foreach(delegate (WorldMapTile tile)
        //{
        //    DrawMapTile(tile, _vertices, _colors, _uvs, _indices, _tileSize);
        //});

        RandomNumberGenerator rng = new RandomNumberGenerator(seed);
        PerlinNoiseGenerater heightGenerator = new PerlinNoiseGenerater(rng.GenerateRandomInt(), PerlinNoiseGenerater.Continum.X, Mathf.CeilToInt(length * 10 * 0.05f), Mathf.CeilToInt(width * 10 * 0.05f), ocataves, lacunarity, persistance);

        for (int y = 0; y < width; y++)
            for (int x = -(y / 2); x < length - (y / 2); x++)
                DrawHexagonAt(WorldMap.ToWorldCoordinates(x, y, _tileSize), heightGenerator,  _vertices, _colors, _uvs, _indices, _tileSize);

        _mesh.Clear();
        _mesh.SetVertices(_vertices);
        _mesh.SetColors(_colors);
        _mesh.SetUVs(0, _uvs);
        _mesh.SetTriangles(_indices, 0);
        _mesh.RecalculateNormals();

        _meshFilter.mesh = _mesh;
    }

    public void Initialize()
    {
        _mesh = new Mesh();
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();

        _vertices = new List<Vector3>();
        _uvs = new List<Vector2>();
        _colors = new List<Color>();
        _indices = new List<int>();
    }

    //private void DrawMapTile(WorldMapTile tile, List<Vector3> vertices, List<Color> colors, List<Vector2> uvs, List<int> indices, float tileSize)
    //{
    //    DrawHexagonAt(WorldMap.ToWorldCoordinates(tile.axialCoordinates, tileSize), vertices, colors, uvs, indices, tileSize);
    //}

    private void DrawHexagonAt(Vector3 center, PerlinNoiseGenerater heightMap, List<Vector3> vertices, List<Color> colors, List<Vector2> uvs, List<int> indices, float size)
    {
        int centerIndex = vertices.Count;

        float centerH = heightMap.SampleAt(Mathf.InverseLerp(0, 4000, center.x), Mathf.InverseLerp(0, 2000, center.z));

        vertices.Add(Elevate(center, centerH));

        Color c = GetSurfaceColor(center, centerH);
        //Debug.Log(c);
        colors.Add(c);

        uvs.Add(new Vector2(center.x / 100, center.z / 100));

        for (int i = 0; i < 6; i++)
        {
            Vector3 position = center + size / 2 * new Vector3(Mathf.Sin(i * DEG_60), 0, Mathf.Cos(i * DEG_60));

            float h = heightMap.SampleAt(Mathf.InverseLerp(0, 4000, position.x), Mathf.InverseLerp(0, 2000, position.z));

            vertices.Add(Elevate(position, h));

            colors.Add(GetSurfaceColor(position, h));

            uvs.Add(new Vector2(position.x / 100, position.z / 100));

            indices.Add(centerIndex);
            indices.Add(centerIndex + 1 + i);
            indices.Add(centerIndex + 1 + (i + 1) % 6);
        }

        for (int i = 0; i < 6; i++)
        {
            Vector3 position1 = center + size * new Vector3(Mathf.Sin(i * DEG_60), 0, Mathf.Cos(i * DEG_60));
            Vector3 position2 = center + size * Mathf.Cos(DEG_30) * new Vector3(Mathf.Sin(DEG_30 + i * DEG_60), 0, Mathf.Cos(DEG_30 + i * DEG_60));

            float h1 = heightMap.SampleAt(Mathf.InverseLerp(0, 4000, position1.x), Mathf.InverseLerp(0, 2000, position1.z));
            float h2 = heightMap.SampleAt(Mathf.InverseLerp(0, 4000, position2.x), Mathf.InverseLerp(0, 2000, position2.z));

            vertices.Add(Elevate(position1, h1));
            vertices.Add(Elevate(position2, h2));

            colors.Add(GetSurfaceColor(position1, h1));
            colors.Add(GetSurfaceColor(position2, h2));

            uvs.Add(new Vector2(position1.x / 100, position1.z / 100));
            uvs.Add(new Vector2(position2.x / 100, position2.z / 100));

            indices.Add(centerIndex + 7 + i * 2 + 1);
            indices.Add(centerIndex + 1 + i);
            indices.Add(centerIndex + 7 + i * 2);

            indices.Add(centerIndex + 7 + i * 2 + 1);
            indices.Add(centerIndex + 1 + (i + 1) % 6);
            indices.Add(centerIndex + 1 + i);

            indices.Add(centerIndex + 7 + i * 2 + 1);
            indices.Add(centerIndex + 7 + (i * 2 + 2) % 12);
            indices.Add(centerIndex + 1 + (i + 1) % 6);
        }

        //for (int i = 0; i < 6; i++)
        //{
        //    Vector3 position1 = center + size * new Vector3(Mathf.Sin(i * DEG_60), 0, Mathf.Cos(i * DEG_60));
        //    Vector3 position2 = center + size * Mathf.Cos(DEG_30) / Mathf.Cos(DEG_10) * new Vector3(Mathf.Sin(DEG_20 + i * DEG_60), 0, Mathf.Cos(DEG_20 + i * DEG_60));
        //    Vector3 position3 = center + size * Mathf.Cos(DEG_30) / Mathf.Cos(DEG_10) * new Vector3(Mathf.Sin(DEG_40 + i * DEG_60), 0, Mathf.Cos(DEG_40 + i * DEG_60));

        //    vertices.Add(AddHeight(position1, heightMap));
        //    vertices.Add(AddHeight(position2, heightMap));
        //    vertices.Add(AddHeight(position3, heightMap));

        //    //if (tile.biome == 0)
        //    //    colors.Add(new Color(1, 1, 1));
        //    //else
        //    //    colors.Add(new Color(0, 0, 0));

        //    uvs.Add(new Vector2(position1.x / 100, position1.z / 100));
        //    uvs.Add(new Vector2(position2.x / 100, position2.z / 100));
        //    uvs.Add(new Vector2(position3.x / 100, position3.z / 100));

        //    indices.Add(centerIndex + 19 + i * 3 + 1);
        //    indices.Add(centerIndex + 7 + i * 2);
        //    indices.Add(centerIndex + 19 + i * 3);

        //    indices.Add(centerIndex + 19 + i * 3 + 1);
        //    indices.Add(centerIndex + 7 + i * 2 + 1);
        //    indices.Add(centerIndex + 7 + i * 2);

        //    indices.Add(centerIndex + 19 + i * 3 + 1);
        //    indices.Add(centerIndex + 19 + i * 3 + 2);
        //    indices.Add(centerIndex + 7 + i * 2 + 1);

        //    indices.Add(centerIndex + 19 + i * 3 + 2);
        //    indices.Add(centerIndex + 7 + (i * 2 + 2) % 12);
        //    indices.Add(centerIndex + 7 + i * 2 + 1);

        //    indices.Add(centerIndex + 19 + i * 3 + 2);
        //    indices.Add(centerIndex + 19 + (i * 3 + 3) % 18);
        //    indices.Add(centerIndex + 7 + (i * 2 + 2) % 12);
        //}
    }

    private Vector3 Elevate(Vector3 position, float h)
    {
        return position + new Vector3(0, h < 0.5 ? Mathf.Pow(h, 5) * 100 - 3.125f : Mathf.Pow(h - 0.5f, 2) * 500, 0);
    }

    private Color GetSurfaceColor(Vector3 position, float h)
    {
        return Color.red;
    }
}

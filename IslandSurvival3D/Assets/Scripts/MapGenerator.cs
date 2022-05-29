using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("PerlinNoise")]
    public Vector3Int mapSize;
    public int seed;
    public int scale;
    public int octaves;
    [Range(0f,1f)]
    public float persistance;
    public int lacunarity;
    public Vector2 offset;
    [Range(0f, 1f)]
    public float falloffStart;
    [Range(0f, 1f)]
    public float fallofEnd;
    [Header("Island")]
    public Terrain terrain;
    public bool autoUpdate;
    public GameObject prefabTree;

    public void GenerateMap()
    {
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }

    private TerrainData GenerateTerrain(TerrainData terrainData)
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapSize.x, mapSize.z, seed, scale, octaves, persistance, lacunarity, offset,falloffStart,fallofEnd);
        terrainData.heightmapResolution = mapSize.x + 1;
        terrainData.size = new Vector3(mapSize.x, mapSize.y, mapSize.z);
        terrainData.SetHeights(0, 0, noiseMap);
        return terrainData;
    }

    private void GenerageGameObject(GameObject prefab,float minHeight, float maxHeight,int count)
    {

    }

    private void OnValidate()
    {
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        if(octaves < 0)
        {
            octaves = 0;
        }
    }
}

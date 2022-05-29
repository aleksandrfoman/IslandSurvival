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
    public Gradient gradient;
    public LayerMask islandLayerMask;

    [Header("GenerateObject")]
    public GenerateObjectStruct[] generateObjects;
    public List<GameObject> gameObjcts;


    private void Start()
    {
        GenerateMap();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GenerateMap();
        }
    }
    public void GenerateMap()
    {
        terrain.terrainData = GenerateTerrain(terrain.terrainData);

        ResetGameObject();
        for (int i = 0; i < generateObjects.Length; i++)
        {
            for (int k = 0; k < generateObjects[i].count; k++)
            {
                gameObjcts.Add(GenerateGameObject(generateObjects[i].prefab, generateObjects[i].minHeight, generateObjects[i].maxHeight));
            }
        }
    }

    private void ResetGameObject()
    {
        foreach (var item in gameObjcts)
        {
            Destroy(item);
        }
        gameObjcts.Clear();
    }

    private TerrainData GenerateTerrain(TerrainData terrainData)
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapSize.x, mapSize.z, seed, scale, octaves, persistance, lacunarity, offset,falloffStart,fallofEnd);
        terrainData.heightmapResolution = mapSize.x + 1;
        terrainData.size = new Vector3(mapSize.x, mapSize.y, mapSize.z);
        terrainData.SetHeights(0, 0, noiseMap);
        return terrainData;
    }

    private GameObject GenerateGameObject(GameObject prefab,float minHeight, float maxHeight)
    {
        RaycastHit hit;
        bool spawnded = false;
        do
        {
            Vector3 rndPos = new Vector3(Random.Range(-mapSize.x / 2, mapSize.x / 2), 500f, Random.Range(-mapSize.z / 2, mapSize.z / 2));

            if (Physics.Raycast(rndPos, Vector3.down, out hit, Mathf.Infinity, islandLayerMask))
            {
                if (hit.point.y >= minHeight && hit.point.y <= maxHeight)
                {
                    spawnded = true;
                }
            }
        }
        while (!spawnded);
        return Instantiate(prefab, hit.point, Quaternion.identity);
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

[System.Serializable]
public struct GenerateObjectStruct
{
    public string name;
    public GameObject prefab;
    public int count;
    public float minHeight;
    public float maxHeight;
    public Transform parent;
}
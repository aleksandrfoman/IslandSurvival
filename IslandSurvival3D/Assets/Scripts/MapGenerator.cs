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
    [Header("Grass")]
    [SerializeField]
    [Range(0,128)]
    int patchDetail;
    [SerializeField]
    [Range(256,2048)]
    int grassDensity;
    [SerializeField]
    [Range(0f, 1f)]
    float grassAmount;
    [SerializeField]
    private int minHeightGrass;
    [SerializeField]
    private int maxHeightGrass;
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
        GenGrass();
        ResetGameObject();
        
        for (int i = 0; i < generateObjects.Length; i++)
        {
            for (int k = 0; k < generateObjects[i].count; k++)
            {
                GameObject generateObject = GenerateGameObject(generateObjects[i].prefab, generateObjects[i].minHeight, generateObjects[i].maxHeight);
                if (generateObject != null)
                {
                    gameObjcts.Add(generateObject);
                }
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
        int tempSpawn = 0;

        do
        {
            Vector3 rndPos = new Vector3(Random.Range(-mapSize.x / 2, mapSize.x / 2), 500f, Random.Range(-mapSize.z / 2, mapSize.z / 2));

            if (Physics.Raycast(rndPos, Vector3.down, out hit, Mathf.Infinity, islandLayerMask))
            {
                if (hit.point.y >= minHeight && hit.point.y <= maxHeight)
                {
                    if (hit.collider != null)
                    {
                        if (hit.transform.GetComponent<Terrain>() != null)
                        {
                            spawnded = true;
                        }
                    }
                }
            }
            tempSpawn++;
            if (tempSpawn > 25)
            {
                return null;
            }
        }
        while (!spawnded);
        GameObject curreGameObject = Instantiate(prefab, hit.point, Quaternion.identity);
        curreGameObject.transform.localEulerAngles += Vector3.up * Random.Range(0, 360f);
        curreGameObject.transform.localScale *= Random.Range(0.95f, 1.2f);
        return curreGameObject;
    }

    public void GenGrass()
    {
        var terrainToPopulate = terrain;
        terrainToPopulate.terrainData.SetDetailResolution(grassDensity, patchDetail);
        var terrainData = terrainToPopulate.terrainData;
        int[,] newMap = new int[terrainToPopulate.terrainData.alphamapWidth, terrainToPopulate.terrainData.alphamapHeight];
        for (int x = 0; x < terrainToPopulate.terrainData.alphamapWidth; x++)
        {
            for (int y = 0; y < terrainToPopulate.terrainData.alphamapHeight; y++)
            {
                float y_01 = (float)y / (float)terrainData.alphamapHeight;
                float x_01 = (float)x / (float)terrainData.alphamapWidth;
                Vector3 normal = terrainData.GetInterpolatedNormal(y_01, x_01);

                // Sample the height at this location (note GetHeight expects int coordinates corresponding to locations in the heightmap array)
                float height = terrainData.GetHeight(Mathf.RoundToInt(y_01 * terrainData.heightmapResolution), Mathf.RoundToInt(x_01 * terrainData.heightmapResolution));
                //&& Vector3.Angle(normal, Vector3.up) < 40
                if (height > minHeightGrass && height < maxHeightGrass)
                {
                    if (Random.Range(0f, 1f) < grassAmount)
                    {
                        newMap[x, y] = 1;
                    }
                }
            }
        }
        terrainToPopulate.terrainData.SetDetailLayer(0, 0, 0, newMap);
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
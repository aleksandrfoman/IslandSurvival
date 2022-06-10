using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private GenerateObjectStruct[] generateObjects;
    [Header("PerlinNoise")]
    [SerializeField]
    private Vector3Int mapSize;
    [SerializeField]
    private int scale;
    [SerializeField]
    private int octaves;
    [Range(0f,1f)]
    [SerializeField]
    private float persistance;
    [SerializeField]
    private int lacunarity;
    [SerializeField]
    private Vector2 offset;
    [Range(0f, 1f)]
    [SerializeField]
    private float falloffStart;
    [Range(0f, 1f)]
    [SerializeField]
    private float fallofEnd;

    [Header("Island")]
    [SerializeField]
    private Terrain terrain;
    [SerializeField]
    private bool autoUpdate;
    [SerializeField]
    private Gradient gradient;
    [SerializeField]
    private LayerMask islandLayerMask;
    [Header("Grass")]
    [Range(0,128)]
    [SerializeField]
    private int patchDetail;
    [Range(256,2048)]
    [SerializeField]
    private int grassDensity;
    [SerializeField]
    [Range(0f, 1f)]
    private float grassAmount;
    [SerializeField]
    private int minHeightGrass;
    [SerializeField]
    private int maxHeightGrass;
    [SerializeField]
    private List<GameObject> gameObjcts;
    

    private void Start()
    {
        GenerateMap(PlayerPrefs.GetInt("Seed", 0));
    }

    public void GenerateMap(int seedGame)
    {
        System.Random prng = new System.Random(seedGame);

        terrain.terrainData = GenerateTerrain(terrain.terrainData,seedGame);
        GenGrass(prng);

        ResetGameObject();

        for (int i = 0; i < generateObjects.Length; i++)
        {
            for (int k = 0; k < generateObjects[i].count; k++)
            {
                GameObject gameObject = GenerateGameObject(generateObjects[i].prefab, generateObjects[i].minHeight, generateObjects[i].maxHeight,
                                                           generateObjects[i].parentTransform, generateObjects[i].minScale, generateObjects[i].maxScale,prng);
                if (gameObject != null)
                {
                    gameObjcts.Add(gameObject);
                }
            }
        }
    }

    private void ResetGameObject()
    {
        foreach (var item in gameObjcts)
        {
            if(item.TryGetComponent(out Collider collider))
            {
                collider.enabled = false;
            }
            Destroy(item);
        }
        gameObjcts.Clear();
    }

    private TerrainData GenerateTerrain(TerrainData terrainData, int seedGame)
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapSize.x, mapSize.z, seedGame, scale, octaves, persistance, lacunarity, offset,falloffStart,fallofEnd);
        terrainData.heightmapResolution = mapSize.x + 1;
        terrainData.size = new Vector3(mapSize.x, mapSize.y, mapSize.z);
        terrainData.SetHeights(0, 0, noiseMap);
        return terrainData;
    }

    private GameObject GenerateGameObject(GameObject prefab,float minHeight, float maxHeight,Transform parent, float minScale, float maxScale,System.Random prng)
    {

        RaycastHit hit;
        bool spawnded = false;
        int tempSpawn = 0;
        
        do
        {
            Vector3 rndPos = new Vector3(prng.NextFloat(-mapSize.x / 2, mapSize.x / 2), 500f, prng.NextFloat(-mapSize.z / 2, mapSize.z / 2));

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
            if (tempSpawn > 50)
            {
                return null;
            }
        }
        while (!spawnded);

        GameObject curreGameObject = Instantiate(prefab, hit.point, Quaternion.identity,parent.transform);
        curreGameObject.transform.localEulerAngles += Vector3.up * prng.NextFloat(0f, 360f);
        curreGameObject.transform.localScale *= prng.NextFloat(minScale, maxScale);

        return curreGameObject;
    }

    public void GenGrass(System.Random prng)
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
                    if (prng.NextFloat(0f, 1f) < grassAmount)
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
    public Transform parentTransform;
    public float minScale;
    public float maxScale;
}
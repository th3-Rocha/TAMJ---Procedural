using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System.Diagnostics;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{
    public float noiseScale = 50f;
    public bool isAsyncGen = false;
    public bool isNoMeshGen = false;
    public GameObject TerrainPrefab;
    [Range(1, 18)]
    public int terrainDistView = 1;
    public int octaves = 4;
    [Range(0, 1)]
    public float persistance = 0.243f;
    public float lacunarity = 2f;
    public int seed;
    public TerrainType[] regions;
    public Transform playerPos;
    private Vector2Int playerActivePos;
    private Vector2Int lastPlayerActivePos = Vector2Int.zero;
    public Dictionary<Vector2Int, GameObject> activeTerrains = new Dictionary<Vector2Int, GameObject>();

    int scaling = 10;


    [ContextMenu("GenStartPack")]
    void Start()
    {
        InitializeActiveTerrains();
        GenTerrainZero();
        StartCoroutine(GenerateAllActiveMaps());
    }
    void Update()
    {
        ActiveTerrainsAroundPlayer();
    }

    [ContextMenu("Test ActiveTerrainsAround")]
    public void ActiveTerrainsAroundPlayer()
    {
        scaling = (VaribleGlobal.worldSize * 1) / 64;
        playerActivePos.x = Mathf.RoundToInt(playerPos.position.x / (VaribleGlobal.worldSize * scaling));
        playerActivePos.y = Mathf.RoundToInt(playerPos.position.z / (VaribleGlobal.worldSize * scaling));
        if (playerActivePos == lastPlayerActivePos)
        {
            return;
        }

        Stopwatch stopwatchMeshUpdate = new Stopwatch();
        stopwatchMeshUpdate.Start();

        HashSet<Vector2Int> newActiveTerrainsPositions = new HashSet<Vector2Int>();
        for (int row = -terrainDistView; row <= terrainDistView; row++)
        {
            for (int col = -terrainDistView; col <= terrainDistView; col++)
            {
                Vector2Int newPosition = CalculateActiveTerrainPosition(row, col);
                newActiveTerrainsPositions.Add(newPosition);

                if (!activeTerrains.ContainsKey(newPosition))
                {
                    GenOneChunck(newPosition);
                }
            }
        }

        // Remove inactive terrains
        var positionsToRemove = new HashSet<Vector2Int>(activeTerrains.Keys);
        positionsToRemove.ExceptWith(newActiveTerrainsPositions);
        foreach (var position in positionsToRemove)
        {
            Destroy(activeTerrains[position]);
            activeTerrains.Remove(position);
            //yield return null; repensar essa logica
        }

        stopwatchMeshUpdate.Stop();
        UnityEngine.Debug.Log("Time spent updating terrains: " + stopwatchMeshUpdate.ElapsedMilliseconds + " ms");
        lastPlayerActivePos = playerActivePos;

    }

    public void GenOneChunck(Vector2Int posToInstantie)
    {
        GameObject newTerrain = Instantiate(TerrainPrefab, new Vector3(posToInstantie.x, 0, posToInstantie.y), Quaternion.identity);
        activeTerrains[posToInstantie] = newTerrain;
        CombineAndCreate CAC = newTerrain.GetComponent<CombineAndCreate>();
        CAC.isAsyncGen = isAsyncGen;
        CAC.isDefaultPlane = isNoMeshGen;
        CAC.Gen(); //esta função demora em media 5ms a 8ms
                   //faz aparecer o chunk e espera proximo frame para ar o proximo
    }


    private void InitializeActiveTerrains()
    {

        for (int row = -terrainDistView; row <= terrainDistView; row++)
        {
            for (int col = -terrainDistView; col <= terrainDistView; col++)
            {
                Vector2Int position = CalculateActiveTerrainPosition(row, col);
                GameObject newTerrain = Instantiate(TerrainPrefab, new Vector3(position.x, 0, position.y), Quaternion.identity);
                activeTerrains.Add(position, newTerrain);
            }
        }

    }


    private Vector2Int CalculateActiveTerrainPosition(int rowOffset, int colOffset)
    {
        scaling = (VaribleGlobal.worldSize * 1) / 64;
        int xPos = (playerActivePos.x + colOffset) * VaribleGlobal.worldSize * scaling;
        int zPos = (playerActivePos.y + rowOffset) * VaribleGlobal.worldSize * scaling;
        return new Vector2Int(xPos, zPos);
    }
    private Vector2Int CalculateOldActiveTerrainPosition(int rowOffset, int colOffset)
    {
        scaling = (VaribleGlobal.worldSize * 1) / 64;
        int xPos = (lastPlayerActivePos.x + colOffset) * VaribleGlobal.worldSize * scaling;
        int zPos = (lastPlayerActivePos.y + rowOffset) * VaribleGlobal.worldSize * scaling;
        return new Vector2Int(xPos, zPos);
    }


    public IEnumerator GenerateAllActiveMaps()
    {
        for (int row = -terrainDistView; row <= terrainDistView; row++)
        {
            for (int col = -terrainDistView; col <= terrainDistView; col++)
            {
                Vector2Int Position = CalculateActiveTerrainPosition(row, col);
                activeTerrains.TryGetValue(Position, out GameObject terrain);
                terrain.transform.position = new Vector3(Position.x, 0, Position.y);
                terrain.GetComponent<CombineAndCreate>().Gen();
                yield return null;
            }
        }
        UnityEngine.Debug.Log($"Terrenos inicias Criados!");
    }
    public void GenTerrainZero()
    {
        activeTerrains.TryGetValue(new Vector2Int(0, 0), out GameObject terrain);
        terrain.transform.position = new Vector3(0, 0, 0);
        terrain.GetComponent<CombineAndCreate>().Gen();
    }
    public float[,] GenTerrainNoise(Vector2 offsetToGen)
    {
        return Noise.GenerateNoiseMap(seed, noiseScale, octaves, persistance, lacunarity, offsetToGen);
    }

    void OnValidate()
    {
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        if (octaves < 0)
        {
            octaves = 0;
        }
    }
}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public float minHeight;
    public Color colour;
    public GameObject regionObj;
    [Range(0f, 1f)] public float spawnProbability; // Probability for spawning this type
}

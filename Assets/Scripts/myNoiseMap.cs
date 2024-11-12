using UnityEngine;

public class myNoiseMap : MonoBehaviour
{
    public MapGenerator mapGenerator;
    public float[,] noiseMap;
    void Awake()
    {

    }

    public void reGenNoise(Vector2Int myPosGrid)
    {
        mapGenerator = FindAnyObjectByType<MapGenerator>();
        noiseMap = mapGenerator.GenTerrainNoise(-myPosGrid * (VaribleGlobal.worldSize)); //convert mynubmer to index
    }

    void Update()
    {

    }
}
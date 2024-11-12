using UnityEditor;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public myNoiseMap myNoiseMap;
    public float treeRate = 10f; // Controls tree density
    public GameObject treePrefab;
    public Vector2 areaSize = new Vector2(50, 50); // Adjust based on your terrain size
    public float scale = 1f; // Scale factor for positioning

    public void TreeGen()
    {
   
        if (myNoiseMap == null || treePrefab == null)
        {
            Debug.LogWarning("Please assign a noise map and tree prefab.");
            return;
        }

        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }

        float[,] noiseMap = myNoiseMap.noiseMap;

        for (int x = 0; x < noiseMap.GetLength(0); x++)
        {
            for (int y = 0; y < noiseMap.GetLength(1); y++)
            {
                float noiseValue = noiseMap[x, y];

             
                if (noiseValue * 100 < treeRate)
                {
                    Vector3 spawnPosition = new Vector3(x * scale, 0, y * scale);
                    GameObject tree = Instantiate(treePrefab, spawnPosition, Quaternion.identity);
                    tree.transform.parent = transform; 
                }
            }
        }
    }
}

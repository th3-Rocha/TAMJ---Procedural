using Unity.Mathematics;
using UnityEngine;
using System.Collections;

public class ObjSpawner : MonoBehaviour
{
    public myNoiseMap myNoiseMap;
    public MeshDrawer meshDrawer;
    public MyGridPos myGridPos;
    public int ObjRate = 10; // Object density
    public TerrainType[] regions;

    public float scale = 1f; // Scale factor for positioning
    private float amplitude;
    private System.Random random;

    public void StartObjGen()
    {
        StartCoroutine(ObjGen());
    }

    private IEnumerator ObjGen()
    {
        float[,] noise = myNoiseMap.noiseMap;
        int seed = noise[0, 0].GetHashCode(); // Use hash of the seed value from noise
        random = new System.Random(seed); // Initialize System.Random with seed
        amplitude = meshDrawer.amplitude;

        // Check to ensure required components are assigned
        if (myNoiseMap == null || regions == null || regions.Length == 0)
        {
            Debug.LogWarning("Please assign a noise map and define terrain regions.");
            yield break;
        }

        // Set minHeight for each region based on the previous region's height
        SetRegionMinHeights();

        for (int z = 0; z < VaribleGlobal.worldSize; z += ObjRate)
        {
            for (int x = 0; x < VaribleGlobal.worldSize; x += ObjRate)
            {
                // Calculate the position
                float heightValue = noise[x, z];
                float yOffset = amplitude * heightValue;
                float posX = gameObject.transform.position.x + VaribleGlobal.worldSize / 2;
                float posZ = gameObject.transform.position.z + VaribleGlobal.worldSize / 2;
                Vector3 position = new Vector3(posX - x, gameObject.transform.position.y + yOffset, posZ - z);

                // Determine which prefab to spawn based on height
                GameObject selectedPrefab = SelectPrefabForExactHeight(heightValue);
                if (selectedPrefab != null)
                {
                    // Instantiate with a random Y rotation using custom random instance
                    GenOne(selectedPrefab, position, new Vector3(0, random.Next(0, 360), 0));
                }
                
                // Yield control to the next frame after each instantiation
                yield return null;
            }
        }
    }

    private void SetRegionMinHeights()
    {
        float previousHeight = 0f;
        for (int i = 0; i < regions.Length; i++)
        {
            regions[i].minHeight = previousHeight;
            previousHeight = regions[i].height;
        }
    }

    private GameObject SelectPrefabForExactHeight(float heightValue)
    {
        foreach (var region in regions)
        {
            // Ensure height matches exactly within the region bounds
            if (heightValue > region.minHeight && heightValue <= region.height)
            {
                // Check spawn probability using custom random instance
                if (random.NextDouble() <= region.spawnProbability)
                {
                    return region.regionObj;
                }
            }
        }
        return null; // Return null if no prefab meets the criteria
    }

    public void GenOne(GameObject prefab, Vector3 pos, Vector3 rot)
    {
        Instantiate(prefab, pos, quaternion.EulerXYZ(rot), transform);
    }
}

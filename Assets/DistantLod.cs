using UnityEngine;

public class DistantLod : MonoBehaviour
{
    public int distanceView = 1; 
    public string layerLod = "LOD_OBJ"; 
    public Transform player; 

    private int layerLodIndex;

    public void Start()
    {
       
        layerLodIndex = LayerMask.NameToLayer(layerLod);
        
        if (player == null)
        {
            Debug.LogWarning("Player Transform not assigned! Please assign it in the inspector.");
        }
    }

    public void Update()
    {
     
        GameObject[] lodObjects = GameObject.FindGameObjectsWithTag(layerLod);

        foreach (GameObject obj in lodObjects)
        {
            float distance = Vector3.Distance(player.position, obj.transform.position);

            if (distance <= distanceView)
            {
            

            }
            else
            {
        
                obj.SetActive(false);
            }
        }
    }
}

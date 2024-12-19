using UnityEngine;

public class PlayerDist : MonoBehaviour
{
    public bool isPlayerNear = false;
    public float triggerDistance = 100f; // Using float for more precise control over distance
    public genCave Cave;
    public bool isGenered = false;
    private Transform playerTransform;

    void Start()
    {
        // Find the player GameObject by tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogWarning("Player not found in the scene.");
        }
    }

    void FixedUpdate()
    {
        if (!isGenered)
        {
            if (playerTransform != null)
            {
                // Check the distance between this object and the player
                float distance = Vector3.Distance(transform.position, playerTransform.position);
                isPlayerNear = distance <= triggerDistance;

                // Call the function if the player is near
                if (isPlayerNear)
                {
                    Cave.genCaveWithCollider();
                    isGenered = true;
                }
            }
        }
    }
}

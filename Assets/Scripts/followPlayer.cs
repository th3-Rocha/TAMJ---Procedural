using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player; // Reference to the player's Transform

    void Update()
    {
        // Get the current position
        Vector3 newPosition = transform.position;

        // Set x and z to follow the player's x and z
        newPosition.x = player.position.x;
        newPosition.z = player.position.z;

        // Apply the updated position
        transform.position = newPosition;
    }
}

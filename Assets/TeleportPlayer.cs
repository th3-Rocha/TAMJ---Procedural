using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    public Transform teleportPosition; // Position to teleport the player
    public bool playerClicked = false; // Flag to check if the object was clicked

    private GameObject player; // Reference to the player GameObject

    void Start()
    {
        // Find the player GameObject by tag
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("Player not found in the scene.");
        }
    }

    void OnMouseDown()
    {
        // Check if teleportPosition and player are assigned
        if (teleportPosition != null && player != null)
        {
            CharacterController characterController = player.GetComponent<CharacterController>();
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (characterController != null) characterController.enabled = false;
            if (rb != null) rb.isKinematic = true;
            player.transform.position = teleportPosition.position;
            if (characterController != null) characterController.enabled = true;
            if (rb != null) rb.isKinematic = false;

            playerClicked = true;
        }

    }
}

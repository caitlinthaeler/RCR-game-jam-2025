using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public float sinkSpeed = 1f;  // Speed at which the plate sinks
    public float minHeight = 7.5f;  // The lowest point the plate can sink to
    public float maxDistance = 1f;  // Maximum distance the player can be from the center of the plate

    public Transform player;
    private float lastYPosition;
    private bool isSinking = false;
    public AudioSource audioSource;
    public AudioClip jumpSound;

    private void Start()
    {
        if (player != null)
        {
            lastYPosition = player.position.y;
        }
    }

    private void Update()
    {
        if (player == null) return;

        // Check if the player is directly above the plate within maxDistance
        bool playerAbove = Mathf.Abs(player.position.x - transform.position.x) < maxDistance &&
                           Mathf.Abs(player.position.z - transform.position.z) < maxDistance &&
                           player.position.y > transform.position.y;

        // Detect if the player is falling (current Y is lower than the last Y position)
        bool isFalling = player.position.y < lastYPosition - 0.1f;

        if (playerAbove && isFalling)
        {
            // Wait until the player has fallen past a certain threshold before sinking the plate
            if (player.position.y < lastYPosition - 0.5f) 
            {
                if (!isSinking)
                {
                    isSinking = true;
                    // Play the jump sound when the plate starts sinking
                    if (audioSource != null && jumpSound != null)
                    {
                        audioSource.PlayOneShot(jumpSound);
                    }
                }
                
            }
        }
        else
        {
            isSinking = false;
        }

        // Move the plate down at a steady speed
        if (isSinking && transform.position.y > minHeight)
        {
            transform.position -= new Vector3(0, sinkSpeed * Time.deltaTime, 0);
        }

        // Update last Y position for next frame
        lastYPosition = player.position.y;
    }
}

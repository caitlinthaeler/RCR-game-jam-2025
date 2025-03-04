using UnityEngine;
using System.Collections;

public class PressurePlate : MonoBehaviour
{
    public float sinkSpeed = 1f;  // Speed at which the plate sinks
    public float minHeight = 7.5f;  // The lowest point the plate can sink to
    public float maxDistance = 1f;  // Maximum distance the player can be from the center of the plate

    public Transform player;
    public Transform trapdoorHinge;
    public float trapdoorRotationStep = 5f;  // Degrees to rotate per jump
    public float maxTrapdoorRotation = 60f; // Maximum rotation before stopping

    private float lastYPosition;
    private bool isSinking = false;
    private float currentTrapdoorRotation = 0f;  // Track how much the trapdoor has rotated
    public AudioSource audioSource;
    public AudioClip jumpSound;
    private bool canJump = true;

    private void Start()
    {
        if (player != null)
        {
            lastYPosition = player.position.y;
        }
    }

    private void Update()
    {
        if (player == null || !canJump) return;

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
                        OpenTrapdoor();
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
        else if (transform.position.y <= minHeight)
        {
            canJump = false;
        }

        // Update last Y position for next frame
        lastYPosition = player.position.y;
    }

    private void OpenTrapdoor()
    {
        if (currentTrapdoorRotation < maxTrapdoorRotation)
        {
            float newRotation = Mathf.Min(currentTrapdoorRotation + trapdoorRotationStep, maxTrapdoorRotation);
            StartCoroutine(RotateTrapdoorSmoothly(newRotation - currentTrapdoorRotation));
            currentTrapdoorRotation = newRotation;
        }
    }


    private IEnumerator RotateTrapdoorSmoothly(float rotationAmount)
    {
        float rotationSpeed = 10f; // Speed of rotation (degrees per second)
        float rotatedSoFar = 0f;

        while (rotatedSoFar < rotationAmount)
        {
            float rotationStep = rotationSpeed * Time.deltaTime;
            rotationStep = Mathf.Min(rotationStep, rotationAmount - rotatedSoFar); // Prevent overshooting

            trapdoorHinge.Rotate(Vector3.left * rotationStep);
            rotatedSoFar += rotationStep;

            yield return null;
        }
    }

}

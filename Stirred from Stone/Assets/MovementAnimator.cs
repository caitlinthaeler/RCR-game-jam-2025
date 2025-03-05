using System.Collections;
using UnityEngine;

public class MovementAnimator : MonoBehaviour
{
    public Vector3 movementVector;  // The distance to move the object (in a vector form)
    public float moveTime = 4f;     // Time to complete the movement in seconds
    private Vector3 startPosition;  // Starting position of the object
    
    // Function to invoke the movement and start the process
    public void StartMovement(System.Action onComplete = null)
    {
        startPosition = transform.position;  // Set the current position as the starting point
        StartCoroutine(MoveObject(onComplete));  // Start the coroutine
    }

    public void ReverseMovement(System.Action onComplete = null)
    {
        startPosition = transform.position;  // Set the current position as the starting point
        StartCoroutine(ReverseObject(onComplete));  // Start the coroutine
    }

    // Coroutine for smooth movement
    private IEnumerator MoveObject(System.Action onComplete)
    {
        float moveStartTime = Time.time;  // Record the start time of the movement
        Vector3 targetPosition = startPosition + movementVector;  // Target position after moving the vector

        while (Time.time - moveStartTime < moveTime)
        {
            float elapsedTime = Time.time - moveStartTime;
            // Calculate the current position by interpolating between the start position and target
            Vector3 currentPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveTime);
            transform.position = currentPosition;
            yield return null;  // Wait until the next frame
        }

        // Make sure we end exactly at the target position
        transform.position = targetPosition;

        // Call the onComplete callback if provided (notifies when the movement is complete)
        onComplete?.Invoke();
    }

    private IEnumerator ReverseObject(System.Action onComplete)
    {
        float moveStartTime = Time.time;  // Record the start time of the movement
        Vector3 targetPosition = startPosition - movementVector;  // Target position after moving the vector

        while (Time.time - moveStartTime < moveTime)
        {
            float elapsedTime = Time.time - moveStartTime;
            // Calculate the current position by interpolating between the start position and target
            Vector3 currentPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveTime);
            transform.position = currentPosition;
            yield return null;  // Wait until the next frame
        }

        // Make sure we end exactly at the target position
        transform.position = targetPosition;

        // Call the onComplete callback if provided (notifies when the movement is complete)
        onComplete?.Invoke();
    }
}

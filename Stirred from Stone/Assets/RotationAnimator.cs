using UnityEngine;

public class RotationAnimator : MonoBehaviour
{
    public Transform pivotPoint;  // The point around which the object rotates
    public float rotationDegrees = 120f;  // The degrees to rotate (positive for clockwise, negative for counterclockwise)
    public float rotationTime = 4f;  // Time to complete the rotation in seconds
    public Vector3 rotationAxis = Vector3.up;  // Axis of rotation (e.g., Vector3.up for Y-axis, Vector3.right for X-axis, Vector3.forward for Z-axis)

    private float rotationAmount;  // Amount to rotate per frame
    private float rotationStartTime;  // Time when rotation starts
    private bool isRotating = false;  // Flag to control if the rotation is ongoing

    void Start()
    {
        // Ensure that the rotation starts at 0
        rotationAmount = 0f;
    }

    void Update()
    {
        if (isRotating)
        {
            // Calculate how much time has passed since the rotation started
            float elapsedTime = Time.time - rotationStartTime;

            // Calculate how much the rotation should be based on the time passed
            float currentRotation = Mathf.Lerp(0f, rotationDegrees, elapsedTime / rotationTime);

            // Rotate the object by the difference in the rotation
            float deltaRotation = currentRotation - rotationAmount;
            RotateAroundPivot(deltaRotation);

            // Update the last rotation amount
            rotationAmount = currentRotation;

            // If the rotation is complete, stop the rotation
            if (elapsedTime >= rotationTime)
            {
                isRotating = false;
            }
        }
    }

    // Function to invoke the rotation and start the process
    public void StartRotation()
    {
        rotationStartTime = Time.time;  // Record the start time of the rotation
        isRotating = true;  // Enable rotation
    }

    // Optional: Reset the rotation to the initial rotation if needed
    public void ResetRotation()
    {
        rotationAmount = 0f;
        transform.rotation = Quaternion.identity;  // Resets to the default rotation
    }

    // Function to rotate the object around the pivot point
    private void RotateAroundPivot(float angle)
    {
        transform.RotateAround(pivotPoint.position, rotationAxis, angle);
    }
}

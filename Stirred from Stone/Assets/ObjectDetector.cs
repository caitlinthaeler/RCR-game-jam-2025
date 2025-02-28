using UnityEngine;
using UnityEngine.UI;

public class ObjectDetector : MonoBehaviour
{
    public Image crosshair;
    public float detectionDistance = 10f;
    public LayerMask layersToDetect;

    private GameObject detectedObject;
    public GameObject DetectedObject
    {
        get { return detectedObject; }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, detectionDistance, layersToDetect))
        {
            detectedObject = hit.collider.gameObject;
            Debug.Log("Detected object: " + detectedObject.name);
            crosshair.color = Color.green;
        }
        else
        {
            detectedObject = null;
            crosshair.color = Color.white;
        }
    }
}

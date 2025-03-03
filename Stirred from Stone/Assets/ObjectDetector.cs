using UnityEngine;
using UnityEngine.UI;
using System;

public class ObjectDetector : MonoBehaviour
{
    public Image crosshair;
    public float detectionDistance = 10f;
    public LayerMask layersToDetect;

    private GameObject detectedObject;
    public GameObject DetectedObject => detectedObject; 

    public Color hoverColor; // ✅ Semi-transparent white overlay

    
    public CrosshairText crosshairText;
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
            Debug.Log($"Hit object: {hit.collider.gameObject.name}");
            GameObject hitObject = hit.collider.gameObject;
            
            if (detectedObject != hitObject)
            {
                Debug.Log("new object hit");
                // changing objects
                if (detectedObject != null)
                {
                    RemoveOverlay(detectedObject);
                }
                // detecting new object
                detectedObject = hitObject;
                ApplyOverlay(detectedObject);
                if (detectedObject.GetComponent<IInteractable>() != null)
                {
                    ProvideDetails(detectedObject.GetComponent<IInteractable>());
                }
                crosshair.color = Color.green;

            }
            
        }
        else
        {
            if (detectedObject != null)
            {
                 RemoveOverlay(detectedObject);
            }
            detectedObject = null;
            crosshair.color = Color.white;
        }
    }

     void ApplyOverlay(GameObject obj)
    {
        
        MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
        if (renderer && renderer.sharedMaterial.HasProperty("_EmissionColor"))
        {
            renderer.material.SetColor("_EmissionColor", hoverColor);
            // Ensure the material is using the emission effect by enabling it
            renderer.material.EnableKeyword("_EMISSION");
        }
    }

    void RemoveOverlay(GameObject obj)
    {
        MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            // Reset the emission color to black (or transparent effect)
            renderer.material.SetColor("_EmissionColor", Color.black);
        }
    }

    public void ProvideDetails(IInteractable interactable)
    {
        crosshairText.DisplayText(interactable);
    }
}

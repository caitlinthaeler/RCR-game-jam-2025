using UnityEngine;
using System.Collections.Generic;

public class InteractionHandler : MonoBehaviour
{
    public ObjectDetector objectDetector;
    public StoneManager stoneManager;
    //public Inventory inventory;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact(List<GameObject> items)
    {
        if (objectDetector.DetectedObject != null)
        {
            switch (objectDetector.DetectedObject.name)
            {
                case "Bell":
                    break;
                case "Candle":
                    break;
                case "Door":
                    break;
                case "Chalice":
                    break;
                case "Statue":
                    break;
                case "Note":
                    break;
                case "HolyWater":
                    break;
                case "Paper":
                    break;
                case "Stone":
                    break;
                case "Book":
                    break;
                case "Key":
                    break;
                case "Lighter":
                    break;
                case "BellStriker":
                    break;
                default:
                    break;
            }
        }
    }
}

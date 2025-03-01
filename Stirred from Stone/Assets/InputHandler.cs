using UnityEngine;
using System;

public class InputHandler : MonoBehaviour
{
    public InventoryHandler inventory;
    public ObjectDetector objectDetector;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Check if the player is looking at a stone
            // If the player is looking at a stone, call the ReturnStone method on the StoneManager
            // If the player is looking at a stone, call the SetText method on the CrosshairText
            if (objectDetector.DetectedObject != null)
            {
                IInteractable interactable = objectDetector.DetectedObject.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    Debug.Log($"Interacted with: {interactable.Name}");
                    interactable.Interact();
            //     }
            //     switch (objectDetector.DetectedObject.Name)
            //     {
            //         case "Bell":
            //             break;
            //         case "Candle":
            //             break;
            //         case "Door":
            //             break;
            //         case "Chalice":
            //             break;
            //         case "Statue":
            //             break;
            //         case "Note":
            //             break;
            //         case "HolyWater":
            //             break;
            //         case "Paper":
            //             break;
            //         case "Stone":
            //             break;
            //         case "Book":
            //             break;
            //         case "Key":
            //             break;
            //         case "Lighter":
            //             break;
            //         case "BellStriker":
            //             break;
            //         default:
            //             break;
            //    }
                 }
            }
            else {
                //try to use item
                //ex. read note
            }
        }

        //

        else // check if a number has been pressed for inventory
        {
            for (int i = 1; i <= 9; i++)
            {
                if (Input.GetKeyDown((KeyCode)Enum.Parse(typeof(KeyCode), "Alpha" + i)))
                {
                    Debug.Log($"Key {i} pressed!");
                    break;
                }
            }
        }
    }
}

using UnityEngine;
using System.Collections.Generic;

public class StoneRecptacleInteractable : MonoBehaviour, IInteractable
{
    public string Name => "StoneReceptacle";
    public Vector3[] positions;
    public StoneManager stoneManager;

    public void Interact()
    {
        Debug.Log($"stoneManager.AllStonesReturned() = {stoneManager.AllStonesReturned()}");
        if (!stoneManager.AllStonesReturned())
        {
            List<int> stonesToRemove = new List<int>();
            for (int i = 0; i < InventoryHandler.Instance.items.Count; i++)
            {

                ItemObject itemObject = InventoryHandler.Instance.items[i];
                if (itemObject != null && itemObject.itemName == "Stone")
                {
                    stonesToRemove.Add(i);
                    stoneManager.ReturnStone();
                    
                    Debug.Log($"Stone returned to receptacle. {stoneManager.GetStonesReturned()} stones returned.");

                    Vector3 spawnPosition = gameObject.transform.position + positions[stoneManager.GetStonesReturned()-1];
                    GameObject stone = Instantiate(itemObject.itemPrefab, spawnPosition, Quaternion.identity);
                    stone.GetComponent<BoxCollider>().enabled = false;
                    stone.layer = LayerMask.NameToLayer("interactableNonCollision");
                }
            }
            for (int i = stonesToRemove.Count - 1; i >= 0; i--)
            {
                InventoryHandler.Instance.RemoveItem(stonesToRemove[i]);
            }
        } else {
            Debug.Log("All stones have been returned!");
        }
    }
}

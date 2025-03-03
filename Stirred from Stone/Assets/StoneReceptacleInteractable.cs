using UnityEngine;
using System.Collections.Generic;

public class StoneRecptacleInteractable : MonoBehaviour, IInteractable
{
    public string Name => "Pictish Altar";
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
                    Vector3 spawnPosition = gameObject.transform.position + positions[stoneManager.GetStonesReturned()-1];
                    GameObject stone = Instantiate(itemObject.itemPrefab, spawnPosition, itemObject.itemPrefab.transform.rotation);
                    stone.GetComponent<BoxCollider>().enabled = false;
                    stone.layer = LayerMask.NameToLayer("interactableNonCollision");
                }
            }
            for (int i = stonesToRemove.Count - 1; i >= 0; i--)
            {
                InventoryHandler.Instance.RemoveItem(stonesToRemove[i]);
            }
            if (stoneManager.AllStonesReturned())
            {
                 UIManager.Instance.DisplayInfoCard("Pictish Altar", $"You have returned the last {stonesToRemove.Count}(s) to the altar. You won the game!", null);

            }else {
                UIManager.Instance.DisplayInfoCard("Pictish Altar", $"You have returned {stonesToRemove.Count} (s)tones to the altar. There are still {stoneManager.totalStonesRequired - stoneManager.GetStonesReturned()} misplaced stones.", null);
            }
        } else {
            UIManager.Instance.DisplayInfoCard("Pictish Altar", $"All stones have been returned, Thank you!", null);
            Debug.Log("All stones have been returned, Thank you!");
        }
    }
}

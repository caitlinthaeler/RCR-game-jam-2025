using UnityEngine;

public class AltarCandleInteractable : MonoBehaviour, IInteractable
{
    public GameObject flame;
    public bool lit = false;
    public ItemObject itemObject;
    public string Name => itemObject.itemName;

    private void Start()
    {
        flame = transform.GetChild(0).gameObject;
        flame.SetActive(false);

    }

    public void Interact()
    {
        if (!lit)
        {
            for (int i = 0; i < InventoryHandler.Instance.items.Count; i++)
            {
                ItemObject itemObject = InventoryHandler.Instance.items[i];
                if (itemObject != null && itemObject.itemName == "Candle Holder")
                {
                    LightCandle();
                    AltarCandlesManager.Instance.CheckCandlesLit();
                    return;
                }
            }
        }
    }

    public void LightCandle()
    {
        lit = true;
        flame.SetActive(true);
    }

    public void UnlightCandle()
    {
        lit = false;
        flame.SetActive(false);
    }
}

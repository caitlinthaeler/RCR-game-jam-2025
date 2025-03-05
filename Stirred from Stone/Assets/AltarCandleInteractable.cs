using UnityEngine;

public class AltarCandleInteractable : MonoBehaviour, IInteractable
{
    private GameObject flame;
    private bool lit = false;
    public ItemObject itemObject;
    public string Name => itemObject.itemName;
    public AudioSource audioSource;
    public AudioClip lightingCandleSound;
    public AudioClip blowingCandleOutSound;

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
                    AltarCandlesManager.Instance.CheckCandlesLit(1);
                    return;
                }
            }
        }
    }

    public void LightCandle()
    {
        lit = true;
        flame.SetActive(true);
        
        if (audioSource && lightingCandleSound)
        {
            audioSource.PlayOneShot(lightingCandleSound);
        }
    }

    public void UnlightCandle()
    {
        lit = false;
        flame.SetActive(false);
        if (audioSource && blowingCandleOutSound)
        {
            audioSource.PlayOneShot(blowingCandleOutSound);
        }
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "ItemObject", menuName = "Scriptable Objects/ItemObject")]
public class ItemObject : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public Sprite itemIcon;
    public GameObject itemPrefab;
}

[CreateAssetMenu(fileName = "BellObject", menuName = "Scriptable Objects/BellObject")]
public class BellObject : ScriptableObject
{    
    public string itemName;
    public int BellNumber;
    public AudioClip audioFile;
}

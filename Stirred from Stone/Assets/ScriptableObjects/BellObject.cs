using UnityEngine;

[CreateAssetMenu(fileName = "BellObject", menuName = "Scriptable Objects/BellObject")]
public class BellObject : ScriptableObject
{    
    public string itemName;
    public int BellNumber;
    public AudioClip audioFile;
}

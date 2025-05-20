using UnityEngine;

[CreateAssetMenu(menuName = "Game/Item", fileName = "NewItem")]
public class ItemSO : ScriptableObject
{
    [Header("Basic Info")]
    public string itemName;
    public Sprite icon;
    [TextArea] public string description;

    [Header("Score Value")]
    public int point = 10;
}

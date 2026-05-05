using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public bool stackable;
    public int maxStack = 1;

    [Header("Inspect View")]
    public Sprite inspectImage; // THIS is your note screenshot / full view
}
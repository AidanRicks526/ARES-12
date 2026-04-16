using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemData> items;
    public ItemData GetById(string id)
    {
        return items.Find(i => i.id == id);
    }
}



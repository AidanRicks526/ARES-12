using UnityEngine;

[CreateAssetMenu(fileName = "NewLabNote", menuName = "Inventory/Lab Note")]
public class LabNote : ItemData
{
    [TextArea(5, 15)]
    public string noteContent;
}
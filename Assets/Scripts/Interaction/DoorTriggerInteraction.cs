using UnityEngine;

public class DoorTriggerInteraction : TriggerInteractionBase
{
    public enum DoorToSpawnAt
    {
        Observatory_toHallway,
        Hallway_toObservatory,
        Hallway_toMainCore,
        MainCore_toHallway,
        MainCore_toCoreRing,
        CoreRing_toMainCore,
        CoreRing_toGateA,
        GateA_toCoreRing,
        CoreRing_toGateB,
        GateB_toCoreRing,
        CoreRing_toEngineRoom,
        EngineRoom_toCoreRing,
        GateA_toPod1,
        Pod1_toGateA,
        GateB_toPod2,
        Pod2_toGateB,
        none,
    }

    [Header("Spawn TO")]
    [SerializeField] private DoorToSpawnAt DoorToSpawnTo;
    [SerializeField] private SceneField _sceneToLoad;

    [Space(10f)]
    [Header("THIS Door")]
    public DoorToSpawnAt CurrentDoorPosition;

    [Space(10f)]
    [Header("Lock Settings")]
    [SerializeField] private bool isLocked = true;
    [SerializeField] private ItemData requiredBadge;

    public override void interact()
    {
        if (isLocked)
        {
            Debug.Log($"CardSwipeManager.Instance = {CardSwipeManager.Instance}");   // Add this
            Debug.Log($"requiredBadge = {requiredBadge}");

            CardSwipeManager.Instance.ShowSwipePanel(this, requiredBadge);
        }
        else
        {
            SceneSwapManager.SwapSceneFromDoorUse(_sceneToLoad, DoorToSpawnTo);
        }
    }

    public void UnlockDoor()
    {
        isLocked = false;
        Debug.Log("Door unlocked! Press E again to enter.");
    }
}
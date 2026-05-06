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

    public enum LockType
    {
        None,
        Keycard,
        Item
    }

    [Header("Spawn TO")]
    [SerializeField] private DoorToSpawnAt DoorToSpawnTo;
    [SerializeField] private SceneField _sceneToLoad;

    [Header("THIS Door")]
    public DoorToSpawnAt CurrentDoorPosition;

    [Header("Lock Settings")]
    [SerializeField] private bool isLocked = true;
    [SerializeField] private LockType lockType = LockType.Keycard;

    [SerializeField] private ItemData requiredBadge;
    [SerializeField] private ItemData requiredItem;

    [Header("Spawn Point")]
    [SerializeField] private Transform spawnPoint;

    public Vector3 GetSpawnPosition()
    {
        return spawnPoint != null ? spawnPoint.position : transform.position;
    }

    public override void interact()
    {
        if (isLocked)
        {
            switch (lockType)
            {
                case LockType.Keycard:
                    CardSwipeManager.Instance.ShowSwipePanel(this, requiredBadge);
                    return;

                case LockType.Item:
                    TryUnlockWithItem();
                    return;

                case LockType.None:
                    break;
            }

            return;
        }

        EnterDoor();
    }

    private void TryUnlockWithItem()
    {
        if (Inventory.Instance == null)
            return;

        if (Inventory.Instance.HasItem(requiredItem))
        {
            Inventory.Instance.RemoveItem(requiredItem);

            UnlockDoor(); // now shows popup too
        }
        else
        {
            DoorLockedUI.Instance?.Show("Door Locked");
        }
    }

    public void UnlockDoor()
    {
        isLocked = false;

        Debug.Log("Door unlocked!");

        // ✨ feedback popup
        DoorLockedUI.Instance?.Show("Door Unlocked");
    }

    private void EnterDoor()
    {
        Door_Looper looper = FindFirstObjectByType<Door_Looper>();
        if (looper != null)
        {
            looper.SaveDoorPositions();
        }

        SceneSwapManager.SwapSceneFromDoorUse(_sceneToLoad, DoorToSpawnTo);
    }
}
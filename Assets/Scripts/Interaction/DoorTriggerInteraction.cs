using System.Collections;
using System.Collections.Generic;
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

    public override void interact()
    {
        SceneSwapManager.SwapSceneFromDoorUse(_sceneToLoad, DoorToSpawnTo);
    }

}

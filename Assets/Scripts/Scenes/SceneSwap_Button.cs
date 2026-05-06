using UnityEngine;

public class SceneSwap_Button : MonoBehaviour
{
    public SceneField sceneToLoad;
    public DoorTriggerInteraction.DoorToSpawnAt door;

    public void LoadScene()
    {
        SceneSwapManager.SwapSceneFromDoorUse(sceneToLoad, door);
    }
}
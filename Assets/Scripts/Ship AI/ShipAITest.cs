using UnityEngine;

public class ShipAITest : MonoBehaviour
{
    public ShipAI shipAI;

    [Header("Test Lines")]
    public VoiceLine line1;
    public VoiceLine line2;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            shipAI.PlayVoiceLine(line1);
            shipAI.PlayVoiceLine(line2);
        }
    }
}
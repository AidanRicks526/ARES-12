using UnityEngine;

public class Fail_Zone : MonoBehaviour
{
    public Minigame_EffectRunner effectObject;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Square"))
        {
            effectObject.PlayEffect(false);
        }
    }
}
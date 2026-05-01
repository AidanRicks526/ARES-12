using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Changable_Layer : MonoBehaviour
{
    private SpriteRenderer sr;
    private SpriteRenderer playerSR;
    private Transform player;

    [Header("Detection Settings")]
    [Tooltip("The 'Ground' Y of this object relative to its pivot")]
    [SerializeField] private float yOffset = 0f;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        // Using the updated Unity method we discussed earlier
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerSR = playerObj.GetComponent<SpriteRenderer>();
        }
    }

    void LateUpdate()
    {
        if (player == null || playerSR == null) return;

        // Compare the bottom of the box to the bottom of the player
        float myBottom = transform.position.y + yOffset;
        float playerBottom = player.position.y;

        if (myBottom < playerBottom)
        {
            // Box is lower on screen -> Put Box in FRONT
            sr.sortingLayerID = playerSR.sortingLayerID;
            sr.sortingOrder = playerSR.sortingOrder + 1;
        }
        else
        {
            // Box is higher on screen -> Put Box BEHIND
            sr.sortingLayerID = playerSR.sortingLayerID;
            sr.sortingOrder = playerSR.sortingOrder - 1;
        }
    }
}
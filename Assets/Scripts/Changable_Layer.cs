using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Changable_Layer : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Sorting Settings")]
    [Tooltip("Offset to adjust where the 'feet' of the object are")]
    [SerializeField] private float yOffset = 0f;

    [Tooltip("Higher value = more precise sorting")]
    [SerializeField] private int precision = 100;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        // 🔥 Universal sorting based on world Y
        float y = transform.position.y + yOffset;

        sr.sortingOrder = Mathf.RoundToInt(-y * precision);
    }
}
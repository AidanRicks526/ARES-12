using UnityEngine;

public class CircularHallway : MonoBehaviour
{
    [Header("Segments in order (left to right)")]
    public Transform[] segments;
    [Tooltip("Optional manual step if auto-calculation fails")]
    public float manualStep = 0f;

    private HallwayPersistence persistence;
    private bool isRecycling = false;
    private float actualStep;

    void Awake()
    {
        DetermineActualStep();
        persistence = HallwayPersistence.Instance;
        if (persistence != null && persistence.hasSavedState)
            RestoreOrientation();
    }

    private void DetermineActualStep()
    {
        if (manualStep > 0.0001f)
        {
            actualStep = manualStep;
            return;
        }

        if (segments.Length >= 2 && segments[0] != null && segments[1] != null)
        {
            float step = segments[1].position.x - segments[0].position.x;
            if (Mathf.Abs(step) > 0.0001f)
            {
                actualStep = step;
                Debug.Log($"Calibrated step = {actualStep}");
                return;
            }
        }
        actualStep = 17.54f; // fallback
    }

    private float GetStepDistance() => actualStep;

    public void MoveLeftmostToRight()
    {
        if (isRecycling || segments == null || segments.Length == 0) return;
        isRecycling = true;

        Transform leftmost = segments[0];
        Transform rightmost = segments[segments.Length - 1];

        Vector3 newPos = rightmost.position;
        newPos.x += GetStepDistance();
        leftmost.position = newPos;

        Transform first = segments[0];
        for (int i = 0; i < segments.Length - 1; i++)
            segments[i] = segments[i + 1];
        segments[segments.Length - 1] = first;

        isRecycling = false;
    }

    public void MoveRightmostToLeft()
    {
        if (isRecycling || segments == null || segments.Length == 0) return;
        isRecycling = true;

        Transform leftmost = segments[0];
        Transform rightmost = segments[segments.Length - 1];

        Vector3 newPos = leftmost.position;
        newPos.x -= GetStepDistance();
        rightmost.position = newPos;

        Transform last = segments[segments.Length - 1];
        for (int i = segments.Length - 1; i > 0; i--)
            segments[i] = segments[i - 1];
        segments[0] = last;

        isRecycling = false;
    }

    public void SaveOrientation()
    {
        if (persistence == null || segments == null || segments.Length == 0) return;
        SegmentData firstData = segments[0].GetComponent<SegmentData>();
        if (firstData != null)
        {
            persistence.savedOffset = firstData.prefabIndex;
            persistence.hasSavedState = true;
        }
    }

    public void RestoreOrientation()
    {
        if (persistence == null || !persistence.hasSavedState || segments == null) return;
        int targetOffset = persistence.savedOffset;

        int firstIndex = -1;
        for (int i = 0; i < segments.Length; i++)
        {
            if (segments[i] != null && segments[i].GetComponent<SegmentData>().prefabIndex == targetOffset)
            {
                firstIndex = i;
                break;
            }
        }
        if (firstIndex == -1) return;

        // Reorder array
        Transform[] newOrder = new Transform[segments.Length];
        for (int i = 0; i < segments.Length; i++)
            newOrder[i] = segments[(firstIndex + i) % segments.Length];
        segments = newOrder;

        // Reposition flush
        float startX = segments[0].position.x;
        float step = GetStepDistance();
        for (int i = 0; i < segments.Length; i++)
        {
            Vector3 pos = segments[i].position;
            pos.x = startX + i * step;
            segments[i].position = pos;
        }
    }

    private void OnDestroy()
    {
        SaveOrientation();
    }
}
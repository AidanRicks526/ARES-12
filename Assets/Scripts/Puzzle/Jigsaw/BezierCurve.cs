using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    // Calculate a point on a cubic Bezier curve
    public static Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1f - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 point = uuu * p0; // (1-t)^3 * p0
        point += 3f * uu * t * p1; // 3(1-t)^2 * t * p1
        point += 3f * u * tt * p2; // 3(1-t) * t^2 * p2
        point += ttt * p3;        // t^3 * p3

        return point;
    }
}

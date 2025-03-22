using System.Collections;
using UnityEngine;

public static class TransformUtil
{
    public static IEnumerator MoveToPos(Transform transform, Vector3 startPos, Vector3 endPos, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
    }

    public static IEnumerator RotateToQuat(Transform transform, Quaternion startQuat, Quaternion endQuat, float duration)
    {
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.rotation = Quaternion.Slerp(startQuat, endQuat, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endQuat;
    }
}

using System.Collections;
using UnityEngine;

public static class MaterialUtil
{
    public static IEnumerator FadeOut(GameObject gameObject, float duration)
    {
        MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
        Material[] materials = renderer.materials;
        Color[] initialColors = new Color[materials.Length];
        for (int i = 0; i < materials.Length; i++)
        {
            initialColors[i] = materials[i].color;
        }

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            for (int i = 0; i < materials.Length; i++)
            {
                float alpha = Mathf.Lerp(initialColors[i].a, 0, t);
                materials[i].color = new Color(initialColors[i].r, initialColors[i].g, initialColors[i].b, alpha);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].color = new Color(initialColors[i].r, initialColors[i].g, initialColors[i].b, 0);
        }
    }

    public static void SetToFullOpacity(GameObject gameObject)
    {
        Material[] materials = gameObject.GetComponent<MeshRenderer>().materials;
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].color = new Color(materials[i].color.r, materials[i].color.g, materials[i].color.b, 1);
        }
    }
}

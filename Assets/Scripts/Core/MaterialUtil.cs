using System.Collections;
using UnityEngine;

public static class MaterialUtil
{
    public static IEnumerator FadeOutAndDestroy(GameObject gameObject, float duration)
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
        
        GameObject.Destroy(gameObject);
    }
}

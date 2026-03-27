using UnityEngine;
using System.Collections;

public class hitFlash : MonoBehaviour
{
    [Header("Flash Settings")]
    public Color flashColor = Color.red;    // Color to flash on hit
    public float flashDuration = 0.5f;     // How long the flash lasts

    private Renderer[] renderers;
    private Color[] originalColors;

    void Start()
    {
        // Grab all renderers on this object and its children (handles multi-mesh enemies)
        renderers = GetComponentsInChildren<Renderer>();
        originalColors = new Color[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
            originalColors[i] = renderers[i].material.color;
    }

    // Hook this up to the Health onHit UnityEvent in the Inspector
    public void Flash()
    {
        StartCoroutine(DoFlash());
    }

    IEnumerator DoFlash()
    {
        // Set all renderers to flash color
        foreach (Renderer r in renderers)
            r.material.color = flashColor;

        yield return new WaitForSeconds(flashDuration);

        // Restore original colors
        for (int i = 0; i < renderers.Length; i++)
            renderers[i].material.color = originalColors[i];
    }
}

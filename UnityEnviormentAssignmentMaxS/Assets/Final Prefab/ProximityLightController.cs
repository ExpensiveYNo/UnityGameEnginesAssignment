using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]
public class ProximityLightController : MonoBehaviour
{
    [Header("Proximity")]
    public float distanceLimit = 5f;
    public string playerTag = "Player";

    [Header("Glow Object")]
    public Renderer glowObject;              // object (or child) with MeshRenderer
    public Color glowColor = Color.red;     // base emission color
    public float emissionMultiplier = 2f;    // how bright emission is at full on

    [Header("Fade Settings")]
    public float fadeDuration = 0.6f;        // seconds to fade in/out
    public AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Light Settings")]
    public float lightOnIntensity = 1.5f;    // intensity when fully on

    Light myLight;
    Transform playerTransform;

    Material glowMaterialInstance;
    float currentFade = 0f;    // 0 = off, 1 = fully on
    float targetFade = 0f;

    void Start()
    {
        myLight = GetComponent<Light>();
        myLight.intensity = 0f;
        myLight.enabled = true; // keep enabled so intensity controls brightness

        GameObject playerObj = GameObject.FindWithTag(playerTag);
        if (playerObj != null) playerTransform = playerObj.transform;
        else Debug.LogError("Player object with tag '" + playerTag + "' not found!");

        if (glowObject != null)
        {
            // create a material instance so we don't modify the shared material
            glowMaterialInstance = glowObject.material;
            // ensure emission keyword is off initially
            glowMaterialInstance.DisableKeyword("_EMISSION");
            SetEmissionColor(0f);
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        float dist = Vector3.Distance(transform.position, playerTransform.position);
        bool isClose = dist < distanceLimit;
        targetFade = isClose ? 1f : 0f;

        // Smoothly move currentFade towards targetFade
        if (!Mathf.Approximately(currentFade, targetFade))
        {
            // move using time and duration
            float delta = (fadeDuration <= 0f) ? 1f : (Time.deltaTime / Mathf.Max(0.0001f, fadeDuration));
            currentFade = Mathf.MoveTowards(currentFade, targetFade, delta);

            // apply the animation curve for non-linear easing
            float curveValue = fadeCurve.Evaluate(currentFade);

            // update light intensity
            if (myLight != null)
            {
                myLight.intensity = Mathf.Lerp(0f, lightOnIntensity, curveValue);
                // keep light enabled while intensity > tiny epsilon, otherwise disable it to save cost
                myLight.enabled = myLight.intensity > 0.001f;
            }

            // update material emission
            if (glowMaterialInstance != null)
            {
                SetEmissionColor(curveValue);
            }
        }
    }

    void SetEmissionColor(float t)
    {
        // t in [0,1] — multiply color by intensity multiplier to get HDR-like emission
        if (glowMaterialInstance == null) return;

        if (t > 0.0001f)
        {
            glowMaterialInstance.EnableKeyword("_EMISSION");
            Color emission = glowColor * (emissionMultiplier * t);
            glowMaterialInstance.SetColor("_EmissionColor", emission);
        }
        else
        {
            glowMaterialInstance.SetColor("_EmissionColor", Color.black);
            glowMaterialInstance.DisableKeyword("_EMISSION");
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanceLimit);
    }
}

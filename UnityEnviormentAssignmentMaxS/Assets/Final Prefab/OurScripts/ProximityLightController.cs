using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]
public class ProximityLightController : MonoBehaviour
{
    [Header("Proximity")]
    public float distanceLimit = 5f;
    public string playerTag = "Player";

    [Header("Glow Object")]
    public Renderer glowObject;
    public Color glowColor = Color.red;
    public float emissionMultiplier = 2f;

    [Header("Fade Settings")]
    public float fadeDuration = 0.6f;
    public AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Light Settings")]
    public float lightOnIntensity = 1.5f;

    [Header("Healing Settings")]
    public bool healPlayer = true;          
    public int healAmount = 5;              
    public float healInterval = 1f;         

    Light myLight;
    Transform playerTransform;
    Health playerHealth;                    // Cached reference — only ever the player's Health
    Material glowMaterialInstance;
    float currentFade = 0f;
    float targetFade = 0f;
    float healTimer = 0f;
    bool playerIsClose = false;

    void Start()
    {
        myLight = GetComponent<Light>();
        myLight.intensity = 0f;
        myLight.enabled = true;

        // Find player and cache their Health component specifically
        GameObject playerObj = GameObject.FindWithTag(playerTag);
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
            playerHealth = playerObj.GetComponent<Health>();

            if (healPlayer && playerHealth == null)
                Debug.LogWarning($"{gameObject.name}: Player found but has no Health component!");
        }
        else
        {
            Debug.LogError("Player object with tag '" + playerTag + "' not found!");
        }

        if (glowObject != null)
        {
            glowMaterialInstance = glowObject.material;
            glowMaterialInstance.DisableKeyword("_EMISSION");
            SetEmissionColor(0f);
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        float dist = Vector3.Distance(transform.position, playerTransform.position);
        playerIsClose = dist < distanceLimit;
        targetFade = playerIsClose ? 1f : 0f;

        // Handle glow fade
        if (!Mathf.Approximately(currentFade, targetFade))
        {
            float delta = (fadeDuration <= 0f) ? 1f : (Time.deltaTime / Mathf.Max(0.0001f, fadeDuration));
            currentFade = Mathf.MoveTowards(currentFade, targetFade, delta);

            float curveValue = fadeCurve.Evaluate(currentFade);

            if (myLight != null)
            {
                myLight.intensity = Mathf.Lerp(0f, lightOnIntensity, curveValue);
                myLight.enabled = myLight.intensity > 0.001f;
            }

            if (glowMaterialInstance != null)
                SetEmissionColor(curveValue);
        }

        // Handle healing tick — only runs when player is close and healing is enabled
        if (healPlayer && playerIsClose && playerHealth != null)
        {
            healTimer += Time.deltaTime;
            if (healTimer >= healInterval)
            {
                healTimer = 0f;
                playerHealth.Heal(healAmount);
            }
        }
        else
        {
            healTimer = 0f; // Reset timer when player leaves so next entry starts a fresh tick
        }
    }

    void SetEmissionColor(float t)
    {
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
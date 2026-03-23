using UnityEngine;
using System.Collections;

public class RaycastShooter : MonoBehaviour
{
    [Header("Shooting Settings")]
    public float fireRate = 0.2f;         // Time between shots (seconds)
    public float range = 100f;            // Max raycast distance
    public int damage = 25;               // Damage dealt per hit
    public LayerMask hitLayers;           // Which layers can be hit

    [Header("Effects")]
    public ParticleSystem muzzleFlash;    // Optional muzzle flash effect
    public GameObject impactEffectPrefab; // Optional hit effect prefab

    [Header("Tracer Settings")]
    public Transform muzzlePoint;         // Where the tracer starts (e.g. gun barrel tip)
    public float tracerDuration = 0.05f;  // How long the tracer line stays visible
    public float tracerWidth = 0.02f;     // Thickness of the tracer line
    public Color tracerColor = new Color(1f, 0.9f, 0.3f); //Color of the tracer

    [Header("References")]
    public Camera playerCamera;           // Camera to shoot from

    private float nextFireTime = 0f;
    private LineRenderer lineRenderer;

    void Start()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;

        SetupLineRenderer();
    }

    void SetupLineRenderer()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = tracerWidth;
        lineRenderer.endWidth = tracerWidth * 0.5f; // Slightly tapered at the end
        lineRenderer.useWorldSpace = true;
        lineRenderer.enabled = false;

        // Create a simple unlit material so the tracer glows without needing lighting
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = tracerColor;
        lineRenderer.endColor = new Color(tracerColor.r, tracerColor.g, tracerColor.b, 0f); // Fade to transparent
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        if (muzzleFlash != null)
            muzzleFlash.Play();

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        // Start point: muzzle point if assigned, otherwise camera position
        Vector3 startPoint = muzzlePoint != null ? muzzlePoint.position : playerCamera.transform.position;

        // End point: where the ray hits, or max range if it hits nothing
        Vector3 endPoint;
        if (Physics.Raycast(ray, out RaycastHit hit, range, hitLayers))
        {
            endPoint = hit.point;

            Debug.Log($"Hit: {hit.collider.name} at {hit.point}");

            Health health = hit.collider.GetComponent<Health>();
            if (health != null)
                health.TakeDamage(damage);

            if (impactEffectPrefab != null)
            {
                GameObject impact = Instantiate(impactEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impact, 2f);
            }
        }
        else
        {
            // No hit — tracer travels to max range
            endPoint = ray.origin + ray.direction * range;
        }

        StartCoroutine(ShowTracer(startPoint, endPoint));
    }

    IEnumerator ShowTracer(Vector3 start, Vector3 end)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);

        // Fade out over the tracer duration
        float elapsed = 0f;
        while (elapsed < tracerDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / tracerDuration);
            lineRenderer.startColor = new Color(tracerColor.r, tracerColor.g, tracerColor.b, alpha);
            lineRenderer.endColor = new Color(tracerColor.r, tracerColor.g, tracerColor.b, 0f);
            yield return null;
        }

        lineRenderer.enabled = false;
    }
}

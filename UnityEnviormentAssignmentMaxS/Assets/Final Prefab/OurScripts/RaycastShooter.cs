using UnityEngine;
using System.Collections;

public class RaycastShooter : MonoBehaviour
{
    public static RaycastShooter instance;

    [Header("Shooting Settings")]
    public float fireRate = 0.2f;
    public float range = 100f;
    public int damage = 25;
    public int maxAmmo = 30;
    public LayerMask hitLayers;

    [Header("Effects")]
    public ParticleSystem muzzleFlash;
    public GameObject impactEffectPrefab;

    [Header("Tracer Settings")]
    public Transform muzzlePoint;
    public float tracerDuration = 0.05f;
    public float tracerWidth = 0.02f;
    public Color tracerColor = new Color(1f, 0.9f, 0.3f);

    [Header("References")]
    public Camera playerCamera;

    public int currentAmmo;
    private float nextFireTime = 0f;
    private LineRenderer lineRenderer;

    void Awake()
    {
        instance = this;
        currentAmmo = maxAmmo;
    }

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
        lineRenderer.endWidth = tracerWidth * 0.5f;
        lineRenderer.useWorldSpace = true;
        lineRenderer.enabled = false;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = tracerColor;
        lineRenderer.endColor = new Color(tracerColor.r, tracerColor.g, tracerColor.b, 0f);
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime && currentAmmo > 0)
        {
            nextFireTime = Time.time + fireRate;
            currentAmmo--;
            Shoot();
        }
    }

    // Called by ammo collectibles 
    public void RefillAmmo(int amount = -1)
    {
        currentAmmo = (amount < 0) ? maxAmmo : Mathf.Min(currentAmmo + amount, maxAmmo);
        Debug.Log($"Ammo refilled! Current ammo: {currentAmmo}/{maxAmmo}");
    }

    void Shoot()
    {
        if (muzzleFlash != null)
            muzzleFlash.Play();

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Vector3 startPoint = muzzlePoint != null ? muzzlePoint.position : playerCamera.transform.position;

        Vector3 endPoint;
        if (Physics.Raycast(ray, out RaycastHit hit, range, hitLayers))
        {
            endPoint = hit.point;
            //Debug.Log($"Hit: {hit.collider.name} at {hit.point}");

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
            endPoint = ray.origin + ray.direction * range;
        }

        StartCoroutine(ShowTracer(startPoint, endPoint));
    }

    IEnumerator ShowTracer(Vector3 start, Vector3 end)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);

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
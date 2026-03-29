using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [Header("Settings")]
    public float lifetime = 3f;             // How long before the ragdoll disappears
    public float fadeOutTime = 1f;          // How long it takes to fade out before destroying

    [Header("Death Nudge")]
    public float knockbackForce = 3f;       // How hard it gets thrown
    public float rotationForce = 2f;        // How much it tumbles

    private Renderer[] renderers;
    private float timer = 0f;
    private bool fading = false;

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Random horizontal direction so it doesn't always fall the same way
            Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0.5f, Random.Range(-1f, 1f)).normalized;
            rb.AddForce(randomDirection * knockbackForce, ForceMode.Impulse);

            // Random spin
            rb.AddTorque(Random.insideUnitSphere * rotationForce, ForceMode.Impulse);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (!fading && timer >= lifetime - fadeOutTime)
            fading = true;

        if (fading)
        {
            float alpha = Mathf.Lerp(1f, 0f, (timer - (lifetime - fadeOutTime)) / fadeOutTime);

            foreach (Renderer r in renderers)
            {
                foreach (Material m in r.materials)
                {
                    Color c = m.color;
                    c.a = alpha;
                    m.color = c;
                }
            }
        }

        if (timer >= lifetime)
            Destroy(gameObject);
    }
}

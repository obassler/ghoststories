using UnityEngine;
using UnityEngine.Events;

public class PriestBanish : MonoBehaviour
{
    public float banishTime = 3f;
    public float hitGracePeriod = 0.15f;

    [Header("Events")]
    public UnityEvent<float> onBanishProgressChanged;
    public UnityEvent onBanishStarted;
    public UnityEvent onBanishCancelled;
    public UnityEvent onBanishComplete;

    private float currentTime;
    private float lastHitTime;
    private bool isBanished;
    private bool wasBeingHit;

    private AudioSource audioSource;
    private PriestManager manager;
    private Renderer[] priestRenderers;
    private Collider priestCollider;

    public float BanishProgress => Mathf.Clamp01(currentTime / banishTime);
    public bool IsBeingBanished => !isBanished && (Time.time - lastHitTime) <= hitGracePeriod;
    public bool IsBanished => isBanished;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        manager = FindFirstObjectByType<PriestManager>();
        priestRenderers = GetComponentsInChildren<Renderer>();
        priestCollider = GetComponent<Collider>();

        lastHitTime = -hitGracePeriod - 1f;
    }

    void Update()
    {
        if (isBanished) return;

        bool isBeingHit = (Time.time - lastHitTime) <= hitGracePeriod;

        if (isBeingHit)
        {
            if (!wasBeingHit)
                onBanishStarted?.Invoke();

            currentTime += Time.deltaTime;
            onBanishProgressChanged?.Invoke(BanishProgress);

            if (currentTime >= banishTime)
                Banish();
        }
        else
        {
            if (wasBeingHit && currentTime > 0)
            {
                onBanishCancelled?.Invoke();
                onBanishProgressChanged?.Invoke(0f);
            }
            currentTime = 0f;
        }

        wasBeingHit = isBeingHit;
    }

    public void HitByTorch()
    {
        if (isBanished) return;
        lastHitTime = Time.time;
    }

    void Banish()
    {
        isBanished = true;

        onBanishComplete?.Invoke();
        onBanishProgressChanged?.Invoke(0f);

        if (audioSource != null)
            audioSource.Play();

        SetRenderersEnabled(false);

        if (priestCollider != null)
            priestCollider.enabled = false;

        currentTime = 0f;

        if (manager != null)
            manager.PriestBanished();
    }

    public void ResetForNewSpawn()
    {
        isBanished = false;
        currentTime = 0f;
        lastHitTime = -hitGracePeriod - 1f;
        wasBeingHit = false;

        SetRenderersEnabled(true);

        if (priestCollider != null)
            priestCollider.enabled = true;

        onBanishProgressChanged?.Invoke(0f);
    }

    private void SetRenderersEnabled(bool enabled)
    {
        if (priestRenderers == null) return;

        foreach (Renderer rend in priestRenderers)
        {
            if (rend != null)
                rend.enabled = enabled;
        }
    }
}

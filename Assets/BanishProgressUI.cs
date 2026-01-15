using UnityEngine;
using UnityEngine.UI;

public class BanishProgressUI : MonoBehaviour
{
    [Header("References")]
    public PriestBanish priestBanish;
    public Transform followTarget;

    [Header("UI Elements")]
    public Canvas worldCanvas;
    public Image progressBarFill;

    [Header("Settings")]
    public Vector3 offset = new Vector3(0, 2.5f, 0);
    public float fadeSpeed = 5f;
    public Color fillColor = new Color(1f, 0.8f, 0.2f);

    private Camera mainCamera;
    private CanvasGroup canvasGroup;
    private float targetAlpha;
    private bool isSetup;

    void Start()
    {
        mainCamera = Camera.main;

        if (priestBanish == null)
            priestBanish = GetComponentInParent<PriestBanish>();

        if (followTarget == null && priestBanish != null)
            followTarget = priestBanish.transform;

        if (worldCanvas == null)
            CreateProgressBarUI();
        else
            SetupExistingUI();

        if (priestBanish != null)
        {
            priestBanish.onBanishProgressChanged.AddListener(OnProgressChanged);
            priestBanish.onBanishStarted.AddListener(OnBanishStarted);
            priestBanish.onBanishCancelled.AddListener(OnBanishCancelled);
            priestBanish.onBanishComplete.AddListener(OnBanishComplete);
        }

        if (canvasGroup != null)
            canvasGroup.alpha = 0;
        targetAlpha = 0;
    }

    void CreateProgressBarUI()
    {
        GameObject canvasObj = new GameObject("BanishProgressCanvas");
        canvasObj.transform.SetParent(transform);
        canvasObj.transform.localPosition = offset;

        worldCanvas = canvasObj.AddComponent<Canvas>();
        worldCanvas.renderMode = RenderMode.WorldSpace;
        worldCanvas.sortingOrder = 100;

        canvasGroup = canvasObj.AddComponent<CanvasGroup>();

        RectTransform canvasRect = worldCanvas.GetComponent<RectTransform>();
        canvasRect.sizeDelta = new Vector2(1f, 0.15f);
        canvasRect.localScale = Vector3.one;

        GameObject fillObj = new GameObject("Fill");
        fillObj.transform.SetParent(canvasObj.transform);
        progressBarFill = fillObj.AddComponent<Image>();
        progressBarFill.color = fillColor;

        RectTransform fillRect = fillObj.GetComponent<RectTransform>();
        fillRect.anchorMin = Vector2.zero;
        fillRect.anchorMax = new Vector2(0, 1);
        fillRect.pivot = new Vector2(0, 0.5f);
        fillRect.offsetMin = new Vector2(0.02f, 0.02f);
        fillRect.offsetMax = new Vector2(0, -0.02f);
        fillRect.localScale = Vector3.one;
        fillRect.localPosition = Vector3.zero;

        isSetup = true;
    }

    void SetupExistingUI()
    {
        canvasGroup = worldCanvas.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = worldCanvas.gameObject.AddComponent<CanvasGroup>();

        if (progressBarFill != null)
            progressBarFill.color = fillColor;

        isSetup = true;
    }

    void Update()
    {
        if (!isSetup) return;

        if (canvasGroup != null)
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, fadeSpeed * Time.deltaTime);

        if (mainCamera != null && worldCanvas != null)
        {
            worldCanvas.transform.LookAt(
                worldCanvas.transform.position + mainCamera.transform.rotation * Vector3.forward,
                mainCamera.transform.rotation * Vector3.up
            );
        }

        if (followTarget != null && worldCanvas != null)
            worldCanvas.transform.position = followTarget.position + offset;
    }

    void OnProgressChanged(float progress)
    {
        if (progressBarFill == null) return;

        RectTransform fillRect = progressBarFill.GetComponent<RectTransform>();
        fillRect.anchorMax = new Vector2(progress, 1);
    }

    void OnBanishStarted()
    {
        targetAlpha = 1f;
    }

    void OnBanishCancelled()
    {
        targetAlpha = 0f;
    }

    void OnBanishComplete()
    {
        targetAlpha = 0f;
    }

    void OnDestroy()
    {
        if (priestBanish != null)
        {
            priestBanish.onBanishProgressChanged.RemoveListener(OnProgressChanged);
            priestBanish.onBanishStarted.RemoveListener(OnBanishStarted);
            priestBanish.onBanishCancelled.RemoveListener(OnBanishCancelled);
            priestBanish.onBanishComplete.RemoveListener(OnBanishComplete);
        }
    }
}

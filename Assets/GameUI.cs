using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using TMPro;
using System.Collections.Generic;

public class GameUI : MonoBehaviour
{
    [Header("Start Screen")]
    public string startTitle = "GHOST STORIES";
    public string startMessage = "Find and banish the priest 3 times to escape.";
    public string startPrompt = "Pull trigger to begin...";
    public float autoStartDelay = 0f;

    [Header("Victory Screen")]
    public string victoryTitle = "YOU SURVIVED";
    public string victoryMessage = "You have banished the evil spirits.";
    public string victoryPrompt = "Pull trigger to restart";

    [Header("HUD")]
    public string hudFormat = "Priests Banished: {0} / {1}";

    [Header("VR Settings")]
    public float uiDistance = 2f;
    public float uiScale = 0.002f;
    public bool followCamera = true;

    [Header("Colors")]
    public Color backgroundColor = new Color(0, 0, 0, 0.85f);
    public Color titleColor = Color.white;
    public Color textColor = new Color(0.8f, 0.8f, 0.8f);

    private Canvas canvas;
    private GameObject startScreen;
    private GameObject victoryScreen;
    private GameObject hudScreen;
    private PriestManager priestManager;
    private TextMeshProUGUI hudText;
    private Camera vrCamera;
    private bool gameStarted;
    private bool gameWon;
    private int lastBanishCount = -1;
    private float startTimer;
    private bool triggerWasPressed;

    void Start()
    {
        priestManager = FindFirstObjectByType<PriestManager>();
        vrCamera = Camera.main;

        CreateUI();

        if (startScreen != null)
            startScreen.SetActive(true);

        if (victoryScreen != null)
            victoryScreen.SetActive(false);

        if (hudScreen != null)
            hudScreen.SetActive(false);

        Time.timeScale = 0f;
        startTimer = 0f;
    }

    void CreateUI()
    {
        canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.sortingOrder = 999;

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        canvasRect.sizeDelta = new Vector2(800, 600);
        canvasRect.localScale = Vector3.one * uiScale;

        gameObject.AddComponent<GraphicRaycaster>();

        startScreen = CreateScreen("StartScreen", startTitle, startMessage, startPrompt);
        victoryScreen = CreateScreen("VictoryScreen", victoryTitle, victoryMessage, victoryPrompt);
        hudScreen = CreateHUD();

        PositionUI();
    }

    GameObject CreateScreen(string name, string title, string message, string prompt)
    {
        GameObject screen = new GameObject(name);
        screen.transform.SetParent(transform, false);

        RectTransform rect = screen.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Image bg = screen.AddComponent<Image>();
        bg.color = backgroundColor;

        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(screen.transform, false);
        TextMeshProUGUI titleText = titleObj.AddComponent<TextMeshProUGUI>();
        titleText.text = title;
        titleText.fontSize = 72;
        titleText.color = titleColor;
        titleText.alignment = TextAlignmentOptions.Center;
        titleText.fontStyle = FontStyles.Bold;
        RectTransform titleRect = titleObj.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0, 0.5f);
        titleRect.anchorMax = new Vector2(1, 0.8f);
        titleRect.offsetMin = Vector2.zero;
        titleRect.offsetMax = Vector2.zero;

        GameObject msgObj = new GameObject("Message");
        msgObj.transform.SetParent(screen.transform, false);
        TextMeshProUGUI msgText = msgObj.AddComponent<TextMeshProUGUI>();
        msgText.text = message;
        msgText.fontSize = 36;
        msgText.color = textColor;
        msgText.alignment = TextAlignmentOptions.Center;
        RectTransform msgRect = msgObj.GetComponent<RectTransform>();
        msgRect.anchorMin = new Vector2(0, 0.35f);
        msgRect.anchorMax = new Vector2(1, 0.55f);
        msgRect.offsetMin = Vector2.zero;
        msgRect.offsetMax = Vector2.zero;

        GameObject promptObj = new GameObject("Prompt");
        promptObj.transform.SetParent(screen.transform, false);
        TextMeshProUGUI promptText = promptObj.AddComponent<TextMeshProUGUI>();
        promptText.text = prompt;
        promptText.fontSize = 24;
        promptText.color = textColor;
        promptText.alignment = TextAlignmentOptions.Center;
        promptText.fontStyle = FontStyles.Italic;
        RectTransform promptRect = promptObj.GetComponent<RectTransform>();
        promptRect.anchorMin = new Vector2(0, 0.15f);
        promptRect.anchorMax = new Vector2(1, 0.3f);
        promptRect.offsetMin = Vector2.zero;
        promptRect.offsetMax = Vector2.zero;

        return screen;
    }

    GameObject CreateHUD()
    {
        GameObject hud = new GameObject("HUD");
        hud.transform.SetParent(transform, false);

        RectTransform rect = hud.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 0.85f);
        rect.anchorMax = new Vector2(1, 1f);
        rect.offsetMin = new Vector2(20, 0);
        rect.offsetMax = new Vector2(-20, -10);

        Image bg = hud.AddComponent<Image>();
        bg.color = new Color(0, 0, 0, 0.5f);

        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(hud.transform, false);
        hudText = textObj.AddComponent<TextMeshProUGUI>();
        hudText.text = string.Format(hudFormat, 0, 3);
        hudText.fontSize = 32;
        hudText.color = textColor;
        hudText.alignment = TextAlignmentOptions.Center;
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        return hud;
    }

    void PositionUI()
    {
        if (vrCamera == null) return;

        Vector3 forward = vrCamera.transform.forward;
        forward.y = 0;
        forward.Normalize();

        transform.position = vrCamera.transform.position + forward * uiDistance;
        transform.rotation = Quaternion.LookRotation(forward);
    }

    void Update()
    {
        if (followCamera && vrCamera != null)
            PositionUI();

        bool triggerPressed = GetVRTriggerPressed();

        if (!gameStarted)
        {
            startTimer += Time.unscaledDeltaTime;

            bool shouldStart = false;

            if (triggerPressed && !triggerWasPressed)
                shouldStart = true;

            if (autoStartDelay > 0 && startTimer >= autoStartDelay)
                shouldStart = true;

            if (shouldStart)
                StartGame();

            triggerWasPressed = triggerPressed;
            return;
        }

        if (gameWon)
        {
            if (triggerPressed && !triggerWasPressed)
                RestartGame();

            triggerWasPressed = triggerPressed;
            return;
        }

        triggerWasPressed = triggerPressed;

        UpdateHUD();
        CheckVictory();
    }

    bool GetVRTriggerPressed()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller, devices);

        foreach (var device in devices)
        {
            if (device.TryGetFeatureValue(CommonUsages.triggerButton, out bool pressed) && pressed)
                return true;

            if (device.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue) && triggerValue > 0.5f)
                return true;

            if (device.TryGetFeatureValue(CommonUsages.primaryButton, out bool primary) && primary)
                return true;
        }

        return false;
    }

    void StartGame()
    {
        gameStarted = true;
        Time.timeScale = 1f;

        if (startScreen != null)
            startScreen.SetActive(false);

        if (hudScreen != null)
            hudScreen.SetActive(true);
    }

    void UpdateHUD()
    {
        if (priestManager == null || hudText == null) return;

        int banishCount = GetBanishCount();
        int required = priestManager.requiredBanished;

        if (banishCount != lastBanishCount)
        {
            hudText.text = string.Format(hudFormat, banishCount, required);
            lastBanishCount = banishCount;
        }
    }

    int GetBanishCount()
    {
        var field = typeof(PriestManager).GetField("banishCount",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (field != null)
            return (int)field.GetValue(priestManager);

        return 0;
    }

    void CheckVictory()
    {
        if (priestManager == null) return;

        if (!priestManager.gameObject.activeSelf && GetBanishCount() >= priestManager.requiredBanished)
            ShowVictory();
    }

    void ShowVictory()
    {
        gameWon = true;

        if (victoryScreen != null)
            victoryScreen.SetActive(true);

        if (hudScreen != null)
            hudScreen.SetActive(false);
    }

    void RestartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}

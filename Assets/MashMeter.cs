using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class MashMeter : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Image barFill;
    [SerializeField] TMP_Text countdownText;
    [SerializeField] TMP_Text zoneText;
    [SerializeField] GameObject pressIndicator; 

    [Header("Timing")]
    [SerializeField] float duration = 5f;

    [Header("Values")]
    [SerializeField] float drainPerSecond = 0.35f;
    [SerializeField] float pressBoost = 0.12f;
    [SerializeField] float startFill = 0.5f;

    [Header("Zones (0..1)")]
    [SerializeField] float yellowThreshold = 0.33f;
    [SerializeField] float greenThreshold = 0.66f;

    [Header("Colors")]
    [SerializeField] Color redCol = new(0.8f, 0.2f, 0.2f);
    [SerializeField] Color yellowCol = new(0.9f, 0.8f, 0.2f);
    [SerializeField] Color greenCol = new(0.2f, 0.8f, 0.2f);

    public Action<int> onFinished; // returns 3/4/5 damage

    float t, fill;
    bool running;

    // Auto-wire if fields are left empty
    void Awake()
    {
        if (!barFill)
            barFill = transform.Find("BarFill")?.GetComponent<Image>();
        if (!countdownText)
            countdownText = transform.Find("MashTimer")?.GetComponent<TMP_Text>();
        if (!zoneText)
            zoneText = transform.Find("ZoneText")?.GetComponent<TMP_Text>();
        if (!pressIndicator)
    pressIndicator = GetComponentInChildren<Image>(true)?.gameObject; 

    }

    public void Begin(Action<int> cb)
    {
        onFinished = cb;
        t = 0f;
        fill = Mathf.Clamp01(startFill);
        running = true;
        gameObject.SetActive(true);

        if (pressIndicator)
            pressIndicator?.SetActive(true); 

        UpdateUI();
    }

    void Update()
    {
        if (!running) return;
        if (pressIndicator && !pressIndicator.activeSelf) pressIndicator.SetActive(true);


        // countdown timer
        t += Time.deltaTime;
        float remaining = Mathf.Max(0, duration - t);
        if (countdownText) countdownText.text = $"{remaining:0.0}s";

        if (t >= duration)
        {
            running = false;
            int dmg = ComputeDamage(fill);
            onFinished?.Invoke(dmg);
                pressIndicator?.SetActive(false); 

            gameObject.SetActive(false);
            return;
        }

        // mash logic (press Space to boost)
        fill -= drainPerSecond * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space)) fill += pressBoost;
        fill = Mathf.Clamp01(fill);
        UpdateUI();
    }

    void UpdateUI()
    {
        if (barFill) barFill.fillAmount = fill;

        string label; int dmg; Color c;
        if (fill >= greenThreshold) { label = "GREEN"; dmg = 5; c = greenCol; }
        else if (fill >= yellowThreshold) { label = "YELLOW"; dmg = 4; c = yellowCol; }
        else { label = "RED"; dmg = 3; c = redCol; }

        if (zoneText) { zoneText.text = $"{label} ({dmg} DMG)"; zoneText.color = c; }
        if (barFill) { barFill.color = c; }
    }

    int ComputeDamage(float f)
    {
        if (f >= greenThreshold) return 5;
        if (f >= yellowThreshold) return 4;
        return 3;
    }
}

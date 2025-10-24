using UnityEngine;
using TMPro;
using System.Collections;

public class HealthNumberUI : MonoBehaviour
{
    [SerializeField] Health target;
    [SerializeField] TMP_Text hpText;     // e.g., "Kai: 10/10 HP"
    [SerializeField] TMP_Text deltaText;  // e.g., "-3" / "+2" (optional)
    [SerializeField] float deltaShowTime = 0.6f;

    [Header("Label")]
    [SerializeField] string label = "";   // e.g., "Kai" or "Nullus". If empty, uses target.gameObject.name

    int lastValue;

    void Start()
    {
        if (!target)
        {
            Debug.LogWarning("HealthNumberUI missing target.");
            return;
        }

        if (string.IsNullOrWhiteSpace(label))
            label = target.gameObject.name;

        lastValue = target.currentHP;
        target.onChanged += OnHPChanged;
        Refresh();
    }

    void Refresh()
    {
        if (hpText) hpText.text = $"{label}: {target.currentHP}/{target.maxHP} HP";
    }

    void OnHPChanged(int cur, int max)
    {
        int diff = cur - lastValue;
        lastValue = cur;
        Refresh();

        if (!deltaText) return;

        StopAllCoroutines();
        if (diff == 0) { deltaText.text = ""; return; }

        deltaText.text = diff > 0 ? $"+{diff}" : $"{diff}";
        StartCoroutine(ClearDelta());
    }

    IEnumerator ClearDelta()
    {
        yield return new WaitForSeconds(deltaShowTime);
        deltaText.text = "";
    }
}

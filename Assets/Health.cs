using UnityEngine;
using System;
using System.Collections; // for IEnumerator
using UnityEngine.UI; // needed for Image fade effect

public class Health : MonoBehaviour
{
    [Header("Values")]
    public int maxHP = 10;
    public int currentHP;

    [Header("On Death (optional)")]
    [SerializeField] bool loadSceneOnDeath = false;
    [SerializeField] string sceneOnDeath = "";

    [SerializeField] bool useDelayBeforeLoad = false;
    [SerializeField] float delaySeconds = 0.75f;

    [Header("Visual Feedback (optional)")]
    [SerializeField] Image damageOverlay; // assign your red UI Image overlay here
    [SerializeField] float flashDuration = 0.2f; // how long the red flash lasts

    public Action<int, int> onChanged;
    public Action onDeath;

    void Awake()
    {
        currentHP = maxHP;
        if (damageOverlay != null)
            damageOverlay.color = new Color(1, 0, 0, 0); // start transparent
    }

    public void SetHP(int value)
    {
        currentHP = Mathf.Clamp(value, 0, maxHP);
        onChanged?.Invoke(currentHP, maxHP);
        CheckDeath();
    }

    public void TakeDamage(int amount)
    {
        currentHP = Mathf.Max(0, currentHP - Mathf.Max(0, amount));
        onChanged?.Invoke(currentHP, maxHP);
        TriggerDamageFeedback();
        CheckDeath();
    }

    public void Heal(int amount)
    {
        currentHP = Mathf.Min(maxHP, currentHP + Mathf.Max(0, amount));
        onChanged?.Invoke(currentHP, maxHP);
    }

    void CheckDeath()
    {
        if (currentHP > 0) return;

        onDeath?.Invoke();

        if (loadSceneOnDeath && !string.IsNullOrEmpty(sceneOnDeath))
        {
            if (useDelayBeforeLoad) StartCoroutine(LoadAfter(delaySeconds, sceneOnDeath));
            else SceneFader.LoadScene(sceneOnDeath);
        }
    }

    IEnumerator LoadAfter(float delay, string scene)
    {
        yield return new WaitForSeconds(delay);
        SceneFader.LoadScene(scene);
    }

    void TriggerDamageFeedback()
    {
        if (damageOverlay != null)
            StartCoroutine(FlashRed());
    }

    IEnumerator FlashRed()
    {
        float t = 0f;
        while (t < flashDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0.5f, 0f, t / flashDuration);
            damageOverlay.color = new Color(1, 0, 0, alpha);
            yield return null;
        }
    }
}

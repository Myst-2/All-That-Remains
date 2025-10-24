using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MiniGameController : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] Health playerHealth;   // drag Kai's Health here
    [SerializeField] TMP_Text hpText;       // drag your TMP text here (e.g., "Kai HP")

    [Header("Timer (optional)")]
    [SerializeField] bool useTimer = false; // set true in Inspector if you want a timer
    [SerializeField] float duration = 10f;
    [SerializeField] TMP_Text countdownText;

    float t;

    void OnEnable()
    {
        if (playerHealth != null)
        {
            // If we came from battle, restore HP snapshot
            if (BattleData.playerHP > 0)
                playerHealth.SetHP(BattleData.playerHP);

            // Subscribe to health changes to keep UI in sync
            playerHealth.onChanged += OnHealthChanged;
            playerHealth.onDeath   += OnPlayerDeath;

            // Draw once
            OnHealthChanged(playerHealth.currentHP, playerHealth.maxHP);
        }
        else
        {
            Debug.LogWarning("MiniGameController: playerHealth not assigned.");
        }

        if (useTimer)
        {
            t = duration;
            UpdateTimerUI();
        }
    }

    void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.onChanged -= OnHealthChanged;
            playerHealth.onDeath   -= OnPlayerDeath;
        }
    }

    void Update()
    {
        if (!useTimer) return;

        t -= Time.deltaTime;
        if (t <= 0f)
        {
            ExitToBattle();
            return;
        }
        UpdateTimerUI();
    }

    void OnHealthChanged(int cur, int max)
    {
        if (hpText) hpText.text = $"Kai: {cur}/{max}";
        if (cur <= 0) ExitToBattle();
    }

    void OnPlayerDeath()
    {
        ExitToBattle();
    }

    void UpdateTimerUI()
    {
        if (countdownText) countdownText.text = $"{Mathf.Ceil(Mathf.Max(0f, t))}s";
    }

    void ExitToBattle()
    {
        // Persist the final HP back to battle
        if (playerHealth != null)
            BattleData.playerHP = playerHealth.currentHP;

        var sceneToReturn = string.IsNullOrEmpty(BattleData.returnScene) ? "Battle" : BattleData.returnScene;
        SceneManager.LoadScene(sceneToReturn);
    }
    
}
 
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public enum BattleState { Intro, PlayerCommand, PlayerConfront, EnemyTurn, Win, Lose }

public class BattleManager : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] Health playerHealth;          // Kai
    [SerializeField] Health enemyHealth;           // Nullus
    [SerializeField] GameObject introPanel;
    [SerializeField] TMP_Text introText;
    [SerializeField] GameObject commandPanel;
    [SerializeField] MashMeter mash;

    [Header("Optional: freeze player controls during battle")]
    [SerializeField] Rigidbody2D playerRB;                 // optional
    [SerializeField] MonoBehaviour[] playerControls;       // e.g. PlayerMovement

    [Header("Optional: visuals")]
    [SerializeField] GameObject enemyVisual;               // Nullus sprite root
    [SerializeField] GameObject playerVisual;              // Kai sprite root

    [Header("Config")]
    [SerializeField] int resistHeal = 2;
    [SerializeField] string gameOverScene = "Death";

    private BattleState state;

    void Start()
    {
        // 1) Subscribe FIRST so SetHP can immediately trigger onDeath if HP==0
        playerHealth.onDeath += () => SetState(BattleState.Lose);
        enemyHealth.onDeath  += () => SetState(BattleState.Win);

        // 2) Reset data only on first entry (not when returning from minigame)
        if (BattleData.playerHP < 0 && BattleData.enemyHP < 0)
        {
            BattleData.Clear(); // reset everything
        }

        // 3) Decide starting HPs (these SetHP calls can now trigger onDeath if zero)
        if (BattleData.playerHP < 0)
        {
            // Fresh run
            playerHealth.SetHP(playerHealth.maxHP);
            BattleData.playerHP = playerHealth.maxHP;
        }
        else
        {
            // Returning from minigame
            playerHealth.SetHP(Mathf.Min(BattleData.playerHP, playerHealth.maxHP));
        }

        if (BattleData.enemyHP < 0)
        {
            enemyHealth.SetHP(enemyHealth.maxHP);
            BattleData.enemyHP = enemyHealth.maxHP;
        }
        else
        {
            enemyHealth.SetHP(Mathf.Min(BattleData.enemyHP, enemyHealth.maxHP));
        }

        // 4) Normal intro setup
        if (playerVisual) playerVisual.SetActive(true);
        if (enemyVisual)  enemyVisual.SetActive(true);

        FreezePlayer(true);
        StartCoroutine(DoIntro());
    }

    IEnumerator DoIntro()
    {
        introPanel.SetActive(true);
        commandPanel.SetActive(false);
        mash.gameObject.SetActive(false);

        if (introText) introText.text = "Nullus is wishing to consume you";
        yield return new WaitForSeconds(1.5f);

        SetState(BattleState.PlayerCommand);
    }

    void SetState(BattleState next)
    {
        state = next;

        introPanel.SetActive(state == BattleState.Intro);
        commandPanel.SetActive(state == BattleState.PlayerCommand);
        mash.gameObject.SetActive(state == BattleState.PlayerConfront);

        if (state == BattleState.Win)
        {
            // Win → go to your win scene
            StartCoroutine(LoadSceneNextFrame("DestroyedHouse"));
        }
        else if (state == BattleState.Lose)
        {
            // Lose → go to Death
            StartCoroutine(LoadSceneNextFrame(gameOverScene)); // "Death"
        }
    }

    IEnumerator LoadSceneNextFrame(string sceneName)
    {
        // one frame delay avoids conflicts with UI events/anim callbacks
        yield return null;
        SceneManager.LoadScene(sceneName);
    }

    // ===== UI Buttons =====
    public void OnConfront()
    {
        if (state != BattleState.PlayerCommand) return;
        SetState(BattleState.PlayerConfront);
        mash.Begin(OnMashFinished);
    }

    public void OnResist()
    {
        if (state != BattleState.PlayerCommand) return;

        playerHealth.Heal(resistHeal);
        StartCoroutine(GoToMinigameAfterFrame());
    }

    IEnumerator GoToMinigameAfterFrame()
    {
        yield return null;
        GoToMinigame();
    }

    void OnMashFinished(int dmg)
    {
        enemyHealth.TakeDamage(dmg);

        if (enemyHealth.currentHP > 0)
        {
            GoToMinigame();
        }
        // If enemy dies here, enemyHealth.onDeath → SetState(Win) → scene load
    }

    IEnumerator EnemyTurn()
    {
        SetState(BattleState.EnemyTurn);
        yield return new WaitForSeconds(0.8f);

        if (playerHealth.currentHP > 0 && enemyHealth.currentHP > 0)
            SetState(BattleState.PlayerCommand);
    }

    void FreezePlayer(bool freeze)
    {
        if (playerControls != null)
        {
            foreach (var mb in playerControls)
                if (mb) mb.enabled = !freeze;
        }

        if (playerRB)
        {
#if UNITY_600_0_OR_NEWER
            playerRB.linearVelocity = Vector2.zero;
#else
            playerRB.linearVelocity = Vector2.zero;
#endif
        }
    }

    void GoToMinigame()
    {
        // Save current HPs before switching scenes
        BattleData.SavePlayer(playerHealth);
        BattleData.SaveEnemy(enemyHealth);
        BattleData.returnScene = "Battle";

        Debug.Log("[BattleManager] Going to minigame... HP snapshot saved.");
        SceneManager.LoadScene("TestMinigame");
    }
}

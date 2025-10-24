using UnityEngine;

public static class BattleData
{
    // Player
    public static int playerHP   = -1;
    public static int playerMaxHP = -1;

    // Enemy
    public static int enemyHP    = -1;
    public static int enemyMaxHP = -1;

    // Scene to return to (minigame/cutscene hops)
    public static string returnScene = "Battle";

    // --- Helpers ---
    public static void SavePlayer(Health h)
    {
        if (!h) return;
        playerHP = h.currentHP;
        playerMaxHP = h.maxHP;
    }

    public static void ApplyPlayer(Health h)
    {
        if (!h) return;
        if (playerHP >= 0) h.SetHP(Mathf.Clamp(playerHP, 0, h.maxHP));
    }

    public static void SaveEnemy(Health h)
    {
        if (!h) return;
        enemyHP = h.currentHP;
        enemyMaxHP = h.maxHP;
    }

    public static void ApplyEnemy(Health h)
    {
        if (!h) return;
        if (enemyHP >= 0) h.SetHP(Mathf.Clamp(enemyHP, 0, h.maxHP));
    }

    // --- CLEAR ALL DATA ---
    public static void Clear()
    {
        playerHP = playerMaxHP = -1;
        enemyHP  = enemyMaxHP  = -1;
        returnScene = "Battle";
    }

    public static bool HasPlayerData()
    {
        return playerHP >= 0 && playerMaxHP > 0;
    }
}

// BaseMinigame.cs
using UnityEngine;
using System;

public abstract class BaseMinigame : MonoBehaviour
{
    protected Health playerHP;
    protected Transform player;           // player transform inside arena
    protected Action onDone;              // callback to BattleManager when finished
    protected Rect arena;                 // local-space play area (x,y = center)

    
    public virtual void Begin(Health playerHP, Transform player, Rect arena, Action onDone)
    {
        this.playerHP = playerHP;
        this.player   = player;
        this.arena    = arena;
        this.onDone   = onDone;
        gameObject.SetActive(true);
    }

    
    protected void Finish() => onDone?.Invoke();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public static PlayerData current;

    #region SCORE
    public int currentScore = 0;
    #endregion

    #region MOVEMENT
    public float moveSpeed = 3.7f;
    #endregion

    #region HEALTH
    public int maxHP = 50;
    public int currentHP;
    #endregion

    #region ATTACK
    public float attackCooldown = 0.6f;
    #endregion

    public PlayerData()
    {
        currentHP = maxHP;
    }

    public void Reset()
    {
        currentScore = 0;
        currentHP = maxHP;
    }
}

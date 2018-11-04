using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public static PlayerData current;

    #region LIMB POWER
    public float moveSpeedBonusPerLeg = 0.35f;
    #region

    #region SCORE
    public int currentScore = 0;
    #endregion

    #region MOVEMENT
    float defaultMoveSpeed = 3.7f;
    public float moveSpeed = 3.7f;
    #endregion

    #region HEALTH
    public int maxHP = 50;
    public int currentHP;
    #endregion

    #region ATTACK
    public float attackCooldown = 0.6f;
    #endregion
    #endregion
    public PlayerData()
    {
        currentHP = maxHP;
    }

    public void Reset()
    {
        moveSpeed = defaultMoveSpeed;
        currentScore = 0;
        currentHP = maxHP;
    }
}
#endregion
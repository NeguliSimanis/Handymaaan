﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public static PlayerData current;

    #region MOVEMENT
    public float moveSpeed = 3.2f;
    #endregion

    #region HEALTH
    public int maxHP = 100;
    public int currentHP;
    #endregion

    #region ATTACK
    public float attackCooldown = 0.6f;
    #endregion

    public PlayerData()
    {
        currentHP = maxHP;
    }
}

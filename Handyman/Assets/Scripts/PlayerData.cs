using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public static PlayerData current;

    #region MOVEMENT
    public float moveSpeed = 3.2f;
    #endregion

    #region ATTACK
    public float attackCooldown = 0.6f;
    #endregion
}

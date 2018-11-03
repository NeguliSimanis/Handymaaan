using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    Image hpBar;
    
	void Start ()
    {
		if (PlayerData.current == null)
        {
            PlayerData.current = new PlayerData();
        }
	}
	void Update ()
    {
        hpBar.fillAmount = (PlayerData.current.currentHP* 1f) / PlayerData.current.maxHP;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealth : MonoBehaviour
{
    public float maxHP;
    private float curHP;

    private void Start()
    {
        curHP = maxHP;
    }
    public void TakeDamage(float damageToTake)
    {
        curHP -= damageToTake;
        HealthCheck();
    }

    private void HealthCheck()
    {
        if (curHP <= 0)
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealth : MonoBehaviour
{
    public float maxHP;
    [SerializeField] private float curHP;
    //messy...
    public bool isEnemey = false;
    public bool isPlayer = false;
    private void Start()
    {
        curHP = maxHP;
    }
    public void TakeDamage(float damageToTake, UnitHealth attacker)
    {
        curHP -= damageToTake;
        HealthCheck();
        if (isPlayer) { return; }
        if (isEnemey)
        {
            GetComponent<EnemyBehavior>().SetAttack(attacker);
        }
        else
        {
            GetComponent<FollowerBehavior>().SetAttack(attacker);
        }
    }

    private void HealthCheck()
    {
        if (curHP <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetHealth(float maxHealth)
    {
        maxHP = maxHealth;
        curHP = maxHP;
    }
}

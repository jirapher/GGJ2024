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
    public bool isBank = false;

    private RewardsSpawner rewardsSpawn;
    private void Start()
    {
        curHP = maxHP;
        if(TryGetComponent<RewardsSpawner>(out RewardsSpawner rewards))
        {
            rewardsSpawn = rewards;
        }
    }
    public void TakeDamage(float damageToTake, UnitHealth attacker)
    {
        curHP -= damageToTake;
        HealthCheck();

        if (isPlayer || isBank) { return; }

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
            if (isEnemey)
            {
                StartCoroutine(FadeOut());
            }

            if (isPlayer)
            {
                print("game over");
            }

            if (isBank)
            {
                BankDeath();
            }
        }
    }

    public void SetHealth(float maxHealth)
    {
        maxHP = maxHealth;
        curHP = maxHP;
    }

    public void InitDeath()
    {
        //enemymanager subtract
        Destroy(gameObject);
    }

    private IEnumerator FadeOut()
    {
        SpriteRenderer sr = GetComponent<EnemyBehavior>().sr;
        Color newCol = sr.material.color;
        while(newCol.a > 0)
        {
            newCol.a -= 0.3f * Time.deltaTime;
            sr.material.color = newCol;
            yield return null;
        }

        if(rewardsSpawn != null)
        {
            rewardsSpawn.SpawnReward(isBank);
            //this calls init death for enems
        }
        yield break;
    }

    private void BankDeath()
    {
        if(rewardsSpawn != null)
        {
            rewardsSpawn.SpawnReward(isBank);
        }
        GetComponent<EnemyBehavior>().anim.SetTrigger("Destroy");
        Destroy(gameObject, 6.2f);
    }
}

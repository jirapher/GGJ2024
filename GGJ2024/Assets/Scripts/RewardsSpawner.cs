using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardsSpawner : MonoBehaviour
{
    public GameObject[] possibleSpawns;
    private UnitHealth health;

    private void Start()
    {
        health = GetComponent<UnitHealth>();
    }
    public void SpawnReward(bool isBank)
    {
        float dice = Random.Range(0, 100);
        if (isBank) { dice = 100; }
        if(dice > 70) { return; }

        int amt = Random.Range(0, 4);

        if (isBank) { amt += Random.Range(3, 6); }

        int reward = Random.Range(0, possibleSpawns.Length);
        StartCoroutine(SpawnItems(amt, reward, isBank));
    }

    private IEnumerator SpawnItems(int amount, int reward, bool bank)
    {
        while(amount > 0)
        {
            Vector3 randomLoc = new Vector3(transform.position.x + Random.Range(-1, 1), transform.position.y + Random.Range(-1, 1), 0);
            Instantiate(possibleSpawns[reward], randomLoc, Quaternion.identity);
            amount--;
            yield return null;
        }

        if (bank) { yield break; }

        health.InitDeath();
    }
}

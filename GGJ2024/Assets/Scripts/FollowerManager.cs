using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerManager : MonoBehaviour
{
    //basically a list that tracks all alive followers
    public static FollowerManager instance;
    public List<FollowerBehavior> allFollowers = new List<FollowerBehavior>();
    public FollowerStats[] allStats;
    public GameObject followerPrefab;
    //create new follower

    private void Awake()
    {
        instance = this;
    }
    public List<FollowerBehavior> GetAllFollowers()
    {
        return allFollowers;
    }

    public void AddFollower(FollowerBehavior unit)
    {
        allFollowers.Add(unit);
    }

    public void CreateNewFollower(int type, Vector2 location)
    {
        GameObject g = Instantiate(followerPrefab, location, Quaternion.identity);
        FollowerBehavior unit = g.GetComponent<FollowerBehavior>();
        unit.SetStats(allStats[type]);
        AddFollower(unit);
    }


}

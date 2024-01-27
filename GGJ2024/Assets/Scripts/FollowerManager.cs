using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerManager : MonoBehaviour
{
    //basically a list that tracks all alive followers
    public static FollowerManager instance;
    public List<FollowerBehavior> allFollowers = new List<FollowerBehavior>();

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


}

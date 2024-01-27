using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/FollowerStats", order = 1)]
public class FollowerStats : ScriptableObject
{
    public int type = -1;
    public LayerMask interactableLayer;
    public Sprite sprite;
    public RuntimeAnimatorController animC;
    //0 : copper, 1 : silver, 2 : gold
    public float maxHP = 1;

    public float timeBetweenAttacks, attackRange, attackDamage;
    public float stoppingDistance, moveSpeed;

    public float detectRadius, detectDistance, detectIntervalTime;


}

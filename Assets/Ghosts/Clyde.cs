using Assets;
using Assets.Ghosts.ChaseStrategies;
using UnityEngine;

public class Clyde : Ghost
{
    public Clyde()
    {
        DeadTimer = 2f;
        chaseStrategy = new FollowStrategy();
    }
}

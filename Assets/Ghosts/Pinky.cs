using Assets;
using Assets.Ghosts.ChaseStrategies;
using UnityEngine;

public class Pinky : Ghost
{
    public Pinky()
    {
        DeadTimer = 4f;
        chaseStrategy = new FollowStrategy();
    }
}

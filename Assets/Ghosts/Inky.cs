using Assets;
using Assets.Ghosts.ChaseStrategies;
using UnityEngine;

public class Inky : Ghost
{
    public Inky()
    {
        DeadTimer = 3f;
        chaseStrategies.Add(new FollowStrategy());
    }
}

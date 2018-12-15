using Assets;
using Assets.Ghosts.ChaseStrategies;
using UnityEngine;

public class Pinky : Ghost
{
    public Pinky()
    {
        //DeadTimer = 4f;
        DeadTimer = 0;
        //chaseStrategies.Add(new AmbushStrategy());
        chaseStrategies.Add(new WandererStrategy());
    }
}

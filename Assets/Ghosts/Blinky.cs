using Assets;
using Assets.Ghosts.ChaseStrategies;
using UnityEngine;

public class Blinky : Ghost
{
    public Blinky()
    {
        DeadTimer = 1;
        ScatterTimer = 10;
        chaseStrategies.Add(new FollowStrategy());
    }
    protected override void Start()
    {
        base.Start();
        SetScatterSpawns(GameObject.Find("BlinkyScatter"));
    }
}

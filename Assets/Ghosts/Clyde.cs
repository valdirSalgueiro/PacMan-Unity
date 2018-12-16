using Assets;
using Assets.Ghosts.ChaseStrategies;
using UnityEngine;

public class Clyde : Ghost
{
    public Clyde()
    {
        DeadTimer = 2f;
        ScatterTimer = 16f;

        chaseStrategies.Add(new ChickenStrategy());
    }

    protected override void Start()
    {
        base.Start();
        SetScatterSpawns(GameObject.Find("ClydeScatter"));
    }
}

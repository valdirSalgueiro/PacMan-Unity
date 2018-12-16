using Assets;
using Assets.Ghosts.ChaseStrategies;
using UnityEngine;

public class Pinky : Ghost
{
    public Pinky()
    {
        
        DeadTimer = 4f;
        ScatterTimer = 28;
        chaseStrategies.Add(new AmbushStrategy());
        chaseStrategies.Add(new WandererStrategy());
    }

    protected override void Start()
    {
        base.Start();
        SetScatterSpawns(GameObject.Find("PinkyScatter"));
    }
}

using Assets;
using Assets.Ghosts.ChaseStrategies;

public class Blinky : Ghost
{
    public Blinky()
    {
        DeadTimer = 1;
        chaseStrategies.Add(new FollowStrategy());
    }
}

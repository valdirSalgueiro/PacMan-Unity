using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Ghosts.ChaseStrategies
{
    public interface IChaseStrategy
    {
        void Start(Ghost ghost, Ghost blinky);
        bool Chase(Ghost ghost);
    }
}

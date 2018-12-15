using Assets.Ghosts.ChaseStrategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Ghosts
{
    public interface IGhostState
    {
        void Start(Ghost ghost);
        IGhostState Update(Ghost ghost);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Ghosts
{
    public interface GhostState
    {
        void update(Ghost ghost);
    }
}

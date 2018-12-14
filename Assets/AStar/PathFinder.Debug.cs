using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Viewer")]

namespace RoyT.AStar
{
    
    internal static partial class PathFinder
    {
        private static List<Position> PartiallyReconstructPath(Grid grid, Position start, Position end, Position[] cameFrom)
        {
            var path = new List<Position> { end };
            return path;
        }
    }

    internal class Step
    {
        public Step(StepType type, Position position, List<Position> path)
        {
            this.Type = type;
            this.Position = position;
            this.Path = path;
        }

        public StepType Type;
        public Position Position;
        public List<Position> Path;
    }

    internal enum StepType
    {
        Current,
        Open,
        Close
    }
}

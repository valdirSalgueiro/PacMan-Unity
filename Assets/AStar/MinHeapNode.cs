namespace RoyT.AStar
{
    /// <summary>
    /// Node in a heap
    /// </summary>
    internal sealed class MinHeapNode
    {        
        public MinHeapNode(Position position, float expectedCost)
        {
            this.Position     = position;
            this.ExpectedCost = expectedCost;            
        }

        public Position Position;
        public float ExpectedCost;
        public MinHeapNode Next;
    }
}

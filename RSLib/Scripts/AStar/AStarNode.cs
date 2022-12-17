namespace RSLib.AStar
{
    public abstract class AStarNode<T> : Framework.Collections.IHeapElement<T>
    {
        public int GCost;
        public int HCost;
        public int FCost => GCost + HCost;

        public IAStarMesh AStarMesh;
        public T ParentNode;
        public System.Collections.Generic.List<T> Neighbours;
        public bool IsNodeAvailable = true;

        public int HeapIndex { get; set; }
        
        public abstract int CostToNode(T node);
        
        public int CompareTo(T other)
        {
            AStarNode<T> otherNode = other as AStarNode<T>;
            UnityEngine.Assertions.Assert.IsNotNull(otherNode);
            
            int comparison = FCost.CompareTo(otherNode.FCost);
            if (comparison == 0)
                comparison = HCost.CompareTo(otherNode.HCost);

            return -comparison;
        }
        
        /// <summary>
        /// Resets the costs that are used to find a path.
        /// Should be called before the A* computation.
        /// </summary>
        public virtual void Reset()
        {
            GCost = 0;
            HCost = 0;
        }
    }
}
namespace RSLib.AStar
{
    public interface IAStarNode<T> : Framework.Collections.IHeapElement<T>
    {
        int GCost { get; set; }
        int HCost { get; set; }
        int FCost { get; }
        
        IAStarMesh AStarMesh { get; }
        T ParentNode { get; set; }
        System.Collections.Generic.List<T> Neighbours { get; }
        
        bool IsNodeAvailable { get; }
        
        int CostToNode(T node);
    }
}
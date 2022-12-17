namespace RSLib.AStar
{
    public interface IAStarMesh
    {
        /// <summary>
        /// Mesh nodes count.
        /// </summary>
        int Size { get; }
        
        /// <summary>
        /// Resets the costs used to find a path.
        /// Should be called before the A* computation.
        /// </summary>
        void ResetNodes();
    }
}
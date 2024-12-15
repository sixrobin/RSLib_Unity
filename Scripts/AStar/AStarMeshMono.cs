namespace RSLib.Unity.AStar
{
    using UnityEngine;

    /// <summary>
    /// Main class of any A* mesh.
    /// Contains event for adding or removing node, the size of the mesh and an abstract method to generate it.
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class AStarMeshMono : MonoBehaviour, IAStarMesh
    {
        public abstract int Size { get; }

        public delegate void NodeChangeEventHandler(AStarNodeMono node);

        public event NodeChangeEventHandler NodeAdded;
        public event NodeChangeEventHandler NodeRemoved;

        public abstract void ResetNodes();

        /// <summary>
        /// Checks if a node is already in the mesh.
        /// </summary>
        public abstract bool ContainsNode(AStarNodeMono node);

        public virtual void AddNode(AStarNodeMono node)
        {
            NodeAdded?.Invoke(node);
        }

        public virtual void AddNode(AStarNodeMono node, System.Collections.Generic.List<AStarNodeMono> nodeNeighbours)
        {
            NodeAdded?.Invoke(node);
        }

        public virtual void RemoveNode(AStarNodeMono node)
        {
            NodeRemoved?.Invoke(node);
        }

        /// <summary>
        /// Determines the way the mesh is constructed and how it is represented in code.
        /// </summary>
        protected abstract void Bake();

        protected virtual void Start()
        {
            Bake();
        }
    }
}
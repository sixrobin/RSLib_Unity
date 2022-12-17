namespace RSLib.AStar
{
	/// <summary>
	/// Main class of any A* mesh type node.
	/// Holds information about the pathfinding, along with some events and the node's world position.
	/// </summary>
	public abstract class AStarNodeMono : AStarNode<AStarNodeMono>
	{
		/// <summary>
		/// World position of the node, used for gameObjects path follow.
		/// </summary>
        public UnityEngine.Vector3 WorldPos;

		/// <summary>
		/// One by default, increase the value to make the node more expensive during the A* research.
		/// </summary>
		public int BaseCost { get; }

		public AStarNodeMono(UnityEngine.Vector3 worldPos, int baseCost)
		{
			WorldPos = worldPos;
			BaseCost = baseCost;
		}

		/// <summary>
		/// Sets the mesh in which the node is belonging and adds listeners to this mesh events.
		/// </summary>
		/// <param name="mesh">The holding mesh.</param>
		public void SetMesh(AStarMeshMono mesh)
		{
			AStarMesh = mesh;
			mesh.NodeAdded += OnNodeAdded;
			mesh.NodeRemoved += OnNodeRemoved;
		}

		/// <summary>
		/// Resets the costs that are used to find a path.
		/// Should be called before the A* computation.
		/// </summary>
		public void Reset()
		{
			GCost = 0;
			HCost = 0;
		}

		/// <summary>
		/// Called when a new node is added to the holding mesh.
		/// </summary>
		/// <param name="node">The new node.</param>
		public virtual void OnNodeAdded(AStarNodeMono node)
        {
        }

		/// <summary>
		/// Called when a node is removed from the holding mesh.
		/// </summary>
		/// <param name="node">The removed node.</param>
		public virtual void OnNodeRemoved(AStarNodeMono node)
        {
        }
	}
}
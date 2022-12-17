namespace RSLib.AStar
{
	/// <summary>
	/// Main class of any A* mesh type node.
	/// Holds information about the pathfinding, along with some events and the node's world position.
	/// </summary>
	public abstract class AStarNodeMono : IAStarNode<AStarNodeMono>
	{
		/// <summary>
		/// World position of the node, used for gameObjects path follow.
		/// </summary>
        public UnityEngine.Vector3 WorldPos;

        public int HeapIndex { get; set; }

		public int GCost { get; set; }
        public int HCost { get; set; }
        public int FCost => GCost + HCost;

        public IAStarMesh AStarMesh { get; private set; }
        public AStarNodeMono ParentNode { get; set; }
        public System.Collections.Generic.List<AStarNodeMono> Neighbours { get; set; }

		/// <summary>
		/// One by default, increase the value to make the node more expensive during the A* research.
		/// </summary>
		public int BaseCost { get; }

        /// <summary>
        /// Node will be ignored during the A* research if this is false.
        /// </summary>
        public bool IsNodeAvailable { get; set; } = true;

		public AStarNodeMono(UnityEngine.Vector3 worldPos, int baseCost)
		{
			WorldPos = worldPos;
			BaseCost = baseCost;
		}

		public int CompareTo(AStarNodeMono node)
		{
			int comparison = FCost.CompareTo(node.FCost);
			if (comparison == 0)
				comparison = HCost.CompareTo(node.HCost);

			return -comparison;
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
		/// Determines the cost from this node to another.
		/// Simple comparison between both nodes WorldPos can work but some mesh types could use better calculations.
		/// </summary>
		/// <param name="node">The compared node.</param>
		/// <returns>The cost to move to the other node.</returns>
		public abstract int CostToNode(AStarNodeMono node);

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
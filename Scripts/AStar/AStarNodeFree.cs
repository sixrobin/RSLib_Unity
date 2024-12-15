namespace RSLib.Unity.AStar
{
	using UnityEngine;

	/// <summary>
	/// Node belonging to any A* free mesh.
	/// </summary>
	public class AStarNodeFree : AStarNodeMono
    {
		public AStarNodeFree(Vector3 worldPos, int baseCost) : base(worldPos, baseCost)
		{

		}

		public override void OnNodeRemoved(AStarNodeMono node)
		{
			if (node == this)
				return;

			if (Neighbours.Contains(node))
				Neighbours.Remove(node);
		}

		/// <summary>
		/// Compares both nodes using their world positions, adding their base cost.
		/// </summary>
		/// <param name="node">The compared node.</param>
		/// <returns>The cost to move to the other node.</returns>
		public override int CostToNode(AStarNodeMono node)
        {
			return (int)(WorldPos - node.WorldPos).sqrMagnitude + BaseCost * BaseCost;
        }

		/// <summary>
		/// Removes the node from the mesh it belongs to.
		/// </summary>
		public void RemoveFromMesh()
		{
			((AStarMeshFree)AStarMesh).RemoveNode(this);
		}
	}
}
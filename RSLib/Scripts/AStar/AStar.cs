namespace RSLib.AStar
{
	public sealed class AStar<T> where T : class, IAStarNode<T>
	{
		/// <summary>
		/// Returns the path to follow to go from a starting node to a destination node if both are in the same mesh.
		/// </summary>
		/// <param name="start">The starting node.</param>
		/// <param name="destination">The destination node.</param>
		/// <returns>The path to follow.</returns>
		public System.Collections.Generic.List<T> FindPath(T start, T destination)
		{
			// Precondition checks.
			UnityEngine.Assertions.Assert.IsTrue(start != destination, $"A* error: {nameof(start)} node and {nameof(destination)} node are the same.");
			UnityEngine.Assertions.Assert.IsTrue(start.AStarMesh != null, $"A* error: {nameof(start)} node mesh is null.");
			UnityEngine.Assertions.Assert.IsTrue(destination.AStarMesh != null, $"A* error: {nameof(destination)} node mesh is null.");
			UnityEngine.Assertions.Assert.IsTrue(start.AStarMesh == destination.AStarMesh, $"A* error: {nameof(start)} node and {nameof(destination)} don't belong to the same mesh.");

			start.AStarMesh.ResetNodes();
			
			Framework.Collections.Heap<T> openSet = new Framework.Collections.Heap<T>(start.AStarMesh.Size);
			System.Collections.Generic.HashSet<T> closeSet = new System.Collections.Generic.HashSet<T>();

			openSet.Add(start);

			while (openSet.Count > 0)
			{
                T currentNode = openSet.RemoveFirst();

				if (currentNode == destination)
					return Retrace(start, destination);

				closeSet.Add(currentNode);

				foreach (T neighbour in currentNode.Neighbours)
				{
					if (!neighbour.IsNodeAvailable || closeSet.Contains(neighbour))
						continue;

					int neighbourCost = currentNode.GCost + neighbour.CostToNode(currentNode);

					if (neighbourCost < neighbour.GCost || !openSet.Contains(neighbour))
					{
						neighbour.GCost = neighbourCost;
						neighbour.HCost = currentNode.GCost + neighbour.CostToNode(currentNode);
						neighbour.ParentNode = currentNode;

						if (!openSet.Contains(neighbour))
							openSet.Add(neighbour);
					}
				}
			}

			UnityEngine.Debug.LogError("A* error: No path found!");
			return null;
		}

		/// <summary>
		/// Rolls back the evaluated path once the A* algorithm is done processing.
		/// Starts loop from end node and traces back using each node parent.
		/// </summary>
		/// <param name="start">The starting node.</param>
		/// <param name="destination">The destination node.</param>
		/// <returns>The retraced back path.</returns>
		private static System.Collections.Generic.List<T> Retrace(T start, T destination)
		{
            System.Collections.Generic.List<T> path = new System.Collections.Generic.List<T>();
            T currentNode = destination;

			while (currentNode != start)
			{
				path.Add(currentNode);
				currentNode = (T)currentNode.ParentNode;
			}

			path.Add(start);
			path.Reverse();
			return path;
		}
	}
}
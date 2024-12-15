namespace RSLib.Unity.AStar
{
    using UnityEngine;

    /// <summary>
    /// Free mesh where nodes are placed manually in the scene.
    /// Has an array of AStarNodeFreeGO to generate the mesh on start.
    /// </summary>
    public class AStarMeshFree : AStarMeshMono
    {
        [Header ("NODES BAKED ON START")]
        // Nodes that are added to the mesh on start.
        [SerializeField]
        private AStarNodeFreeGO[] _nodesGO = null;

        public System.Collections.Generic.List<AStarNodeFree> Mesh { get; } = new System.Collections.Generic.List<AStarNodeFree>();

        public override int Size => Mesh.Count;

        public override void ResetNodes()
        {
            foreach (AStarNodeFree node in Mesh)
                node.Reset();
        }

        public override bool ContainsNode(AStarNodeMono node)
        {
            return Mesh.Contains(node as AStarNodeFree);
        }

        public override void AddNode(AStarNodeMono node)
        {
            base.AddNode(node);

            if (ContainsNode(node))
                return;

            node.SetMesh(this);
            Mesh.Add((AStarNodeFree)node);
        }

        public override void AddNode(AStarNodeMono node, System.Collections.Generic.List<AStarNodeMono> nodeNeighbours)
        {
            base.AddNode(node, nodeNeighbours);

            if (ContainsNode(node))
                return;

            node.SetMesh(this);
            Mesh.Add((AStarNodeFree)node);

            foreach (AStarNodeFree potentialNeighbour in Mesh)
                if (node.Neighbours.Contains(potentialNeighbour))
                    potentialNeighbour.Neighbours.Add(node);
        }

        public override void RemoveNode(AStarNodeMono node)
        {
            base.RemoveNode(node);
            Mesh.Remove((AStarNodeFree)node);
        }

        protected override void Bake()
        {
            foreach (AStarNodeFreeGO node in _nodesGO)
            {
                node.Node.SetMesh(this);
                Mesh.Add(node.Node);
            }
        }
    }
}
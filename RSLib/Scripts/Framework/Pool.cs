namespace RSLib.Framework
{
	using System.Collections.Generic;
	using UnityEngine;

    [DisallowMultipleComponent]
	public class Pool : Singleton<Pool>
	{
        [System.Serializable]
        public class PooledObject
        {
            [SerializeField] private string _id = string.Empty;
            [SerializeField] private GameObject _gameObject = null;
            [SerializeField] private int _quantity = 10;

            public PooledObject(GameObject gameObject, int quantity)
            {
                _gameObject = gameObject;
                _id = gameObject.name;
                _quantity = quantity;
            }

            public GameObject GameObject => _gameObject;
            public string Id => _id;
            public int Quantity => _quantity;
        }

        [SerializeField] private PooledObject[] _pooledObjects = null;

		private static Dictionary<int, Queue<GameObject>> s_poolsByGameObject = new Dictionary<int, Queue<GameObject>>();
		private static Dictionary<string, Queue<GameObject>> s_poolsById = new Dictionary<string, Queue<GameObject>>();

		/// <summary>Clears all the pooled objects.</summary>
		public static void Clear()
        {
			s_poolsByGameObject.Clear();
			s_poolsById.Clear();
        }

		/// <summary>Gets a pooled gameObject using a GameObject reference, and creates a new pool if none has been found.</summary>
		/// <param name="gameObject">Reference gameObject to find a pooled instance of.</param>
		/// <returns>Instance of the gameObject.</returns>
		public static GameObject Get(GameObject gameObject)
		{
			int poolKey = gameObject.GetInstanceID();

			if (!s_poolsByGameObject.ContainsKey(poolKey))
			{
				Instance.LogWarning("Trying to get a pooled object that has not been pooled, creating new pool of 10 objects.");
				GenerateNewPool(new PooledObject(gameObject, 10));
			}

			GameObject result = s_poolsByGameObject[poolKey].Dequeue();
			s_poolsByGameObject[poolKey].Enqueue(result);
			result.SetActive(true);

			return result.gameObject;
		}

		/// <summary>Gets a pooled gameObject using an Id, and returns null if no pool has been found.</summary>
		/// <param name="id">Reference ID to find a pool of.</param>
		/// <returns>Instance of a gameObject of the pool corresponding to the ID.</returns>
		public static GameObject Get(string id)
		{
			if (!s_poolsById.ContainsKey(id))
			{
				Instance.LogError("Trying to get a pooled object with ID that has not been pooled.");
				return null;
			}

			GameObject result = s_poolsById[id].Dequeue();
			s_poolsById[id].Enqueue(result);
			result.SetActive(true);

			return result.gameObject;
		}

        /// <summary>Checks if a pool with a given Id is known.</summary>
        /// <param name="id">Reference Id to find a pool of.</param>
        /// <returns>True if the given Id has been found, else false.</returns>
        public static bool ContainsId(string id)
        {
            return s_poolsById.ContainsKey(id);
        }

        /// <summary>Gets an IEnumerable of all the pools Ids.</summary>
        /// <returns>IEnumerable of all Ids.</returns>
        public static IEnumerable<string> GetPoolsIds()
        {
            return s_poolsById.Keys;
        }

		/// <summary>Sends back a gameObject to the pool, setting the pool transform as its parent, and setting it inactive.</summary>
		/// <param name="gameObject">GameObject to send back to pool.</param>
		public void SendBackToPool(GameObject gameObject)
		{
			gameObject.transform.SetParent(transform);
			gameObject.SetActive(false);
		}

		/// <summary>Sends back a transform's gameObject to the pool, setting the pool transform as its parent, and setting it inactive.</summary>
		/// <param name="transform">Transform to send the gameObject back to pool.</param>
		public void SendBackToPool(Transform transform)
		{
			transform.SetParent(transform);
			transform.gameObject.SetActive(false);
		}

		/// <summary>
		/// Creates a new pool using an instance of PooledObject, which is a class containing pooling datas for each pooled object.
		/// PooledObjects should be set in the inspector but they can be added later if needed.
		/// </summary>
		/// <param name="pooledObject">Pooled object to create a pool from.</param>
		public static void GenerateNewPool(PooledObject pooledObject)
		{
			UnityEngine.Assertions.Assert.IsNotNull(pooledObject.GameObject, $"Trying to generate pool with Id {pooledObject.Id} but gameObject reference is null.");
			UnityEngine.Assertions.Assert.IsTrue(pooledObject.Quantity > 0, $"Trying to generate pool with Id {pooledObject.Id} but count is 0 or less ({pooledObject.Quantity}).");

			if (s_poolsByGameObject.ContainsKey(pooledObject.GameObject.GetInstanceID()))
			{
				Instance.LogWarning("Trying to create a pool of an object that has already been pooled.");
				return;
			}

			Queue<GameObject> newPool = new Queue<GameObject>(pooledObject.Quantity);
            Transform container = new GameObject($"{pooledObject.Id} Pool").transform;
            container.SetParent(Instance.transform);

			for (int i = pooledObject.Quantity; i >= 0; --i)
			{
				GameObject newObject = Instantiate(pooledObject.GameObject, container);
				newObject.gameObject.SetActive(false);
				newPool.Enqueue(newObject);
			}

			s_poolsByGameObject.Add(pooledObject.GameObject.GetInstanceID(), newPool);
			s_poolsById.Add(pooledObject.Id, newPool);
		}

		/// <summary>Creates pools using the pooled objects setup beforehand.</summary>
		private void Initialize()
		{
			for (int pooledObjectIndex = _pooledObjects.Length - 1; pooledObjectIndex >= 0; --pooledObjectIndex)
				GenerateNewPool(_pooledObjects[pooledObjectIndex]);

			Log($"Initialized pool with {_pooledObjects.Length} objects.", gameObject);
		}

		protected override void Awake()
		{
			base.Awake();

			if (!IsValid)
				return;

			Initialize();
		}

        [ContextMenu("Sort Alphabetical")]
        private void SortPooledObjectsAlphabetical()
        {
            System.Array.Sort(_pooledObjects, delegate (PooledObject a, PooledObject b) { return a.Id.CompareTo(b.Id); });
        }
	}
}
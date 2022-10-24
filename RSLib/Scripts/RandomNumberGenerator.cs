namespace RSLib
{
    using System.Collections.Generic;
    using System.Linq;

    public class RandomNumberGenerator
    {
        public RandomNumberGenerator()
        {
        }

        public RandomNumberGenerator(int seed)
        {
            _seed = seed;
        }

        private readonly int _seed;
        private readonly Dictionary<string, System.Random> _randomsLibrary = new Dictionary<string, System.Random>();
        
        /// <summary>
        /// Retrieves the Random instance to use for a given calling object, based on its type.
        /// If no Random is found, a new instance is created on the fly and added to the randoms library.
        /// </summary>
        /// <param name="caller">Calling object, which type will be used.</param>
        /// <returns>Caller related random.</returns>
        private System.Random GetRandom(object caller)
        {
            string callerId = caller.GetType().Name;
            
            if (!_randomsLibrary.TryGetValue(callerId, out System.Random random))
            {
                random = _seed != 0 ? new System.Random(_seed) : new System.Random();
                _randomsLibrary.Add(callerId, random);
            }

            return random;
        }

        /// <summary>
        /// Initializes the seed for a given calling object, based on its type.
        /// </summary>
        /// <param name="caller">Calling object, which type will be used.</param>
        /// <param name="seed">Random seed.</param>
        /// <param name="cleanup">Removes the already existing Random for the caller, if it has been found. Else, do nothing.</param>
        public void InitRandomSeed(object caller, int seed, bool cleanup = false)
        {
            string callerId = caller.GetType().Name;

            if (_randomsLibrary.ContainsKey(callerId))
            {
                if (!cleanup)
                    return;

                _randomsLibrary.Remove(callerId);
            }

            _randomsLibrary.Add(callerId, new System.Random(seed));
        }
        
        #region BOOLEAN
        /// <summary>
        /// Computes a random boolean.
        /// </summary>
        /// <param name="caller">Calling object, which type will be used.</param>
        /// <returns>True or false.</returns>
        public bool GetRandomBool(object caller)
        {
            return GetRandomRange(caller, 0f, 1f) > 0.5f;
        }
        #endregion // BOOLEAN

        #region FLOAT RANGE
        /// <summary>
        /// Computes a random float value between a minimum boundary and a maximum boundary.
        /// </summary>
        /// <param name="random">Random to use.</param>
        /// <param name="min">Minimum value (inclusive).</param>
        /// <param name="max">Maximum value (inclusive).</param>
        /// <returns>Random float value.</returns>
        private float GetRandomRange(System.Random random, float min, float max)
        {
            return (float)random.NextDouble() * (max - min) + min;
        }

        /// <summary>
        /// Computes a random float value between a minimum boundary and a maximum boundary.
        /// </summary>
        /// <param name="caller">Calling object, which type will be used.</param>
        /// <param name="min">Minimum value (inclusive).</param>
        /// <param name="max">Maximum value (inclusive).</param>
        /// <returns>Random float value.</returns>
        public float GetRandomRange(object caller, float min, float max)
        {
            return GetRandomRange(GetRandom(caller), min, max);
        }
        
        /// <summary>
        /// Computes a random float value between a minimum boundary and a maximum boundary.
        /// </summary>
        /// <param name="caller">Calling object, which type will be used.</param>
        /// <param name="range">Minimum and maximum values (both inclusive).</param>
        /// <returns>Random float value.</returns>
        public float GetRandomRange(object caller, UnityEngine.Vector2 range)
        {
            return GetRandomRange(caller, range.x, range.y);
        }
        #endregion // FLOAT RANGE

        #region INT RANGE
        /// <summary>
        /// Computes a random int value between a minimum boundary and a maximum boundary.
        /// </summary>
        /// <param name="random">Random to use.</param>
        /// <param name="min">Minimum value (inclusive).</param>
        /// <param name="max">Maximum value (exclusive).</param>
        /// <returns>Random int value.</returns>
        private int GetRandomRange(System.Random random, int min, int max)
        {
            return random.Next(min, max);
        }

        /// <summary>
        /// Computes a random int value between a minimum boundary and a maximum boundary.
        /// </summary>
        /// <param name="caller">Calling object, which type will be used.</param>
        /// <param name="min">Minimum value (inclusive).</param>
        /// <param name="max">Maximum value (exclusive).</param>
        /// <returns>Random int value.</returns>
        public int GetRandomRange(object caller, int min, int max)
        {
            return GetRandomRange(GetRandom(caller), min, max);
        }
        
        /// <summary>
        /// Computes a random int value between a minimum boundary and a maximum boundary.
        /// </summary>
        /// <param name="caller">Calling object, which type will be used.</param>
        /// <param name="range">Minimum and maximum values (minimum is inclusive, maximum is exclusive).</param>
        /// <returns>Random int value.</returns>
        public int GetRandomRange(object caller, UnityEngine.Vector2Int range)
        {
            return GetRandomRange(caller, range.x, range.y);
        }
        #endregion // INT RANGE

        #region COLLECTIONS
        /// <summary>
        /// Gets a random element from a list.
        /// </summary>
        /// <param name="caller">Calling object, which type will be used.</param>
        /// <param name="list">List to pick a random element in.</param>
        /// <returns>Random list element.</returns>
        private T GetRandomElement<T>(object caller, IReadOnlyList<T> list)
        {
            return list[GetRandomRange(caller, 0, list.Count)];
        }
        
        /// <summary>
        /// Gets a random element from an IEnumerable.
        /// </summary>
        /// <param name="caller">Calling object, which type will be used.</param>
        /// <param name="enumerable">IEnumerable to pick a random element in.</param>
        /// <returns>Random IEnumerable element.</returns>
        public T GetRandomElement<T>(object caller, IEnumerable<T> enumerable)
        {
            List<T> list = enumerable.ToList();
            return GetRandomElement(caller, list);
        }
        
        /// <summary>
        /// Shuffles an IEnumerable.
        /// Result is returned a new IEnumerable object, original is not modified.
        /// </summary>
        /// <param name="caller">Calling object, which type will be used.</param>
        /// <param name="enumerable">IEnumerable to shuffle.</param>
        /// <typeparam name="T">Shuffled collection as a new object.</typeparam>
        /// <returns></returns>
        public IEnumerable<T> Shuffle<T>(object caller, IEnumerable<T> enumerable)
        {
            System.Random random = GetRandom(caller);
            return enumerable.Select(o => o).OrderBy(o => random.Next());
        }
        #endregion // COLLECTIONS
    }
}

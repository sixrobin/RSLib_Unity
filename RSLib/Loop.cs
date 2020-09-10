namespace RSLib.Framework.Collections
{
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	/// Custom generic type structure, similar to a queue but where dequeued items are then reenqueued.
	/// This structure, as it is written, should probably be used for randomly peeked elements.
	/// </summary>
	/// <typeparam name="T"> Type of items stored in the loop. </typeparam>
	public sealed class Loop<T> : IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection<T> where T : System.IComparable
	{
        #region FIELDS

        private List<T> loop = new List<T>();
		private int peeksCount = 0;

        #endregion FIELDS

        #region CONSTRUCTORS

        public Loop()
		{
		}

		public Loop(bool shuffleOnLoopCompleted)
		{
			this.ShuffleOnLoopCompleted = shuffleOnLoopCompleted;
		}

		public Loop(IEnumerable<T> content)
		{
			this.loop = content.ToList();
		}

		public Loop(IEnumerable<T> content, bool shuffleOnLoopCompleted, bool shuffledOnInit)
		{
			this.loop = content.ToList();
			this.ShuffleOnLoopCompleted = shuffleOnLoopCompleted;

			if (shuffledOnInit)
			{
				this.Shuffle();
			}
		}

		#endregion CONSTRUCTORS

		#region EVENTS

		public delegate void LoopPointReachedEventHandler();

		public event LoopPointReachedEventHandler LoopPointReached;

		#endregion EVENTS

		#region PROPERTIES

		public bool ShuffleOnLoopCompleted { get; set; }

		public int Count => this.loop.Count;

		public bool IsReadOnly => true;

		#endregion PROPERTIES

		#region METHODS

		public void Add(T element)
		{
			this.loop.Add(element);
		}

		public void AddRange(IEnumerable<T> collection)
		{
			foreach (T element in collection.ToArray())
			{
				this.loop.Add(element);
			}
		}

		public void Clear()
		{
			this.loop.Clear();
		}

		public bool Contains(T element)
		{
			return this.loop.Contains(element);
		}

		public IEnumerator<T> GetEnumerator()
		{
			return this.loop.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.loop.GetEnumerator();
		}

		/// <summary>
		/// Peeks the next loop item and sends it to the end of the structure.
		/// Also checks if a complete loop has been done, and shuffles if needed.
		/// </summary>
		/// <returns>Next item.</returns>
		public T Next()
		{
			if (this.loop.Count == 0)
			{
				return default;
			}

			this.peeksCount++;

			T item = this.loop[0];
			this.loop.RemoveAt(0);
			this.loop.Add(item);

			if (this.peeksCount == this.Count)
			{
				this.peeksCount = 0;
				if (this.ShuffleOnLoopCompleted)
				{
					this.Shuffle();
				}

				this.LoopPointReached?.Invoke();
			}

			return item;
		}

		public void Remove(T element)
		{
			if (this.Contains(element))
			{
				this.loop.Remove(element);
			}
		}

		public void Replace(T replacedElement, T newElement, bool allOccurences = true)
		{
			for (int elementIndex = 0; elementIndex < this.Count; ++elementIndex)
			{
				if (this.loop[elementIndex].Equals(replacedElement))
				{
					this.loop[elementIndex] = newElement;

					if (!allOccurences)
					{
						return;
					}
				}
			}
		}

		public void Shuffle()
		{
			System.Random rnd = new System.Random();
			int n = this.Count;
			while (n > 1)
			{
				int rndNb = rnd.Next(n--);
				(this.loop[rndNb], this.loop[n]) = (this.loop[n], this.loop[rndNb]);
			}
		}

		public void Sort()
		{
			this.loop.Sort();
		}

		public void Sort(IComparer<T> comparer)
		{
			this.loop.Sort(comparer);
		}

		public void Sort(System.Comparison<T> comparison)
		{
			this.loop.Sort(comparison);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			this.loop.CopyTo(array, arrayIndex);
		}

		bool ICollection<T>.Remove(T element)
		{
			bool result = false;

			if (this.Contains(element))
			{
				result = true;
				this.loop.Remove(element);
			}

			return result;
		}

		public override string ToString()
		{
			string str = "";

			for (int i = 0; i < this.loop.Count; ++i)
			{
				str += this.loop[i].ToString() + (i == this.Count - 1 ? "" : ", ");
			}

			return str;
		}
		
		#endregion METHODS
	}
}
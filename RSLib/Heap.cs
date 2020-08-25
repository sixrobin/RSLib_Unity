namespace RSLib
{
    public class Heap<T> where T : IHeapElement<T>
    {
        #region FIELDS

        private T[] elements;

        #endregion FIELDS

        #region CONSTRUCTORS

        public Heap(int maxSize)
        {
            this.elements = new T[maxSize];
        }

        #endregion CONSTRUCTORS

        #region PROPERTIES

        public int Count { get; private set; }

        #endregion PROPERTIES

        #region METHODS

        /// <summary>Removes the first element in the heap tree.</summary>
        /// <returns>First element.</returns>
        public T RemoveFirst()
        {
            T first = this.elements[0];
            this.Count--;
            this.elements[0] = this.elements[this.Count];
            this.elements[0].HeapIndex = 0;
            this.SortDown(this.elements[0]);

            return first;
        }

        /// <summary>Adds an item to the heap tree and sorts it in.</summary>
        /// <param name="element">Item to add.</param>
        public void Add(T element)
        {
            element.HeapIndex = this.Count;
            this.elements[this.Count] = element;
            this.SortUp(element);
            this.Count++;
        }

        /// <summary>Checks if the heap tree contains the given element.</summary>
        /// <param name="element">Element to look for.</param>
        /// <returns>True if the heap contains the element, else false.</returns>
        public bool Contains(T element)
        {
            return Equals(this.elements[element.HeapIndex], element);
        }

        /// <summary>Swaps two items positions in the heap and their indexes.</summary>
        /// <param name="a">First item.</param>
        /// <param name="b">Second item.</param>
        private void Swap(T a, T b)
        {
            (this.elements[a.HeapIndex], this.elements[b.HeapIndex]) = (this.elements[b.HeapIndex], this.elements[a.HeapIndex]);
            (a.HeapIndex, b.HeapIndex) = (b.HeapIndex, a.HeapIndex);
        }

        /// <summary>Retrieves the correct position of an item by sorting it up the heap tree.</summary>
        /// <param name="element">Item to sort up.</param>
        private void SortUp(T element)
        {
            int parentIndex = (element.HeapIndex - 1) / 2;
            while (true)
            {
                T parentItem = this.elements[parentIndex];
                if (element.CompareTo(parentItem) > 0)
                {
                    this.Swap(element, parentItem);
                }
                else
                {
                    break;
                }

                parentIndex = (element.HeapIndex - 1) / 2;
            }
        }

        /// <summary>Retrieves the correct position of an item by sorting it down the heap tree.</summary>
        /// <param name="element">Item to sort down.</param>
        private void SortDown(T element)
        {
            while (true)
            {
                int leftChildIndex = element.HeapIndex * 2 + 1;
                int rightChildIndex = element.HeapIndex * 2 + 2;

                if (leftChildIndex >= this.Count)
                {
                    return;
                }

                int swapIndex = leftChildIndex;

                if (rightChildIndex < this.Count && this.elements[leftChildIndex].CompareTo(this.elements[rightChildIndex]) < 0)
                {
                    swapIndex = rightChildIndex;
                }

                if (element.CompareTo(this.elements[swapIndex]) >= 0)
                {
                    return;
                }

                this.Swap(element, this.elements[swapIndex]);
            }
        }

        #endregion METHODS
    }

    public interface IHeapElement<T> : System.IComparable<T>
    {
        int HeapIndex { get; set; }
    }
}
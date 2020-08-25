﻿namespace RSLib.Extensions
{
	using System.Collections.Generic;
	using System.Linq;

	public static class ArrayExtensions
	{
		private static System.Random rnd = new System.Random();

		#region CONVERSION

		/// <summary>Converts array to a queue.</summary>
		/// <param name="enqueueFromStart">Starts enqueuing from first element.</param>
		/// <returns>Array as a new queue.</returns>
		public static Queue<T> ToQueue<T>(this T[] arr, bool enqueueFromStart = true)
		{
			Queue<T> queue = new Queue<T>();
			for (int i = enqueueFromStart ? 0 : arr.Length - 1;
				enqueueFromStart ? (i < arr.Length) : (i >= 0);
				i += enqueueFromStart ? 1 : -1)
			{
				queue.Enqueue(arr[i]);
			}

			return queue;
		}

		/// <summary>Converts array to a stack.</summary>
		/// <param name="enqueueFromStart">Start stacking from first element.</param>
		/// <returns>Array as a new stack.</returns>
		public static Stack<T> ToStack<T>(this T[] arr, bool stackFromStart = true)
		{
			Stack<T> stack = new Stack<T>();
			for (int i = stackFromStart ? 0 : arr.Length - 1;
				stackFromStart ? (i < arr.Length) : (i >= 0);
				i += stackFromStart ? 1 : -1)
			{
				stack.Push(arr[i]);
			}

			return stack;
		}

		/// <summary>Writes the array elements in a string.</summary>
		/// <param name="arr">Array to write.</param>
		/// <returns>String with the array elements.</returns>
		public static string ToStringImproved<T>(this T[] arr)
		{
			string str = "";
			for (int i = 0; i < arr.Length; ++i)
			{
				str += arr[i].ToString() + (i == arr.Length - 1 ? "" : ", ");
			}

			return str;
		}

		/// <summary>Writes the array elements in a string using a given splitting char.</summary>
		/// <param name="arr">Array to write.</param>
		/// <param name="split">Splitting char.</param>
		/// <returns>String with the array elements.</returns>
		public static string ToStringImproved<T>(this T[] arr, char split = ',')
		{
			string str = "";
			for (int i = 0; i < arr.Length; ++i)
			{
				str += arr[i].ToString() + (i == arr.Length - 1 ? "" : split.ToString());
			}

			return str;
		}

		/// <summary>Writes the array elements in a string using a given splitting string.</summary>
		/// <param name="arr">Array to write.</param>
		/// <param name="split">Splitting string.</param>
		/// <returns>String with the array elements.</returns>
		public static string ToStringImproved<T>(this T[] arr, string split = ",")
		{
			string str = "";
			for (int i = 0; i < arr.Length; ++i)
			{
				str += arr[i].ToString() + (i == arr.Length - 1 ? "" : string.IsNullOrEmpty(split) ? " / " : split);
			}

			return str;
		}

		#endregion CONVERSION

		#region GENERAL

		/// <summary>Returns any randomly picked element.</summary>
		/// <param name="arr">Array to get any element from.</param>
		/// <returns>Any element.</returns>
		public static T Any<T>(this T[] arr)
		{
			return arr[rnd.Next(arr.Length)];
		}

		/// <summary>
		/// Returns many randomly picked elements of an array.
		/// Returns the same array if quantity is greater than original array length.
		/// Returns an empty array if quantity is minus or equal to 0.
		/// </summary>
		/// <param name="arr">Array to get elements from.</param>
		/// <param name="quantity">Amount of elements to pick.</param>
		/// <returns>New array with picked elements.</returns>
		public static T[] Any<T>(this T[] arr, int quantity)
		{
			if (quantity <= 0)
			{
				return new T[0];
			}

			if (quantity >= arr.Length)
			{
				return arr;
			}

			List<T> list = arr.ToList();
			T[] choice = new T[quantity];
			for (int i = quantity - 1; i >= 0; --i)
			{
				T pick = list.Any();
				choice[i] = pick;
				list.Remove(pick);
			}

			return choice;
		}

		/// <summary>Concatenates array with another.</summary>
		/// <param name="second">Array to concatenate with.</param>
		/// <returns>Concatenated array.</returns>
		public static T[] Concat<T>(this T[] arr, T[] second)
		{
			int firstLength = arr.Length;
			int concatLength = firstLength + second.Length;

			T[] concatenated = new T[concatLength];

			for (int i = 0; i < firstLength; ++i)
			{
				concatenated[i] = arr[i];
			}

			for (int i = firstLength; i < concatLength; ++i)
			{
				concatenated[i] = second[i - firstLength];
			}

			return concatenated;
		}

		/// <summary>Returns the last element.</summary>
		/// <param name="arr">Array to get last element from.</param>
		/// <returns>Last element.</returns>
		public static T Last<T>(this T[] arr)
		{
			return arr[arr.Length - 1];
		}

		/// <summary>Reverses the array.</summary>
		/// <param name="arr">Array to reverse.</param>
		/// <returns>Reversed array.</returns>
		public static void Reverse<T>(this T[] arr)
		{
			System.Array.Reverse(arr);
		}

		/// <summary>Reverses the elements in a new array.</summary>
		/// <param name="arr">Array to reverse.</param>
		/// <returns>Reversed array.</returns>
		public static T[] ReverseIntoNewArray<T>(this T[] arr)
		{
			T[] copy = new T[arr.Length];

			if (arr.Length == 1)
			{
				copy[0] = arr[0];
				return copy;
			}

			if (arr.Length == 2)
			{
				arr.Swap(0, 1);
				return copy;
			}

			System.Array.Copy(arr, copy, arr.Length);
			System.Array.Reverse(copy);

			return copy;
		}

		/// <summary>Shuffles the array.</summary>
		/// <param name="arr">Array to shuffle.</param>
		/// <returns>Shuffled array.</returns>
		public static void Shuffle<T>(this T[] arr)
		{
			int n = arr.Length;
			while (n > 1)
			{
				arr.Swap(rnd.Next(n--), n);
			}
		}

		/// <summary>Shuffles the elements in a new array.</summary>
		/// <param name="arr">Array to shuffle.</param>
		/// <returns>Shuffled array.</returns>
		public static T[] ShuffleIntoNewArray<T>(this T[] arr)
		{
			T[] copy = new T[arr.Length];
			System.Array.Copy(arr, copy, arr.Length);

			int n = copy.Length;
			while (n > 1)
			{
				copy.Swap(rnd.Next(n--), n);
			}

			return copy;
		}

		/// <summary>Swaps the positions of 2 elements.</summary>
		/// <param name="first">Index of first.</param>
		/// <param name="second">Index of second.</param>
		public static void Swap<T>(this T[] arr, int first, int second)
		{
			(arr[first], arr[second]) = (arr[second], arr[first]);
		}

		#endregion GENERAL
	}
}
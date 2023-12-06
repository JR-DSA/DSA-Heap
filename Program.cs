using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Xml.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Heap
{
	public class Heap<T> where T : IComparable 
	{
		private List<T> list = new List<T>();

		/// <summary>
		/// Creates a new heap with the items provided in the <paramref name="initList"/>.
		/// </summary>
		/// <param name="initList">Items to add to the new heap.</param>
		/// <returns>A heap created from the <paramref name="initList"/></returns>
		public static Heap<T> FromList(List<T> initList)
		{
			Heap<T> heap = new Heap<T>();

			heap.list.AddRange(initList);
			heap.MinHeapify();

			return heap;
		}

		public void PrettyPrint(string indent = "", int index = 0)
		{
			if (index == 0)
			{
				Console.WriteLine();
			}

			Console.Write(indent);

			if (index == list.Count - 1)
			{
				Console.Write("\\ ");
				indent += "  ";
			}
			else
			{
				Console.Write("| ");
				indent += "| ";
			}

			Console.WriteLine($":{list[index]}");

			int left = GetLeft(index);
			int right = GetRight(index);

			if (left < list.Count)
			{
				PrettyPrint(indent, left);
			}

			if (right < list.Count)
			{
				PrettyPrint(indent, right);
			}
		}

		/// <summary>
		/// This method returns whether or not a certain <paramref name="index"/> satisfies the min-heap property.
		/// </summary>
		/// <remarks>
		/// Note that the Verify method will only check the children of an item.
		/// </remarks>
		/// <param name="nonRecursive">Whether or not all descendants of the index should be checked as well as the <paramref name="index"/>.</param>
		/// <param name="index">Index to check.</param>
		/// <returns>Whether or not the index (and its descendants depending on whether or not it is <paramref name="nonRecursive"/>) satisfies the min-heap property.</returns>
		public bool Verify(int index = 0, bool nonRecursive = false)
		{
			// If the index is outside of the list, then we just return true
			if (index >= list.Count)
			{
				return true;
			}

			int left = GetLeft(index);
			int right = GetRight(index);

			// If the left or right child is less than the index, then we return false;

			if (left < list.Count && (list[left].CompareTo(list[index]) < 0))
			{
				return false;
			}

			if (right < list.Count && (list[right].CompareTo(list[index]) < 0))
			{
				return false;
			}

			// If both children check out and recursive mode is enabled, then we verify the children themselves. Otherwise, we just return true.

			return nonRecursive || (Verify(left) && Verify(right));
		}

		private int GetLeft(int index)
		{
			return (2 * (index + 1)) - 1;
		}

		private int GetRight(int index)
		{
			return 2 * (index + 1);
		}

		internal int GetParentIndex(int index)
		{
			return ((index + 1) / 2) - 1;
		}

		private void Log(int index, int parent)
		{
			Console.WriteLine($"\tSwapping {list[index]} (i: {index}) with {list[parent]} (i: {parent})");
		}

		private void Fix(int index)
		{
			int parent = GetParentIndex(index);

			// If the parent is larger than the child, then we swap them.

			if (list[index].CompareTo(list[parent]) < 0)
			{
				(list[index], list[parent]) = (list[parent], list[index]);
			}

			// If the parent is not equal to the root, then we check it too.

			if (parent > 0)
			{
				Fix(parent);
			}
		}

		/// <summary>
		/// Maintains the min-heap property.
		/// </summary>
		public void MinHeapify()
		{
			// For whatever reason, it only works when you run it twice.
			for (int j = 0; j < 2; j++)
			{
				// Go through each child from the last to the first and "fix" it.
				for (int i = list.Count - 1; i > 0; i--)
				{
					Fix(i);
				}
			}

		}

		/// <summary>
		/// Appends an item to the end of the heap and restores the min-heap property.
		/// </summary>
		/// <param name="item">Item to append</param>
		public void Insert(T item)
		{
			list.Add(item);

			MinHeapify();
		}

		/// <summary>
		/// Fetches, removes, and returns the minimum of the heap (or the root, if the min-heap property is maintained)
		/// </summary>
		/// <returns>Smallest value in the heap</returns>
		public T ExtractMinimum()
		{
			T minimum = PeekMinimum();

			list = list.GetRange(1, list.Count - 1);

			MinHeapify();

			return minimum;
		}

		/// <summary>
		/// Fetches and returns the minimum of the heap (or the root, if the min-heap property is maintained)
		/// </summary>
		/// <returns>Smallest value in the heap</returns>
		public T PeekMinimum()
		{
			return list[0];
		}

		/// <summary>
		/// Returns whether or not the heap has at least one item.
		/// </summary>
		public bool IsEmpty => list.Count == 0;

		public override string ToString()
		{
			return "\t" + string.Join(", ", list);
		}
	}

	public class TaskHandler
	{
		private Heap<Task> taskHeap = new();

		public class Task : IComparable, IComparable<Task>
		{
			public int priority = 0;
			public required string name;

			public int CompareTo(Task? other)
			{
				return other.priority - priority;
			}

			public int CompareTo(object? obj)
			{
				return CompareTo(obj as Task);
			}

			public override string ToString()
			{
				return $"{name} ({priority})";
			}
		}

		public void AddTask(Task task)
		{
			taskHeap.Insert(task);
		}

		public Task HandleHighestPriorityTask()
		{
			return taskHeap.ExtractMinimum();
		}
	}
	public class Program
	{
		public static void TestHeapBasis()
		{
			List<int> values = new List<int>()
			{
				20, 98, 97, 7, 60, 73, 40, 12, 84, 13, 85, 67, 84, 68, 30, 74, 20, 42, 64, 5, 7, 79, 12, 21, 24, 9, 23, 76, 63, 64, 54, 76, 31, 13, 43, 63, 88, 94, 10, 66, 91, 16, 17, 96, 3, 31, 49, 12, 69, 64, 100, 7, 85, 89, 10, 89, 97, 62, 42, 30, 58, 52, 62, 57, 59, 79, 83, 56, 63, 91, 22, 68, 45, 8, 58, 73, 92, 48, 11, 8, 80, 33, 38, 41, 51, 45, 5, 13, 27, 39, 24, 82, 67, 2, 63, 15, 35, 84, 25, 74
				//12,11,10,9,8,7,6,5,4,3,2,1,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32
			};

			Heap<int> myHeap = Heap<int>.FromList(values);

			Console.WriteLine($"Heap successful: {myHeap.Verify()}");
			myHeap.PrettyPrint();

			Console.WriteLine($"Heap successful: {myHeap.Verify()}");
			myHeap.PrettyPrint();
		}

		public static void Main(string[] args)
		{
			TaskHandler handler = new();

			handler.AddTask(new() { name = "Low priority task" });
			handler.AddTask(new() { name = "High priority task", priority = 1 });
			handler.AddTask(new() { name = "Super High priority task", priority = 2 });

			Console.WriteLine(handler.HandleHighestPriorityTask());
			Console.WriteLine(handler.HandleHighestPriorityTask());
			Console.WriteLine(handler.HandleHighestPriorityTask());
		}
	}
}


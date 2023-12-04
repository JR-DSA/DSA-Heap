using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Xml.Linq;

public class Program
{
	public class Heap
	{
		private List<int> list = new List<int>();

		public static Heap FromList(List<int> initList)
		{
			Heap heap = new Heap();

			heap.list.AddRange(initList);
			heap.MinHeapify4();

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

		public void RebuildHeap()
		{
			Verify();
			MinHeapify4();
			Verify();
		}

		public bool Verify(int index = 0)
		{
			if (index >= list.Count)
			{
				return true;
			}

			int left = GetLeft(index);
			int right = GetRight(index);

			if (left < list.Count && list[left] < list[index])
			{
				return false;
			}

			if (right < list.Count && list[right] < list[index])
			{
				return false;
			}

			return Verify(left) && Verify(right);
		}

		private int GetLeft(int index)
		{
			return 2 * index + 1;
		}

		private int GetRight(int index)
		{
			return 2 * index + 2;
		}

		internal int GetParentIndex(int index)
		{
			return ((index + 1) / 2) - 1;
		}

		//public void MinHeapify(int index = 0)
		//{
		//	int left = GetLeft(index + 1) - 1;
		//	int right = GetRight(index + 1) - 1;
		//	int smallest = index;

		//	if (left < list.Count && list[left] < list[index])
		//	{
		//		smallest = left;
		//	}
		//	if (right < list.Count && list[right] < list[smallest])
		//	{
		//		smallest = right;
		//	}

		//	if (smallest != index)
		//	{
		//		(list[smallest], list[index]) = (list[index], list[smallest]);
		//	}

		//	if (left < list.Count)
		//	{
		//		MinHeapify(left);
		//	}

		//	if (right < list.Count)
		//	{
		//		MinHeapify(right);
		//	}

		//}

		//public void MinHeapify2(int index = 0)
		//{
		//	int left = 2 * (index + 1) - 1;
		//	int right = 2 * (index + 1);
		//	int smallest = index;

		//	Console.WriteLine($"Left Index: {left}; Right Index: {right}; Main Index: {index}; Left: {(left < list.Count ? list[left] : -1)}; Right: {(right < list.Count ? list[right] : -1)}; Main: {list[index]}");

		//	if (left < list.Count && list[left] < list[index])
		//	{
		//		Console.WriteLine($"{list[left]} is less than {list[index]}");
		//		(list[index], list[left]) = (list[left], list[index]);
		//	}
		//	if (right < list.Count && list[right] < list[index])
		//	{
		//		Console.WriteLine($"{list[right]} is less than {list[index]}");
		//		(list[index], list[right]) = (list[right], list[index]);
		//	}

		//	//if (smallest != index)
		//	//{
		//	//	(list[smallest], list[index]) = (list[index], list[smallest]);
		//	//}

		//	if (left < list.Count)
		//	{
		//		MinHeapify2(left);
		//	}

		//	if (right < list.Count)
		//	{
		//		MinHeapify2(right);
		//	}
		//}

		private void Fix(int index)
		{
			int parent = GetParentIndex(index);

			if (list[index] < list[parent])
			{
				(list[index], list[parent]) = (list[parent], list[index]);
			}

			if (parent > 0)
			{
				Fix(parent);
			}
		}

		public void MinHeapify4()
		{
			for (int j = 0; j < 2; j++)
			{
				for (int i = list.Count - 1; i > 0; i--)
				{
					Fix(i);
				}
			}
			
		}

		public void Insert(int item)
		{
			list.Add(item);

			MinHeapify4();
		}

		public int ExtractMinimum()
		{
			int minimum = PeekMinimum();

			list = list.GetRange(1, list.Count - 1);

			MinHeapify4();

			return minimum;
		}

		public int PeekMinimum()
		{
			return list[0];
		}

		public bool IsEmpty => list.Count == 0;

		public override string ToString()
		{
			return "\t" + string.Join(", ", list);
		}
	}

	public static void Main(string[] args)
	{
		List<int> values = new List<int>()
		{
			20, 98, 97, 7, 60, 73, 40, 12, 84, 13, 85, 67, 84, 68, 30, 74, 20, 42, 64, 5, 7, 79, 12, 21, 24, 9, 23, 76, 63, 64, 54, 76, 31, 13, 43, 63, 88, 94, 10, 66, 91, 16, 17, 96, 3, 31, 49, 12, 69, 64, 100, 7, 85, 89, 10, 89, 97, 62, 42, 30, 58, 52, 62, 57, 59, 79, 83, 56, 63, 91, 22, 68, 45, 8, 58, 73, 92, 48, 11, 8, 80, 33, 38, 41, 51, 45, 5, 13, 27, 39, 24, 82, 67, 2, 63, 15, 35, 84, 25, 74
			//12,11,10,9,8,7,6,5,4,3,2,1
		};

		Heap myHeap = Heap.FromList(values);

		myHeap.PrettyPrint();

		myHeap.MinHeapify4();
		Console.WriteLine($"Heap successful: {myHeap.Verify()}");

		//Console.WriteLine($"Heap successful: {myHeap.Verify(0)}");
		//myHeap.RebuildHeap();

		myHeap.PrettyPrint();
	}
}
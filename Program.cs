using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Xml.Linq;

namespace Heap
{
	public class Heap
	{
		private List<int> list = new List<int>();

		public static Heap FromList(List<int> initList)
		{
			Heap heap = new Heap();

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

			if (list[index] < list[parent])
			{
				Log(index, parent);
				(list[index], list[parent]) = (list[parent], list[index]);
			}

			if (parent > 0)
			{
				Fix(parent);
			}
		}

		public void MinHeapify()
		{
			for (int j = 0; j < 2; j++)
			{
				Console.WriteLine($"Iteration {j + 1}");

				for (int i = list.Count - 1; i > 0; i--)
				{
					Fix(i);
				}
			}

		}

		public void Insert(int item)
		{
			list.Add(item);

			MinHeapify();
		}

		public int ExtractMinimum()
		{
			int minimum = PeekMinimum();

			list = list.GetRange(1, list.Count - 1);

			MinHeapify();

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
	public class Program
	{
		public static void Main(string[] args)
		{
			List<int> values = new List<int>()
		{
			20, 98, 97, 7, 60, 73, 40, 12, 84, 13, 85, 67, 84, 68, 30, 74, 20, 42, 64, 5, 7, 79, 12, 21, 24, 9, 23, 76, 63, 64, 54, 76, 31, 13, 43, 63, 88, 94, 10, 66, 91, 16, 17, 96, 3, 31, 49, 12, 69, 64, 100, 7, 85, 89, 10, 89, 97, 62, 42, 30, 58, 52, 62, 57, 59, 79, 83, 56, 63, 91, 22, 68, 45, 8, 58, 73, 92, 48, 11, 8, 80, 33, 38, 41, 51, 45, 5, 13, 27, 39, 24, 82, 67, 2, 63, 15, 35, 84, 25, 74
			//12,11,10,9,8,7,6,5,4,3,2,1,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32
		};

			Heap myHeap = Heap.FromList(values);

			Console.WriteLine($"Heap successful: {myHeap.Verify()}");
			myHeap.PrettyPrint();

			Console.WriteLine($"Heap successful: {myHeap.Verify()}");
			myHeap.PrettyPrint();
		}
	}
}


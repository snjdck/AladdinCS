using System;
using System.Collections.Generic;

namespace Aladdin.PathFinding
{
	//最小二叉堆
	class BinHeap<T> where T : IComparable<T>
	{
		readonly List<T> itemList;

		public BinHeap()
		{
			itemList = new List<T>();
		}

		public void push(T item)
		{
			int index = itemList.Count;
			itemList[index] = item;
			bubbleUp(index);
		}

		public T shift()
		{
			T item = itemList[0];

			if (itemList.Count > 2){
				int lastIndex = itemList.Count - 1;
				itemList[0] = itemList[lastIndex];
				itemList.RemoveAt(lastIndex);
				bubbleDown(0);
			}else{
				itemList.RemoveAt(0);
			}

			return item;
		}

		public void update(T item)
		{
			int index = itemList.IndexOf(item);
			if (index >= 0){
				bubbleDown(index);
				bubbleUp(index);
			}
		}

		void bubbleUp(int fromIndex)
		{
			int childIndex = fromIndex;
			int parentIndex;

			while (childIndex > 0)
			{
				parentIndex = (childIndex - 1) >> 1;
				if (!needSwap(parentIndex, childIndex)){
					break;
				}
				swap(childIndex, parentIndex);
				childIndex = parentIndex;
			}
		}

		void bubbleDown(int fromIndex)
		{
			int maxLength = itemList.Count;

			int parentIndex = fromIndex;
			int childIndex, leftChildIndex, rightChildIndex;

			for (;;)
			{
				leftChildIndex = (parentIndex << 1) + 1;
				if (leftChildIndex >= maxLength){
					break;
				}
				rightChildIndex = leftChildIndex + 1;
				childIndex = (rightChildIndex < maxLength && needSwap(leftChildIndex, rightChildIndex)) ? rightChildIndex : leftChildIndex;
				if (!needSwap(parentIndex, childIndex)){
					break;
				}
				swap(parentIndex, childIndex);
				parentIndex = childIndex;
			}
		}

		bool needSwap(int parentIndex, int childIndex)
		{
			T parent = itemList[parentIndex];
			T child = itemList[childIndex];
			return parent.CompareTo(child) > 0;
		}

		void swap(int index1, int index2)
		{
			T item = itemList[index1];
			itemList[index1] = itemList[index2];
			itemList[index2] = item;
		}

		public void clear()
		{
			itemList.Clear();
		}

		public bool isEmpty()
		{
			return itemList.Count == 0;
		}

		public bool has(T item)
		{
			return itemList.Contains(item);
		}
	}
}

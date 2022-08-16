using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Core.Fonts.Type1.Utils;

namespace Telerik.Windows.Documents.Core.PostScript.Data
{
	class OperandsCollection
	{
		public OperandsCollection()
		{
			this.store = new LinkedList<object>();
		}

		public int Count
		{
			get
			{
				return this.store.Count;
			}
		}

		public object First
		{
			get
			{
				return this.store.First<object>();
			}
		}

		public object Last
		{
			get
			{
				return this.store.Last<object>();
			}
		}

		public object GetElementAt(Origin origin, int index)
		{
			if (origin == Origin.Begin)
			{
				LinkedListNode<object> linkedListNode = this.store.First;
				for (int i = 0; i < index; i++)
				{
					linkedListNode = linkedListNode.Next;
				}
				return linkedListNode.Value;
			}
			LinkedListNode<object> linkedListNode2 = this.store.Last;
			for (int j = 0; j < index; j++)
			{
				linkedListNode2 = linkedListNode2.Previous;
			}
			return linkedListNode2.Value;
		}

		public void AddLast(object obj)
		{
			this.store.AddLast(obj);
		}

		public void AddFirst(object obj)
		{
			this.store.AddFirst(obj);
		}

		public object GetLast()
		{
			object value = this.store.Last.Value;
			this.store.RemoveLast();
			return value;
		}

		public T GetLastAs<T>()
		{
			return (T)((object)this.GetLast());
		}

		public T GetLastUnboxedAs<T>() where T : struct
		{
			object last = this.GetLast();
			T result;
			Helper.Unbox<T>(last, out result);
			return result;
		}

		public int GetLastAsInt()
		{
			int result;
			Helper.UnboxInteger(this.GetLast(), out result);
			return result;
		}

		public double GetLastAsReal()
		{
			double result;
			Helper.UnboxReal(this.GetLast(), out result);
			return result;
		}

		public object GetFirst()
		{
			object value = this.store.First.Value;
			this.store.RemoveFirst();
			return value;
		}

		public T GetFirstAs<T>()
		{
			return (T)((object)this.GetFirst());
		}

		public int GetFirstAsInt()
		{
			int result;
			Helper.UnboxInteger(this.GetFirst(), out result);
			return result;
		}

		public double GetFirstAsReal()
		{
			double result;
			Helper.UnboxReal(this.GetFirst(), out result);
			return result;
		}

		public void Clear()
		{
			this.store.Clear();
		}

		readonly LinkedList<object> store;
	}
}

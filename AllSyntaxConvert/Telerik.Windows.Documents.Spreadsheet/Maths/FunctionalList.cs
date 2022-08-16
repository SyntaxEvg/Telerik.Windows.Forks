using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Telerik.Windows.Documents.Spreadsheet.Maths
{
	sealed class FunctionalList<T> : IEnumerable<T>, IEnumerable
	{
		public FunctionalList(T head, FunctionalList<T> tail)
		{
			this.head = head;
			this.tail = (tail.IsEmpty ? FunctionalList<T>.Empty : tail);
		}

		public FunctionalList(T head)
			: this(head, FunctionalList<T>.Empty)
		{
		}

		public FunctionalList(T firstValue, params T[] values)
		{
			this.head = firstValue;
			if (values.Length > 0)
			{
				FunctionalList<T> functionalList = FunctionalList<T>.Empty;
				for (int i = values.Length - 1; i >= 0; i--)
				{
					functionalList = functionalList.Cons(values[i]);
				}
				this.tail = functionalList;
				return;
			}
			this.tail = FunctionalList<T>.Empty;
		}

		public FunctionalList(IEnumerable<T> source)
		{
			T[] array = source.ToArray<T>();
			int num = array.Length;
			if (num > 0)
			{
				this.head = array[0];
				this.tail = FunctionalList<T>.Empty;
				for (int i = num - 1; i > 0; i--)
				{
					this.tail = this.tail.Cons(array[i]);
				}
				return;
			}
			this.isEmpty = true;
		}

		FunctionalList()
		{
			this.isEmpty = true;
		}

		public T Head
		{
			get
			{
				if (this.isEmpty)
				{
					throw new InvalidOperationException("The list is empty.");
				}
				return this.head;
			}
		}

		public bool IsEmpty
		{
			get
			{
				return this.isEmpty;
			}
		}

		public FunctionalList<T> Tail
		{
			get
			{
				if (this.isEmpty)
				{
					throw new InvalidOperationException("The list is empty.");
				}
				return this.tail;
			}
		}

		public static FunctionalList<T> Cons(T element, FunctionalList<T> list)
		{
			if (!list.IsEmpty)
			{
				return new FunctionalList<T>(element, list);
			}
			return new FunctionalList<T>(element);
		}

		public FunctionalList<T> Append(FunctionalList<T> other)
		{
			return this.Append(other);
		}

		public FunctionalList<T> Cons(T element)
		{
			return FunctionalList<T>.Cons(element, this);
		}

		public IEnumerator<T> GetEnumerator()
		{
			for (FunctionalList<T> element = this; element != FunctionalList<T>.Empty; element = element.Tail)
			{
				yield return element.Head;
			}
			yield break;
		}

		public FunctionalList<T> Remove(T element)
		{
			return Functional.Remove<T>(this, element);
		}

		public override string ToString()
		{
			string str = "[";
			bool flag = this.Count<T>() > 10;
			IEnumerable<string> enumerable;
			if (!flag)
			{
				enumerable = this.Map((T x) => x.ToString());
			}
			else
			{
				enumerable = this.Map((T x) => x.ToString()).Take(10);
			}
			IEnumerable<string> list = enumerable;
			if (!this.IsEmpty)
			{
				str += list.Fold((string r, string x) => r + ", " + x, string.Empty);
			}
			return str + (flag ? "...]" : "]");
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public static readonly FunctionalList<T> Empty = new FunctionalList<T>();

		readonly T head;

		readonly bool isEmpty;

		readonly FunctionalList<T> tail;
	}
}

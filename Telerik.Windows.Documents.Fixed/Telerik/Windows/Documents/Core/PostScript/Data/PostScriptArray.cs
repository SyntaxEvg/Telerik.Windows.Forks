using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Core.Fonts.Type1.Utils;

namespace Telerik.Windows.Documents.Core.PostScript.Data
{
	class PostScriptArray : PostScriptObject, IList<object>, ICollection<object>, IEnumerable<object>, IEnumerable
	{
		public static PostScriptArray MatrixIdentity
		{
			get
			{
				return new PostScriptArray(new object[] { 1, 0, 0, 1, 0, 0 });
			}
		}

		public PostScriptArray()
		{
			this.store = new List<object>();
		}

		public PostScriptArray(int capacity)
		{
			this.store = new List<object>(capacity);
			for (int i = 0; i < capacity; i++)
			{
				this.store.Add(null);
			}
		}

		public PostScriptArray(object[] initialValue)
		{
			this.store = new List<object>(initialValue);
		}

		public object this[int index]
		{
			get
			{
				return this.store[index];
			}
			set
			{
				this.store[index] = value;
			}
		}

		public Matrix ToMatrix()
		{
			double m;
			Helper.UnboxReal(this.store[0], out m);
			double m2;
			Helper.UnboxReal(this.store[1], out m2);
			double m3;
			Helper.UnboxReal(this.store[2], out m3);
			double m4;
			Helper.UnboxReal(this.store[3], out m4);
			double offsetX;
			Helper.UnboxReal(this.store[4], out offsetX);
			double offsetY;
			Helper.UnboxReal(this.store[5], out offsetY);
			return new Matrix(m, m2, m3, m4, offsetX, offsetY);
		}

		public Rect ToRect()
		{
			double x;
			Helper.UnboxReal(this.store[0], out x);
			double y;
			Helper.UnboxReal(this.store[1], out y);
			double x2;
			Helper.UnboxReal(this.store[2], out x2);
			double y2;
			Helper.UnboxReal(this.store[3], out y2);
			return new Rect(new Point(x, y), new Point(x2, y2));
		}

		public int IndexOf(object item)
		{
			return this.store.IndexOf(item);
		}

		public void Insert(int index, object item)
		{
			this.store.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			this.store.RemoveAt(index);
		}

		public void Add(object item)
		{
			this.store.Add(item);
		}

		public void Clear()
		{
			this.store.Clear();
		}

		public bool Contains(object item)
		{
			return this.store.Contains(item);
		}

		public void CopyTo(object[] array, int arrayIndex)
		{
			this.store.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get
			{
				return this.store.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public bool Remove(object item)
		{
			return this.store.Remove(item);
		}

		public IEnumerator<object> GetEnumerator()
		{
			return this.store.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.store.GetEnumerator();
		}

		public void Load(object[] content)
		{
			foreach (object item in content)
			{
				this.store.Add(item);
			}
		}

		public T GetElementAs<T>(int index)
		{
			return (T)((object)this.store[index]);
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (object value in this.store)
			{
				stringBuilder.Append(" ");
				stringBuilder.Append(value);
			}
			stringBuilder.Remove(0, 1);
			stringBuilder.Append("]");
			stringBuilder.Insert(0, "[");
			return stringBuilder.ToString();
		}

		readonly List<object> store;
	}
}

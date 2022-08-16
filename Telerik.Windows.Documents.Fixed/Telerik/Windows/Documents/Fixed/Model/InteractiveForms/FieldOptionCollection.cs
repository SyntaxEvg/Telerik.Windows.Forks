using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.InteractiveForms
{
	public class FieldOptionCollection<T> : IEnumerable<T>, IEnumerable where T : FieldOptionBase
	{
		internal FieldOptionCollection()
		{
			this.options = new List<T>();
		}

		public T this[int index]
		{
			get
			{
				return this.options[index];
			}
			set
			{
				Guard.ThrowExceptionIfNull<T>(value, "value");
				this.OnBeforeRemoveOption(this.options[index]);
				this.OnBeforeAddOption(value);
				this.options[index] = value;
			}
		}

		public int Count
		{
			get
			{
				return this.options.Count;
			}
		}

		public void Add(T option)
		{
			this.OnBeforeAddOption(option);
			this.options.Add(option);
		}

		public void RemoveAt(int index)
		{
			this.OnBeforeRemoveOption(this.options[index]);
			this.options.RemoveAt(index);
		}

		public void Clear()
		{
			this.OnBeforeRemoveOptions(this.options);
			this.options.Clear();
		}

		public IEnumerator<T> GetEnumerator()
		{
			return this.options.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.options.GetEnumerator();
		}

		internal bool TryGetOptionIndex(T option, out int index)
		{
			if (option == null)
			{
				index = -1;
			}
			else
			{
				index = this.options.IndexOf(option);
			}
			return index > -1;
		}

		internal bool TryGetValueIndex(string value, out int index)
		{
			index = 0;
			foreach (T t in this)
			{
				if (t.Value.Equals(value))
				{
					return true;
				}
				index++;
			}
			return false;
		}

		internal virtual void OnBeforeAddOption(T option)
		{
			Guard.ThrowExceptionIfTrue(option.IsAdded, "option.IsAdded");
			option.IsAdded = true;
		}

		internal virtual void OnBeforeRemoveOptions(IEnumerable<T> options)
		{
			foreach (T t in options)
			{
				t.IsAdded = false;
			}
		}

		void OnBeforeRemoveOption(T option)
		{
			this.OnBeforeRemoveOptions(Enumerable.Repeat<T>(option, 1));
		}

		readonly List<T> options;
	}
}

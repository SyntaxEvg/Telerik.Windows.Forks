using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Attributes
{
	class ClassNamesCollection
	{
		public ClassNamesCollection()
		{
			this.linkedList = new LinkedList<string>();
		}

		public ClassNamesCollection(IEnumerable<string> data)
		{
			this.linkedList = new LinkedList<string>(data);
		}

		public int Count
		{
			get
			{
				return this.linkedList.Count;
			}
		}

		public string PeekFirst()
		{
			return this.linkedList.First.Value;
		}

		public string PeekLast()
		{
			return this.linkedList.Last.Value;
		}

		public void AddFirst(string name)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			this.linkedList.AddFirst(name);
		}

		public void AddLast(string name)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			this.linkedList.AddLast(name);
		}

		public string GetFirst()
		{
			string value = this.linkedList.First.Value;
			this.linkedList.RemoveFirst();
			return value;
		}

		public string GetLast()
		{
			string value = this.linkedList.Last.Value;
			this.linkedList.RemoveLast();
			return value;
		}

		public void AddRange(IEnumerable<string> names)
		{
			foreach (string name in names)
			{
				this.AddLast(name);
			}
		}

		public IEnumerable<string> ToEnumerable()
		{
			return this.linkedList;
		}

		public void Clear()
		{
			this.linkedList.Clear();
		}

		readonly LinkedList<string> linkedList;
	}
}

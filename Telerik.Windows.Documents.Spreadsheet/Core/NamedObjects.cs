using System;
using System.Collections;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Core
{
	public class NamedObjects<T> : IEnumerable<T>, IEnumerable where T : INamedObject
	{
		bool AreChangeEventsSuspended
		{
			get
			{
				return this.suspendChangeEventsCount > 0;
			}
		}

		public int Count
		{
			get
			{
				return this.nameToItem.Count;
			}
		}

		public T this[int index]
		{
			get
			{
				return this.nameToItem.GetAt(index);
			}
		}

		public NamedObjects()
		{
			this.nameToItem = new QueueDictionary<string, T>();
		}

		void SuspendChangeEvents()
		{
			this.suspendChangeEventsCount++;
		}

		void ResumeChangeEvents()
		{
			if (this.suspendChangeEventsCount == 0)
			{
				throw new InvalidOperationException("There is no active update to end.");
			}
			this.suspendChangeEventsCount--;
			if (this.suspendChangeEventsCount == 0)
			{
				this.OnChanged();
			}
		}

		public void Add(T item)
		{
			Guard.ThrowExceptionIfNull<T>(item, "item");
			this.nameToItem.Add(item.Name, item);
			this.OnChanged();
		}

		public void Replace(string itemName, T item)
		{
			Guard.ThrowExceptionIfNull<T>(item, "item");
			Guard.ThrowExceptionIfNullOrEmpty(itemName, "itemName");
			using (new UpdateScope(new Action(this.SuspendChangeEvents), new Action(this.ResumeChangeEvents)))
			{
				this.Remove(itemName);
				this.Add(item);
			}
			this.OnItemReplaced(itemName, item.Name);
			this.OnChanged();
		}

		public void AddRange(IEnumerable<T> items)
		{
			using (new UpdateScope(new Action(this.SuspendChangeEvents), new Action(this.ResumeChangeEvents)))
			{
				foreach (T item in items)
				{
					this.Add(item);
				}
			}
			this.OnChanged();
		}

		public bool TryGetByName(string name, out T result)
		{
			return this.nameToItem.TryGetValue(name, out result);
		}

		public T GetByName(string name)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			return this.nameToItem[name];
		}

		public bool Contains(string name)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			return this.nameToItem.ContainsKey(name);
		}

		public bool Contains(T item)
		{
			Guard.ThrowExceptionIfNull<T>(item, "item");
			T second;
			return this.nameToItem.TryGetValue(item.Name, out second) && TelerikHelper.EqualsOfT<T>(item, second);
		}

		public void Remove(string name)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			this.nameToItem.Remove(name);
			this.OnChanged();
		}

		public void Clear()
		{
			this.nameToItem.Clear();
			this.OnChanged();
		}

		public IEnumerator<T> GetEnumerator()
		{
			foreach (KeyValuePair<string, T> pair in this.nameToItem)
			{
				KeyValuePair<string, T> keyValuePair = pair;
				yield return keyValuePair.Value;
			}
			yield break;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}

		void OnItemReplaced(string replacedItemName, string newItemName)
		{
			this.OnItemReplaced(new NamedObjectsItemReplaceEventArgs(replacedItemName, newItemName));
		}

		public event EventHandler<NamedObjectsItemReplaceEventArgs> ItemReplaced;

		protected virtual void OnItemReplaced(NamedObjectsItemReplaceEventArgs args)
		{
			if (this.ItemReplaced != null)
			{
				this.ItemReplaced(this, args);
			}
		}

		public event EventHandler Changed;

		protected virtual void OnChanged()
		{
			if (this.AreChangeEventsSuspended)
			{
				return;
			}
			if (this.Changed != null)
			{
				this.Changed(this, EventArgs.Empty);
			}
		}

		readonly QueueDictionary<string, T> nameToItem;

		int suspendChangeEventsCount;
	}
}

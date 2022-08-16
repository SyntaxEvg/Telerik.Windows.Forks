using System;
using System.Collections.Generic;
using CsQuery.Implementation;

namespace CsQuery.Engine
{
	class DomIndexRanged : IDomIndex, IDomIndexSimple, IDomIndexRanged, IDomIndexQueue
	{
		public DomIndexRanged()
		{
			this.QueueChanges = true;
		}

		Queue<IndexOperation> _PendingIndexChanges
		{
			get
			{
				return this.__PendingIndexChanges;
			}
			set
			{
				this.__PendingIndexChanges = value;
			}
		}

		Queue<IndexOperation> PendingIndexChanges
		{
			get
			{
				if (this._PendingIndexChanges == null)
				{
					this._PendingIndexChanges = new Queue<IndexOperation>();
				}
				return this._PendingIndexChanges;
			}
		}

		bool IndexNeedsUpdate
		{
			get
			{
				return (this._PendingIndexChanges != null) & (this.PendingIndexChanges.Count > 0);
			}
		}

		internal RangeSortedDictionary<ushort, IDomObject> SelectorXref
		{
			get
			{
				if (this._SelectorXref == null)
				{
					this._SelectorXref = new RangeSortedDictionary<ushort, IDomObject>(PathKeyComparer.Comparer, PathKeyComparer.Comparer, ushort.MaxValue);
				}
				return this._SelectorXref;
			}
		}

		public bool QueueChanges { get; set; }

		public void AddToIndex(IDomIndexedNode element)
		{
			ushort[] nodePath = element.IndexReference.NodePath;
			this.QueueAddToIndex(this.RangePath(nodePath), element);
			foreach (ushort[] key in element.IndexKeys())
			{
				this.QueueAddToIndex(this.RangePath(key, nodePath), element);
			}
			if (element.HasChildren)
			{
				foreach (IDomElement domElement in ((IDomContainer)element).ChildElements)
				{
					DomElement element2 = (DomElement)domElement;
					this.AddToIndex(element2);
				}
			}
		}

		public void AddToIndex(ushort[] key, IDomIndexedNode element)
		{
			this.QueueAddToIndex(this.RangePath(key, element), element);
		}

		public void RemoveFromIndex(ushort[] key, IDomIndexedNode element)
		{
			this.QueueRemoveFromIndex(this.RangePath(key, element));
		}

		public void RemoveFromIndex(IDomIndexedNode element)
		{
			ushort[] nodePath = element.IndexReference.NodePath;
			this.QueueRemoveFromIndex(this.RangePath(null, nodePath));
			if (element.HasChildren)
			{
				foreach (IDomElement domElement in ((IDomContainer)element).ChildElements)
				{
					if (domElement.IsIndexed)
					{
						this.RemoveFromIndex(domElement);
					}
				}
			}
			foreach (ushort[] key in element.IndexKeys())
			{
				this.QueueRemoveFromIndex(this.RangePath(key, nodePath));
			}
		}

		public IEnumerable<IDomObject> QueryIndex(ushort[] subKey, int depth, bool includeDescendants)
		{
			this.ProcessQueue();
			return this.SelectorXref.GetRange(subKey, depth, includeDescendants);
		}

		public IEnumerable<IDomObject> QueryIndex(ushort[] key)
		{
			this.ProcessQueue();
			ushort[] array = new ushort[key.Length + 1];
			Buffer.BlockCopy(key, 0, array, 0, key.Length << 1);
			array[key.Length] = ushort.MaxValue;
			return this.SelectorXref.GetRange(array);
		}

		public void Clear()
		{
			this.SelectorXref.Clear();
			this._PendingIndexChanges = null;
		}

		public int Count
		{
			get
			{
				this.ProcessQueue();
				return this.SelectorXref.Count;
			}
		}

		void QueueAddToIndex(ushort[] key, IDomIndexedNode element)
		{
			if (this.QueueChanges)
			{
				this.PendingIndexChanges.Enqueue(new IndexOperation
				{
					Key = key,
					Value = element.IndexReference,
					IndexOperationType = IndexOperationType.Add
				});
				return;
			}
			this.SelectorXref.Add(key, element.IndexReference);
		}

		void QueueRemoveFromIndex(ushort[] key)
		{
			if (this.QueueChanges)
			{
				this.PendingIndexChanges.Enqueue(new IndexOperation
				{
					Key = key,
					IndexOperationType = IndexOperationType.Remove
				});
				return;
			}
			this.SelectorXref.Remove(key);
		}

		void ProcessQueue()
		{
			if (this._PendingIndexChanges == null)
			{
				return;
			}
			while (this.PendingIndexChanges.Count > 0)
			{
				IndexOperation indexOperation = this.PendingIndexChanges.Dequeue();
				switch (indexOperation.IndexOperationType)
				{
				case IndexOperationType.Add:
					this.SelectorXref.Add(indexOperation.Key, indexOperation.Value);
					break;
				case IndexOperationType.Remove:
					this.SelectorXref.Remove(indexOperation.Key);
					break;
				}
			}
		}

		ushort[] RangePath(ushort[] key, ushort[] path)
		{
			int num = ((key == null) ? 0 : key.Length);
			ushort[] array = new ushort[num + path.Length + 1];
			int i;
			for (i = 0; i < num; i++)
			{
				array[i] = key[i];
			}
			array[i++] = ushort.MaxValue;
			int j = 0;
			while (j < path.Length)
			{
				array[i++] = path[j++];
			}
			return array;
		}

		ushort[] RangePath(ushort[] key, IDomIndexedNode element)
		{
			ushort[] nodePath = element.IndexReference.NodePath;
			return this.RangePath(key, nodePath);
		}

		ushort[] RangePath(ushort[] path)
		{
			ushort[] array = new ushort[path.Length + 1];
			array[0] = ushort.MaxValue;
			int i = 0;
			int num = 1;
			while (i < path.Length)
			{
				array[num++] = path[i++];
			}
			return array;
		}

		RangeSortedDictionary<ushort, IDomObject> _SelectorXref;

		Queue<IndexOperation> __PendingIndexChanges;
	}
}

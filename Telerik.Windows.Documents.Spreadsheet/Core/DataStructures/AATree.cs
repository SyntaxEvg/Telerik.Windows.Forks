using System;
using System.Collections;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Core.DataStructures
{
	class AATree<T> : IEnumerable<T>, IEnumerable where T : IComparable<T>
	{
		public AATree()
		{
			this.root = (this.sentinel = new AATree<T>.AATreeNode());
			this.deleted = null;
		}

		void Skew(ref AATree<T>.AATreeNode node)
		{
			if (node.level == node.left.level)
			{
				AATree<T>.AATreeNode left = node.left;
				node.left = left.right;
				left.right = node;
				node = left;
			}
		}

		void Split(ref AATree<T>.AATreeNode node)
		{
			if (node.right.right.level == node.level)
			{
				AATree<T>.AATreeNode right = node.right;
				node.right = right.left;
				right.left = node;
				node = right;
				node.level++;
			}
		}

		bool Insert(ref AATree<T>.AATreeNode node, T value)
		{
			if (node == this.sentinel)
			{
				node = new AATree<T>.AATreeNode(value, this.sentinel);
				return true;
			}
			int num = value.CompareTo(node.value);
			if (num < 0)
			{
				if (!this.Insert(ref node.left, value))
				{
					return false;
				}
			}
			else
			{
				if (num <= 0)
				{
					return false;
				}
				if (!this.Insert(ref node.right, value))
				{
					return false;
				}
			}
			this.Skew(ref node);
			this.Split(ref node);
			this.InvalidateEnumerableCache();
			return true;
		}

		bool Delete(ref AATree<T>.AATreeNode node, T value)
		{
			if (node == this.sentinel)
			{
				return this.deleted != null;
			}
			int num = value.CompareTo(node.value);
			if (num < 0)
			{
				if (!this.Delete(ref node.left, value))
				{
					return false;
				}
			}
			else
			{
				if (num == 0)
				{
					this.deleted = node;
				}
				if (!this.Delete(ref node.right, value))
				{
					return false;
				}
			}
			if (this.deleted != null)
			{
				this.deleted.value = node.value;
				this.deleted = null;
				node = node.right;
			}
			else if (node.left.level < node.level - 1 || node.right.level < node.level - 1)
			{
				node.level--;
				if (node.right.level > node.level)
				{
					node.right.level = node.level;
				}
				this.Skew(ref node);
				this.Skew(ref node.right);
				this.Skew(ref node.right.right);
				this.Split(ref node);
				this.Split(ref node.right);
			}
			this.InvalidateEnumerableCache();
			return true;
		}

		AATree<T>.AATreeNode Search(AATree<T>.AATreeNode node, T value)
		{
			if (node == this.sentinel)
			{
				return null;
			}
			int num = value.CompareTo(node.value);
			if (num < 0)
			{
				return this.Search(node.left, value);
			}
			if (num > 0)
			{
				return this.Search(node.right, value);
			}
			return node;
		}

		void BuildEnumerateCache(AATree<T>.AATreeNode node, ref Queue<T> result)
		{
			if (node != this.sentinel)
			{
				this.BuildEnumerateCache(node.left, ref result);
				result.Enqueue(node.value);
				this.BuildEnumerateCache(node.right, ref result);
			}
		}

		protected void InvalidateEnumerableCache()
		{
			this.isEnumerableCacheValid = false;
		}

		public bool Add(T value)
		{
			return this.Insert(ref this.root, value);
		}

		public bool Delete(T value)
		{
			return this.Delete(ref this.root, value);
		}

		public bool Contains(T value)
		{
			return this.Search(this.root, value) != null;
		}

		public IEnumerator<T> GetEnumerator()
		{
			if (!this.isEnumerableCacheValid)
			{
				Queue<T> queue = new Queue<T>();
				this.BuildEnumerateCache(this.root, ref queue);
				this.enumerableCache = queue;
				this.isEnumerableCacheValid = true;
			}
			return this.enumerableCache.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		protected AATree<T>.AATreeNode root;

		protected readonly AATree<T>.AATreeNode sentinel;

		AATree<T>.AATreeNode deleted;

		bool isEnumerableCacheValid;

		IEnumerable<T> enumerableCache;

		internal class AATreeNode
		{
			internal AATreeNode()
			{
				this.level = 0;
				this.left = this;
				this.right = this;
			}

			internal AATreeNode(T value, AATree<T>.AATreeNode sentinel)
			{
				this.level = 1;
				this.left = sentinel;
				this.right = sentinel;
				this.value = value;
			}

			internal int level;

			internal AATree<T>.AATreeNode left;

			internal AATree<T>.AATreeNode right;

			internal T value;
		}
	}
}

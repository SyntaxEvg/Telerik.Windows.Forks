using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Core
{
	class ObjectPool
	{
		public ObjectPool()
		{
			this.typeToPool = new Dictionary<Type, Stack<object>>();
			this.createInstanceFunc = (Type type) => Activator.CreateInstance(type);
		}

		public ObjectPool(Func<Type, object> createInstanceFunc)
		{
			this.typeToPool = new Dictionary<Type, Stack<object>>();
			this.createInstanceFunc = createInstanceFunc;
		}

		public T CreateObject<T>()
		{
			T result;
			lock (ObjectPool.lockObject)
			{
				Type typeFromHandle = typeof(T);
				Stack<object> stack = null;
				if (this.typeToPool.TryGetValue(typeFromHandle, out stack) && stack.Count > 0)
				{
					result = (T)((object)stack.Pop());
				}
				else
				{
					result = (T)((object)this.createInstanceFunc(typeFromHandle));
				}
			}
			return result;
		}

		public void ReleaseObject(object obj)
		{
			lock (ObjectPool.lockObject)
			{
				Type type = obj.GetType();
				Stack<object> stack;
				if (!this.typeToPool.TryGetValue(type, out stack))
				{
					stack = new Stack<object>();
					this.typeToPool[type] = stack;
				}
				if (!false)
				{
					stack.Push(obj);
				}
			}
		}

		static readonly object lockObject = new object();

		readonly Dictionary<Type, Stack<object>> typeToPool;

		readonly Func<Type, object> createInstanceFunc;
	}
}

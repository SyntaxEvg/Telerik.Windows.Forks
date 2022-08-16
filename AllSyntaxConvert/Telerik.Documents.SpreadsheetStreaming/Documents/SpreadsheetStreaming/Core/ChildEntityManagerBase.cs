using System;
using System.Collections.Generic;

namespace Telerik.Documents.SpreadsheetStreaming.Core
{
	abstract class ChildEntityManagerBase<T> where T : IChildEntry
	{
		public ChildEntityManagerBase()
		{
			this.typeToRegisterIndex = new Dictionary<Type, Tuple<int, Type>>();
			this.typeToInstance = new Dictionary<Type, T>();
			this.typeToFactory = new Dictionary<Type, Func<T>>();
			this.indexToInstance = new Dictionary<int, T>();
			this.currentChildIndex = -1;
		}

		public void EnsureCanGetElement<TResult>() where TResult : T
		{
			Type typeFromHandle = typeof(TResult);
			Tuple<int, Type> tuple = this.typeToRegisterIndex[typeFromHandle];
			int item = tuple.Item1;
			Type item2 = tuple.Item2;
			this.ValidateExportOrder(item2, item);
			this.EnsureEndUsingPreviousChildInstance(item);
			this.ValidatePreviousInstanceUsage(typeFromHandle);
		}

		public abstract TResult GetRegisteredChild<TResult>() where TResult : T;

		public bool TryGetLastChild<TResult>(out TResult child) where TResult : T
		{
			return this.TryGetChildAtIndex<TResult>(this.currentChildIndex, out child);
		}

		public void RegisterChild<TResult>(Func<T> factory) where TResult : T
		{
			this.RegisterChild<TResult>(factory, null);
		}

		public void RegisterChild<TResult>(Func<T> factory, Type dependOnType) where TResult : T
		{
			int count = this.typeToRegisterIndex.Count;
			Tuple<int, Type> value = new Tuple<int, Type>(count, dependOnType);
			this.typeToRegisterIndex.Add(typeof(TResult), value);
			this.typeToFactory.Add(typeof(TResult), factory);
		}

		public int GetRegisteredChildIndex(Type type)
		{
			if (type == null || !this.typeToRegisterIndex.ContainsKey(type))
			{
				return -1;
			}
			return this.typeToRegisterIndex[type].Item1;
		}

		protected TResult GetRegisteredChild<TResult>(bool reuseChild) where TResult : T
		{
			Type typeFromHandle = typeof(TResult);
			Tuple<int, Type> tuple = this.typeToRegisterIndex[typeFromHandle];
			int item = tuple.Item1;
			Type item2 = tuple.Item2;
			this.ValidateExportOrder(item2, item);
			this.EnsureEndUsingPreviousChildInstance(item);
			this.currentChildIndex = item;
			TResult tresult;
			if (reuseChild && this.indexToInstance.ContainsKey(this.currentChildIndex))
			{
				tresult = (TResult)((object)this.indexToInstance[this.currentChildIndex]);
			}
			else
			{
				tresult = this.CreateChild<TResult>();
				this.indexToInstance[this.currentChildIndex] = (T)((object)tresult);
			}
			return tresult;
		}

		protected abstract void EnsureEndUsingChild(T previousInstance);

		TResult CreateChild<TResult>() where TResult : T
		{
			Type typeFromHandle = typeof(TResult);
			this.ValidatePreviousInstanceUsage(typeFromHandle);
			TResult tresult = (TResult)((object)this.typeToFactory[typeFromHandle]());
			this.typeToInstance[typeFromHandle] = (T)((object)tresult);
			return tresult;
		}

		void ValidatePreviousInstanceUsage(Type type)
		{
			if (this.typeToInstance.ContainsKey(type))
			{
				T t = this.typeToInstance[type];
				if (t.IsUsageBegan && !t.IsUsageCompleted)
				{
					throw new InvalidOperationException("Can not start using new instance before stop using the previous.");
				}
			}
		}

		void EnsureEndUsingPreviousChildInstance(int childIndex)
		{
			if (this.currentChildIndex >= 0 && this.currentChildIndex != childIndex)
			{
				T previousInstance = this.indexToInstance[this.currentChildIndex];
				this.EnsureEndUsingChild(previousInstance);
			}
		}

		void ValidateExportOrder(Type dependOnType, int childIndex)
		{
			bool flag = false;
			if (dependOnType != null && this.typeToRegisterIndex.ContainsKey(dependOnType))
			{
				Tuple<int, Type> tuple = this.typeToRegisterIndex[dependOnType];
				flag = !this.indexToInstance.ContainsKey(tuple.Item1);
			}
			if (childIndex < this.currentChildIndex || flag)
			{
				throw new InvalidOperationException("Invalid export order.");
			}
		}

		bool TryGetChildAtIndex<TResult>(int index, out TResult child) where TResult : T
		{
			child = default(TResult);
			if (this.indexToInstance.ContainsKey(index))
			{
				child = (TResult)((object)this.indexToInstance[index]);
				return true;
			}
			return false;
		}

		readonly Dictionary<Type, Tuple<int, Type>> typeToRegisterIndex;

		readonly Dictionary<Type, T> typeToInstance;

		readonly Dictionary<Type, Func<T>> typeToFactory;

		readonly Dictionary<int, T> indexToInstance;

		int currentChildIndex;
	}
}

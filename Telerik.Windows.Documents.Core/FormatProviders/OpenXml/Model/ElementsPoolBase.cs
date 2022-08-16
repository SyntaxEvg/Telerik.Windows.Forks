using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model
{
	abstract class ElementsPoolBase<TElement>
	{
		public ElementsPoolBase()
		{
			this.store = new Dictionary<string, Stack<TElement>>();
			this.syncRoot = new object();
		}

		public TElement CreateElement(string elementName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(elementName, "elementName");
			Stack<TElement> stack;
			TElement result;
			if (!this.store.TryGetValue(elementName, out stack) || stack.Count == 0)
			{
				result = this.CreateInstance(elementName);
			}
			else
			{
				lock (this.syncRoot)
				{
					if (stack.Count == 0)
					{
						result = this.CreateInstance(elementName);
					}
					else
					{
						result = stack.Pop();
					}
				}
			}
			return result;
		}

		public void ReleaseElement(string elementName, TElement element)
		{
			Guard.ThrowExceptionIfNull<TElement>(element, "element");
			Guard.ThrowExceptionIfNullOrEmpty(elementName, "elementName");
			Stack<TElement> value;
			if (!this.store.TryGetValue(elementName, out value))
			{
				lock (this.syncRoot)
				{
					if (!this.store.TryGetValue(elementName, out value))
					{
						value = new Stack<TElement>();
						this.store[elementName] = value;
					}
				}
			}
			this.store[elementName].Push(element);
		}

		protected abstract TElement CreateInstance(string elementName);

		readonly Dictionary<string, Stack<TElement>> store;

		readonly object syncRoot;
	}
}

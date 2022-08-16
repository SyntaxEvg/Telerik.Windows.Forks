using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model
{
	abstract class OpenXmlElementsPoolBase<T> where T : OpenXmlPartsManager
	{
		public OpenXmlElementsPoolBase(T partsManager)
		{
			Guard.ThrowExceptionIfNull<T>(partsManager, "partsManager");
			this.partsManager = partsManager;
			this.store = new Dictionary<string, Stack<OpenXmlElementBase>>();
			this.syncRoot = new object();
		}

		public T PartsManager
		{
			get
			{
				return this.partsManager;
			}
		}

		public OpenXmlElementBase CreateElement(string elementName, OpenXmlPartBase part)
		{
			Guard.ThrowExceptionIfNullOrEmpty(elementName, "elementName");
			Guard.ThrowExceptionIfNull<OpenXmlPartBase>(part, "part");
			Stack<OpenXmlElementBase> stack;
			OpenXmlElementBase openXmlElementBase;
			if (!this.store.TryGetValue(elementName, out stack) || stack.Count == 0)
			{
				openXmlElementBase = this.CreateInstance(elementName);
			}
			else
			{
				lock (this.syncRoot)
				{
					if (stack.Count == 0)
					{
						openXmlElementBase = this.CreateInstance(elementName);
					}
					else
					{
						openXmlElementBase = stack.Pop();
					}
				}
			}
			if (openXmlElementBase != null)
			{
				openXmlElementBase.Part = part;
			}
			return openXmlElementBase;
		}

		public void ReleaseElement(OpenXmlElementBase element)
		{
			Guard.ThrowExceptionIfNull<OpenXmlElementBase>(element, "element");
			string key = element.UniquePoolId ?? element.ElementName;
			element.Clear();
			Stack<OpenXmlElementBase> value;
			if (!this.store.TryGetValue(key, out value))
			{
				lock (this.syncRoot)
				{
					if (!this.store.TryGetValue(key, out value))
					{
						value = new Stack<OpenXmlElementBase>();
						this.store[key] = value;
					}
				}
			}
			this.store[key].Push(element);
		}

		protected virtual OpenXmlElementBase CreateInstance(string elementName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(elementName, "elementName");
			if (OpenXmlElementsFactory.CanCreateInstance(elementName))
			{
				return OpenXmlElementsFactory.CreateInstance(elementName, this.PartsManager);
			}
			return null;
		}

		readonly T partsManager;

		readonly Dictionary<string, Stack<OpenXmlElementBase>> store;

		readonly object syncRoot;
	}
}

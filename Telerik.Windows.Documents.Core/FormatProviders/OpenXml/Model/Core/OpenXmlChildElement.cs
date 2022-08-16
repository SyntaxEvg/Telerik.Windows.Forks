using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core
{
	class OpenXmlChildElement<T> : IOpenXmlChildElement where T : OpenXmlElementBase
	{
		public OpenXmlChildElement(string elementName)
			: this(elementName, elementName)
		{
		}

		public OpenXmlChildElement(string elementName, string uniqueElementName)
		{
			Guard.ThrowExceptionIfNull<string>(elementName, "elementName");
			Guard.ThrowExceptionIfNull<string>(uniqueElementName, "uniqueElementName");
			this.elementName = elementName;
			this.uniquePoolId = uniqueElementName;
		}

		public OpenXmlChildElement(T element)
		{
			Guard.ThrowExceptionIfNull<T>(element, "element");
			this.element = element;
			this.elementName = this.element.ElementName;
		}

		public T Element
		{
			get
			{
				return this.element;
			}
			set
			{
				if (this.element != value)
				{
					this.element = value;
				}
			}
		}

		string IOpenXmlChildElement.ElementName
		{
			get
			{
				return this.elementName;
			}
		}

		string IOpenXmlChildElement.UniquePoolId
		{
			get
			{
				return this.uniquePoolId;
			}
		}

		bool IOpenXmlChildElement.HasElement
		{
			get
			{
				return this.Element != null;
			}
		}

		void IOpenXmlChildElement.SetElement(OpenXmlElementBase element)
		{
			this.Element = (T)((object)element);
		}

		OpenXmlElementBase IOpenXmlChildElement.GetElement()
		{
			return this.Element;
		}

		void IOpenXmlChildElement.ClearElement()
		{
			this.Element = default(T);
		}

		readonly string elementName;

		readonly string uniquePoolId;

		T element;
	}
}

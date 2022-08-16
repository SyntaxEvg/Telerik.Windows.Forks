using System;
using System.Xml;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core
{
	abstract class DirectElementBase<T> : ElementBase
	{
		public void Write(T value)
		{
			this.OnBeforeWrite(value);
			this.InitializeAttributesOverride(value);
			if (base.ShouldExport())
			{
				base.BeginWrite();
				this.WriteChildElementsOverride(value);
				base.EndWrite();
			}
		}

		public void Read(ref T value)
		{
			base.BeginRead();
			if (!base.IsEmptyElement)
			{
				XmlNodeType nodeType = base.Reader.GetNodeType();
				if (nodeType == XmlNodeType.Text || nodeType == XmlNodeType.SignificantWhitespace)
				{
					base.ReadInnerText();
					base.Reader.Read();
				}
				else
				{
					this.ReadChildElements(ref value);
				}
			}
			this.CopyAttributesOverride(ref value);
			base.EndRead();
		}

		protected virtual void OnBeforeWrite(T value)
		{
		}

		protected abstract void InitializeAttributesOverride(T value);

		protected abstract void WriteChildElementsOverride(T value);

		protected virtual void CopyAttributesOverride(ref T value)
		{
			throw new NotImplementedException();
		}

		protected virtual void ReadChildElementOverride(ElementBase element, ref T value)
		{
			throw new NotImplementedException();
		}

		protected TResult CreateChildElement<TResult>() where TResult : ElementBase, new()
		{
			ElementContext context = new ElementContext(base.Writer, base.Reader, base.Theme);
			TResult result = Activator.CreateInstance<TResult>();
			result.SetContext(context);
			return result;
		}

		void ReadChildElements(ref T value)
		{
			while (!base.Reader.IsElementOfType(XmlNodeType.EndElement))
			{
				XmlNodeType nodeType = base.Reader.GetNodeType();
				if (nodeType == XmlNodeType.Element)
				{
					string elementName;
					if (base.Reader.GetElementName(out elementName))
					{
						ElementBase elementBase = base.CreateElement(elementName);
						if (elementBase != null)
						{
							this.ReadChildElementOverride(elementBase, ref value);
						}
						else
						{
							base.Reader.SkipElement();
						}
					}
					else
					{
						base.Reader.SkipElement();
					}
				}
				else
				{
					base.Reader.SkipElement();
				}
			}
			base.Reader.Read();
		}
	}
}

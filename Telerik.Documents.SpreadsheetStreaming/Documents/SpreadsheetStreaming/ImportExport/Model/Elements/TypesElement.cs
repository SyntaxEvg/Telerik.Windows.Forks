using System;
using System.Collections.Generic;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Writers;
using Telerik.Documents.SpreadsheetStreaming.Model.ContentTypes;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements
{
	class TypesElement : DirectElementBase<ContentTypesRepository>
	{
		public override OpenXmlNamespace Namespace
		{
			get
			{
				return OpenXmlNamespaces.ContentTypesNamespace;
			}
		}

		public override string ElementName
		{
			get
			{
				return "Types";
			}
		}

		protected override void InitializeAttributesOverride(ContentTypesRepository contentTypesRepository)
		{
		}

		protected override void WriteChildElementsOverride(ContentTypesRepository contentTypesRepository)
		{
			this.WriteDefaultElements(contentTypesRepository.Defaults);
			this.WriteOverrideElements(contentTypesRepository.Overrides);
		}

		protected override void CopyAttributesOverride(ref ContentTypesRepository value)
		{
		}

		protected override void ReadChildElementOverride(ElementBase element, ref ContentTypesRepository value)
		{
			string elementName;
			if ((elementName = element.ElementName) != null)
			{
				if (elementName == "Default")
				{
					DefaultElement defaultElement = element as DefaultElement;
					DefaultContentType value2 = new DefaultContentType();
					defaultElement.Read(ref value2);
					value.Register(value2);
					return;
				}
				if (elementName == "Override")
				{
					OverrideElement overrideElement = element as OverrideElement;
					OverrideContentType value3 = new OverrideContentType();
					overrideElement.Read(ref value3);
					value.Register(value3);
					return;
				}
			}
			throw new NotSupportedException();
		}

		void WriteDefaultElements(IEnumerable<DefaultContentType> defaults)
		{
			foreach (DefaultContentType value in defaults)
			{
				DefaultElement defaultElement = base.CreateChildElement<DefaultElement>();
				defaultElement.Write(value);
			}
		}

		void WriteOverrideElements(IEnumerable<OverrideContentType> overrides)
		{
			foreach (OverrideContentType value in overrides)
			{
				OverrideElement overrideElement = base.CreateChildElement<OverrideElement>();
				overrideElement.Write(value);
			}
		}
	}
}

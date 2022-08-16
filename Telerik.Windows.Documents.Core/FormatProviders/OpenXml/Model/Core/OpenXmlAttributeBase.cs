using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core
{
	abstract class OpenXmlAttributeBase
	{
		public OpenXmlAttributeBase(string name, OpenXmlNamespace ns, bool isRequired = false)
		{
			this.name = name;
			this.ns = ns;
			this.isRequired = isRequired;
		}

		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public OpenXmlNamespace Namespace
		{
			get
			{
				return this.ns;
			}
		}

		public bool IsRequired
		{
			get
			{
				return this.isRequired;
			}
		}

		public abstract bool HasValue { get; }

		public abstract string GetValueAsString();

		public abstract void SetStringValue(IOpenXmlImportContext context, string value);

		public abstract bool ShouldExport();

		public abstract void ResetValue();

		readonly string name;

		readonly OpenXmlNamespace ns;

		readonly bool isRequired;
	}
}

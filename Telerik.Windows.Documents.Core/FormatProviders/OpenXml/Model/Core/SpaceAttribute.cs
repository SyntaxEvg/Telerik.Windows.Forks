using System;
using System.Linq;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core
{
	class SpaceAttribute : OpenXmlAttributeBase
	{
		public SpaceAttribute(OpenXmlElementBase element)
			: base("space", new OpenXmlNamespace("", "xml", ""), false)
		{
			Guard.ThrowExceptionIfNull<OpenXmlElementBase>(element, "element");
			this.element = element;
		}

		public string Value { get; set; }

		public override bool HasValue
		{
			get
			{
				return !string.IsNullOrEmpty(this.Value);
			}
		}

		public override string GetValueAsString()
		{
			return this.Value;
		}

		public override void SetStringValue(IOpenXmlImportContext context, string value)
		{
			this.Value = value;
		}

		public override bool ShouldExport()
		{
			string innerText = this.element.InnerText;
			if (!string.IsNullOrEmpty(innerText) && (char.IsWhiteSpace(innerText.First<char>()) || char.IsWhiteSpace(innerText.Last<char>())))
			{
				this.Value = "preserve";
				return true;
			}
			return false;
		}

		public override void ResetValue()
		{
			this.Value = null;
		}

		const string Preserve = "preserve";

		readonly OpenXmlElementBase element;
	}
}

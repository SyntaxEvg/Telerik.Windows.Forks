using System;
using Telerik.Windows.Documents.Common.FormatProviders.XmlBased.Model.Namespaces;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Converters;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Attributes
{
	class StyleValueAttribute : HtmlAttribute
	{
		public StyleValueAttribute(string name, string stylePropertyName, StyleAttribute styleAttribute, XmlNamespace ns = null, bool isRequired = false, ILegacyConverter converter = null)
			: base(name, ns, isRequired)
		{
			Guard.ThrowExceptionIfNull<StyleAttribute>(styleAttribute, "styleAttribute");
			Guard.ThrowExceptionIfNull<string>(stylePropertyName, "stylePropertyName");
			this.styleAttribute = styleAttribute;
			this.stylePropertyName = stylePropertyName;
			this.converter = converter;
		}

		public StyleValueAttribute(string name, StyleAttribute styleAttribute, XmlNamespace ns = null, bool isRequired = false, ILegacyConverter converter = null)
			: this(name, name, styleAttribute, ns, isRequired, converter)
		{
		}

		public override bool HasValue
		{
			get
			{
				return false;
			}
		}

		public override string GetValue()
		{
			throw new NotSupportedException();
		}

		public override void ResetValue()
		{
		}

		public override void SetValue(string value)
		{
			HtmlStyleProperty htmlStyleProperty;
			if (!this.styleAttribute.Value.TryGetProperty(this.stylePropertyName, out htmlStyleProperty))
			{
				string empty = string.Empty;
				if (this.converter != null && this.converter.TryGetConvertedValue(value, out empty))
				{
					this.styleAttribute.Value.RegisterProperty(this.stylePropertyName, empty);
					return;
				}
				this.styleAttribute.Value.RegisterProperty(this.stylePropertyName, value);
			}
		}

		public override bool ShouldExport()
		{
			return false;
		}

		readonly StyleAttribute styleAttribute;

		readonly string stylePropertyName;

		readonly ILegacyConverter converter;
	}
}

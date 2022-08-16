using System;
using System.Globalization;
using Telerik.Windows.Documents.Common.FormatProviders.XmlBased.Model.Namespaces;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Attributes
{
	class HtmlAttribute<T> : HtmlAttribute
	{
		public HtmlAttribute(string name, XmlNamespace ns, bool isRequired = false)
			: this(name, ns, default(T), isRequired)
		{
		}

		public HtmlAttribute(string name, bool isRequired = false)
			: this(name, null, default(T), isRequired)
		{
		}

		public HtmlAttribute(string name, XmlNamespace ns, T defaultValue, bool isRequired = false)
			: base(name, ns, isRequired)
		{
			this.defaultValue = defaultValue;
			this.value = defaultValue;
		}

		public T DefaultValue
		{
			get
			{
				return this.defaultValue;
			}
			protected set
			{
				this.defaultValue = value;
				if (!this.isSet)
				{
					this.ResetValue();
				}
			}
		}

		public T Value
		{
			get
			{
				return this.value;
			}
			set
			{
				if (!ObjectExtensions.EqualsOfT<T>(this.value, value))
				{
					this.value = value;
				}
				this.isSet = true;
			}
		}

		public override bool HasValue
		{
			get
			{
				return this.isSet || !ObjectExtensions.EqualsOfT<T>(this.value, this.defaultValue);
			}
		}

		public override string GetValue()
		{
			if (object.ReferenceEquals(this.Value, null))
			{
				return string.Empty;
			}
			T t = this.Value;
			return t.ToString();
		}

		public override void SetValue(string value)
		{
			Guard.ThrowExceptionIfNull<string>(value, "value");
			this.Value = (T)((object)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture));
			this.isSet = true;
		}

		public override bool ShouldExport()
		{
			return base.IsRequired || this.HasValue;
		}

		public override void ResetValue()
		{
			this.value = this.defaultValue;
			this.isSet = false;
		}

		T defaultValue;

		T value;

		bool isSet;
	}
}

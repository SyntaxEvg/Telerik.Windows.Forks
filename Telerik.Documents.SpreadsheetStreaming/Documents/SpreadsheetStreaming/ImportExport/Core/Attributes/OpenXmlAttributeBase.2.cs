using System;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Attributes
{
	abstract class OpenXmlAttributeBase<T> : OpenXmlAttributeBase
	{
		public OpenXmlAttributeBase(string name, OpenXmlNamespace ns, bool isRequired = false)
			: this(name, ns, default(T), isRequired)
		{
		}

		public OpenXmlAttributeBase(string name, bool isRequired = false)
			: this(name, null, default(T), isRequired)
		{
		}

		public OpenXmlAttributeBase(string name, OpenXmlNamespace ns, T defaultValue, bool isRequired = false)
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
				base.EnsureSetAttributeAllowed();
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

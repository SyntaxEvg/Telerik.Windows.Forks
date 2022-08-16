using System;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Attributes
{
	abstract class OpenXmlAttributeBase
	{
		public OpenXmlAttributeBase(string name, OpenXmlNamespace ns, bool isRequired = false)
		{
			this.name = name;
			this.ns = ns;
			this.isRequired = isRequired;
			this.isWritten = false;
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

		public void SetStringValue(string value)
		{
			this.EnsureSetAttributeAllowed();
			this.SetStringValueOverride(value);
		}

		public abstract bool ShouldExport();

		public abstract void ResetValue();

		internal void MarkAsWritten()
		{
			this.isWritten = true;
		}

		protected void EnsureSetAttributeAllowed()
		{
			if (this.isWritten)
			{
				throw new InvalidOperationException("All attributes must be set before writing the element start.");
			}
		}

		protected abstract void SetStringValueOverride(string value);

		readonly string name;

		readonly OpenXmlNamespace ns;

		readonly bool isRequired;

		bool isWritten;
	}
}

using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.InteractiveForms
{
	public class RadioOption : FieldOptionBase, IContextClonable<RadioOption>
	{
		public RadioOption(string value)
			: base(value)
		{
		}

		public override string Value
		{
			get
			{
				return this.value;
			}
			set
			{
				Guard.ThrowExceptionIfNull<string>(value, "value");
				this.value = value;
			}
		}

		RadioOption IContextClonable<RadioOption>.Clone(RadFixedDocumentCloneContext cloneContext)
		{
			return new RadioOption(this.Value);
		}

		internal bool Equals(RadioOption other, bool shouldUpdateRadiosInUnison)
		{
			bool result;
			if (shouldUpdateRadiosInUnison)
			{
				result = other != null && other.Value.Equals(this.Value);
			}
			else
			{
				result = other != null && other.Equals(this);
			}
			return result;
		}

		string value;
	}
}

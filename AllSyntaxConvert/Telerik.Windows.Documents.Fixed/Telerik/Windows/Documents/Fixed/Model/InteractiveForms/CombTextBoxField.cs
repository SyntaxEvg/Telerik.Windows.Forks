using System;

namespace Telerik.Windows.Documents.Fixed.Model.InteractiveForms
{
	public sealed class CombTextBoxField : TextField
	{
		public CombTextBoxField(string fieldName)
			: base(fieldName)
		{
		}

		public override FormFieldType FieldType
		{
			get
			{
				return FormFieldType.CombTextBox;
			}
		}

		public int MaxLengthOfInputCharacters
		{
			get
			{
				return this.maxLengthOfInputCharacters;
			}
			set
			{
				if (this.maxLengthOfInputCharacters != value)
				{
					base.InvalidateWidgetAppearances();
					this.maxLengthOfInputCharacters = value;
				}
			}
		}

		internal sealed override TextField GetClonedTextField(RadFixedDocumentCloneContext cloneContext)
		{
			return new CombTextBoxField(base.Name)
			{
				MaxLengthOfInputCharacters = this.MaxLengthOfInputCharacters
			};
		}

		int maxLengthOfInputCharacters;
	}
}

using System;

namespace Telerik.Windows.Documents.Fixed.Model.InteractiveForms
{
	public sealed class TextBoxField : TextField
	{
		public TextBoxField(string fieldName)
			: base(fieldName)
		{
		}

		public override FormFieldType FieldType
		{
			get
			{
				return FormFieldType.TextBox;
			}
		}

		public bool IsMultiline
		{
			get
			{
				return this.isMultiline;
			}
			set
			{
				if (this.isMultiline != value)
				{
					base.InvalidateWidgetAppearances();
					this.isMultiline = value;
				}
			}
		}

		public bool IsPassword
		{
			get
			{
				return this.isPassword;
			}
			set
			{
				if (this.isPassword != value)
				{
					base.InvalidateWidgetAppearances();
					this.isPassword = value;
				}
			}
		}

		public bool IsFileSelect { get; set; }

		public bool ShouldSpellCheck { get; set; }

		public bool AllowScroll { get; set; }

		public int? MaxLengthOfInputCharacters
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
			return new TextBoxField(base.Name)
			{
				IsMultiline = this.IsMultiline,
				IsPassword = this.IsPassword,
				IsFileSelect = this.IsFileSelect,
				ShouldSpellCheck = this.ShouldSpellCheck,
				AllowScroll = this.AllowScroll,
				MaxLengthOfInputCharacters = this.MaxLengthOfInputCharacters
			};
		}

		bool isMultiline;

		bool isPassword;

		int? maxLengthOfInputCharacters;
	}
}

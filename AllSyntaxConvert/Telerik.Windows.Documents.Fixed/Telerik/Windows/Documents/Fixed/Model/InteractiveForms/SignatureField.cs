using System;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.DigitalSignatures;

namespace Telerik.Windows.Documents.Fixed.Model.InteractiveForms
{
	public sealed class SignatureField : FormField<SignatureWidget>
	{
		public SignatureField(string fieldName)
			: base(fieldName)
		{
		}

		public Signature Signature
		{
			get
			{
				return this.signature;
			}
			set
			{
				if (value == null)
				{
					this.signature = null;
					return;
				}
				if (!string.IsNullOrEmpty(value.Properties.FieldName))
				{
					throw new ArgumentException("The new signature value is already associated with another signature field named \"" + value.Properties.FieldName + "\".");
				}
				this.signature = value;
				this.signature.Properties.FieldName = base.Name;
			}
		}

		public sealed override FormFieldType FieldType
		{
			get
			{
				return FormFieldType.Signature;
			}
		}

		internal sealed override SignatureWidget CreateEmptyWidget()
		{
			return new SignatureWidget(this);
		}

		internal sealed override void PrepareWidgetAppearancesForExport()
		{
			foreach (SignatureWidget signatureWidget in base.Widgets)
			{
				signatureWidget.PrepareAppearancesForExport();
			}
		}

		internal override FormField GetClonedInstanceWithoutWidgetsOverride(RadFixedDocumentCloneContext cloneContext)
		{
			return new SignatureField(base.Name)
			{
				Signature = this.Signature
			};
		}

		internal sealed override void InitializeWidgetAppearanceProperties(SignatureWidget widget)
		{
		}

		Signature signature;
	}
}

using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Objects;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Annotations
{
	class AppearanceCharacteristics : PdfObject
	{
		public AppearanceCharacteristics()
		{
			this.rotation = base.RegisterDirectProperty<PdfInt>(new PdfPropertyDescriptor("R"), new PdfInt(0));
			this.borderColor = base.RegisterReferenceProperty<PdfArray>(new PdfPropertyDescriptor("BC"));
			this.background = base.RegisterReferenceProperty<PdfArray>(new PdfPropertyDescriptor("BG"));
			this.buttonNormalCaption = base.RegisterDirectProperty<PdfString>(new PdfPropertyDescriptor("CA"));
			this.pushButtonRolloverCaption = base.RegisterDirectProperty<PdfString>(new PdfPropertyDescriptor("RC"));
			this.pushButtonDownCaption = base.RegisterDirectProperty<PdfString>(new PdfPropertyDescriptor("AC"));
			this.pushButtonNormalIcon = base.RegisterReferenceProperty<FormXObject>(new PdfPropertyDescriptor("I", false, PdfPropertyRestrictions.MustBeIndirectReference));
			this.pushButtonRolloverIcon = base.RegisterReferenceProperty<FormXObject>(new PdfPropertyDescriptor("RI", false, PdfPropertyRestrictions.MustBeIndirectReference));
			this.pushButtonDownIcon = base.RegisterReferenceProperty<FormXObject>(new PdfPropertyDescriptor("IX", false, PdfPropertyRestrictions.MustBeIndirectReference));
			this.pushButtonIconFit = base.RegisterReferenceProperty<IconFit>(new PdfPropertyDescriptor("IF"), new IconFit());
			this.pushButtonTextPosition = base.RegisterDirectProperty<PdfInt>(new PdfPropertyDescriptor("TP"), new PdfInt(0));
		}

		public PdfInt Rotation
		{
			get
			{
				return this.rotation.GetValue();
			}
			set
			{
				this.rotation.SetValue(value);
			}
		}

		public PdfArray BorderColor
		{
			get
			{
				return this.borderColor.GetValue();
			}
			set
			{
				this.borderColor.SetValue(value);
			}
		}

		public PdfArray Background
		{
			get
			{
				return this.background.GetValue();
			}
			set
			{
				this.background.SetValue(value);
			}
		}

		public PdfString ButtonNormalCaption
		{
			get
			{
				return this.buttonNormalCaption.GetValue();
			}
			set
			{
				this.buttonNormalCaption.SetValue(value);
			}
		}

		public PdfString RolloverCaption
		{
			get
			{
				return this.pushButtonRolloverCaption.GetValue();
			}
			set
			{
				this.pushButtonRolloverCaption.SetValue(value);
			}
		}

		public PdfString DownCaption
		{
			get
			{
				return this.pushButtonDownCaption.GetValue();
			}
			set
			{
				this.pushButtonDownCaption.SetValue(value);
			}
		}

		public FormXObject NormalIcon
		{
			get
			{
				return this.pushButtonNormalIcon.GetValue();
			}
			set
			{
				this.pushButtonNormalIcon.SetValue(value);
			}
		}

		public IconFit IconFit
		{
			get
			{
				return this.pushButtonIconFit.GetValue();
			}
			set
			{
				this.pushButtonIconFit.SetValue(value);
			}
		}

		public FormXObject RolloverIcon
		{
			get
			{
				return this.pushButtonRolloverIcon.GetValue();
			}
			set
			{
				this.pushButtonRolloverIcon.SetValue(value);
			}
		}

		public FormXObject DownIcon
		{
			get
			{
				return this.pushButtonDownIcon.GetValue();
			}
			set
			{
				this.pushButtonDownIcon.SetValue(value);
			}
		}

		public PdfInt PushButtonTextPosition
		{
			get
			{
				return this.pushButtonTextPosition.GetValue();
			}
			set
			{
				this.pushButtonTextPosition.SetValue(value);
			}
		}

		readonly DirectProperty<PdfInt> rotation;

		readonly ReferenceProperty<PdfArray> borderColor;

		readonly ReferenceProperty<PdfArray> background;

		readonly DirectProperty<PdfString> buttonNormalCaption;

		readonly DirectProperty<PdfString> pushButtonRolloverCaption;

		readonly DirectProperty<PdfString> pushButtonDownCaption;

		readonly ReferenceProperty<FormXObject> pushButtonNormalIcon;

		readonly ReferenceProperty<FormXObject> pushButtonRolloverIcon;

		readonly ReferenceProperty<FormXObject> pushButtonDownIcon;

		readonly ReferenceProperty<IconFit> pushButtonIconFit;

		readonly DirectProperty<PdfInt> pushButtonTextPosition;
	}
}

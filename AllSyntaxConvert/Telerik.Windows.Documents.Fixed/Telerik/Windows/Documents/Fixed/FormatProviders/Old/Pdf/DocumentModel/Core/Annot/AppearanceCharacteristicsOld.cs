using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.XObjects;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Annot
{
	[PdfClass]
	class AppearanceCharacteristicsOld : PdfObjectOld
	{
		public AppearanceCharacteristicsOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.rotation = base.CreateInstantLoadProperty<PdfIntOld>(new PdfPropertyDescriptor("R"));
			this.borderColor = base.CreateLoadOnDemandProperty<PdfArrayOld>(new PdfPropertyDescriptor("BC"));
			this.background = base.CreateLoadOnDemandProperty<PdfArrayOld>(new PdfPropertyDescriptor("BG"));
			this.normalCaption = base.CreateInstantLoadProperty<PdfStringOld>(new PdfPropertyDescriptor("CA"));
			this.rolloverCaption = base.CreateInstantLoadProperty<PdfStringOld>(new PdfPropertyDescriptor("RC"));
			this.downCaption = base.CreateInstantLoadProperty<PdfStringOld>(new PdfPropertyDescriptor("AC"));
			this.normalIcon = base.CreateLoadOnDemandProperty<XForm>(new PdfPropertyDescriptor("I"), Converters.XObjectConverter);
			this.rolloverIcon = base.CreateLoadOnDemandProperty<XForm>(new PdfPropertyDescriptor("RI"), Converters.XObjectConverter);
			this.downIcon = base.CreateLoadOnDemandProperty<XForm>(new PdfPropertyDescriptor("IX"), Converters.XObjectConverter);
			this.iconFit = base.CreateLoadOnDemandProperty<PdfDictionaryOld>(new PdfPropertyDescriptor("IF"));
			this.captionTextPosition = base.CreateInstantLoadProperty<PdfIntOld>(new PdfPropertyDescriptor("TP"));
		}

		public PdfIntOld Rotation
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

		public PdfArrayOld BorderColor
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

		public PdfArrayOld Background
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

		public PdfStringOld NormalCaption
		{
			get
			{
				return this.normalCaption.GetValue();
			}
			set
			{
				this.normalCaption.SetValue(value);
			}
		}

		public PdfStringOld RolloverCaption
		{
			get
			{
				return this.rolloverCaption.GetValue();
			}
			set
			{
				this.rolloverCaption.SetValue(value);
			}
		}

		public PdfStringOld DownCaption
		{
			get
			{
				return this.downCaption.GetValue();
			}
			set
			{
				this.downCaption.SetValue(value);
			}
		}

		public XForm NormalIcon
		{
			get
			{
				return this.normalIcon.GetValue();
			}
			set
			{
				this.normalIcon.SetValue(value);
			}
		}

		public XForm RolloverIcon
		{
			get
			{
				return this.rolloverIcon.GetValue();
			}
			set
			{
				this.rolloverIcon.SetValue(value);
			}
		}

		public XForm DownIcon
		{
			get
			{
				return this.downIcon.GetValue();
			}
			set
			{
				this.downIcon.SetValue(value);
			}
		}

		public PdfDictionaryOld IconFit
		{
			get
			{
				return this.iconFit.GetValue();
			}
			set
			{
				this.iconFit.SetValue(value);
			}
		}

		public PdfIntOld CaptionTextPosition
		{
			get
			{
				return this.captionTextPosition.GetValue();
			}
			set
			{
				this.captionTextPosition.SetValue(value);
			}
		}

		readonly InstantLoadProperty<PdfIntOld> rotation;

		readonly LoadOnDemandProperty<PdfArrayOld> borderColor;

		readonly LoadOnDemandProperty<PdfArrayOld> background;

		readonly InstantLoadProperty<PdfStringOld> normalCaption;

		readonly InstantLoadProperty<PdfStringOld> rolloverCaption;

		readonly InstantLoadProperty<PdfStringOld> downCaption;

		readonly LoadOnDemandProperty<XForm> normalIcon;

		readonly LoadOnDemandProperty<XForm> rolloverIcon;

		readonly LoadOnDemandProperty<XForm> downIcon;

		readonly LoadOnDemandProperty<PdfDictionaryOld> iconFit;

		readonly InstantLoadProperty<PdfIntOld> captionTextPosition;
	}
}

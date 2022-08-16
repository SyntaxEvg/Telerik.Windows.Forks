using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Annot
{
	[PdfClass]
	class AppearancesOld : PdfObjectOld
	{
		public AppearancesOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.normalAppearance = base.CreateLoadOnDemandProperty<AppearanceOld>(new PdfPropertyDescriptor
			{
				Name = "N",
				IsRequired = true
			}, Converters.AppearanceConverter);
			this.rolloverAppearance = base.CreateLoadOnDemandProperty<AppearanceOld>(new PdfPropertyDescriptor
			{
				Name = "R"
			}, Converters.AppearanceConverter);
			this.downAppearance = base.CreateLoadOnDemandProperty<AppearanceOld>(new PdfPropertyDescriptor
			{
				Name = "D"
			}, Converters.AppearanceConverter);
		}

		public AppearanceOld NormalAppearance
		{
			get
			{
				return this.normalAppearance.GetValue();
			}
			set
			{
				this.normalAppearance.SetValue(value);
			}
		}

		public AppearanceOld RolloverAppearance
		{
			get
			{
				return this.rolloverAppearance.GetValue();
			}
			set
			{
				this.rolloverAppearance.SetValue(value);
			}
		}

		public AppearanceOld DownAppearance
		{
			get
			{
				return this.downAppearance.GetValue();
			}
			set
			{
				this.downAppearance.SetValue(value);
			}
		}

		readonly LoadOnDemandProperty<AppearanceOld> normalAppearance;

		readonly LoadOnDemandProperty<AppearanceOld> rolloverAppearance;

		readonly LoadOnDemandProperty<AppearanceOld> downAppearance;
	}
}

using System;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.Model.Internal;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.ColorSpaces
{
	[PdfClass(TypeName = "Pattern", SubtypeProperty = "PatternType", SubtypeValue = "2")]
	class ShadingPatternOld : PatternOld
	{
		public ShadingPatternOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.shading = base.CreateLoadOnDemandProperty<ShadingOld>(new PdfPropertyDescriptor("Shading", true), Converters.ShadingConverter);
			this.extGState = base.CreateInstantLoadProperty<ExtGStateOld>(new PdfPropertyDescriptor("ExtGState"));
		}

		public ShadingOld Shading
		{
			get
			{
				return this.shading.GetValue();
			}
			set
			{
				this.shading.SetValue(value);
			}
		}

		public ExtGStateOld Resources
		{
			get
			{
				return this.extGState.GetValue();
			}
			set
			{
				this.extGState.SetValue(value);
			}
		}

		protected override PatternBrush CreateBrushOverride(Matrix transform, object[] pars)
		{
			return this.Shading.CreateBrush(transform, pars);
		}

		readonly LoadOnDemandProperty<ShadingOld> shading;

		readonly InstantLoadProperty<ExtGStateOld> extGState;
	}
}

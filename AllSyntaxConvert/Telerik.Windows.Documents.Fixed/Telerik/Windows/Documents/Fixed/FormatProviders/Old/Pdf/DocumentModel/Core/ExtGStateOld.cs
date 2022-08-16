using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core
{
	[PdfClass]
	class ExtGStateOld : PdfObjectOld
	{
		public ExtGStateOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.lineWidth = base.CreateInstantLoadProperty<PdfRealOld>(new PdfPropertyDescriptor
			{
				Name = "LW"
			}, Converters.PdfRealConverter);
			this.lineCap = base.CreateInstantLoadProperty<PdfIntOld>(new PdfPropertyDescriptor
			{
				Name = "LC"
			});
			this.lineJoin = base.CreateInstantLoadProperty<PdfIntOld>(new PdfPropertyDescriptor
			{
				Name = "LJ"
			});
			this.mitterLimit = base.CreateInstantLoadProperty<PdfRealOld>(new PdfPropertyDescriptor
			{
				Name = "ML"
			}, Converters.PdfRealConverter);
			this.dashPattern = base.CreateInstantLoadProperty<PdfArrayOld>(new PdfPropertyDescriptor
			{
				Name = "D"
			});
			this.renderingIndent = base.CreateInstantLoadProperty<PdfNameOld>(new PdfPropertyDescriptor
			{
				Name = "RI"
			});
			this.overprintAll = base.CreateInstantLoadProperty<PdfBoolOld>(new PdfPropertyDescriptor
			{
				Name = "OP"
			});
			this.overprint = base.CreateInstantLoadProperty<PdfBoolOld>(new PdfPropertyDescriptor
			{
				Name = "op"
			});
			this.overprintMode = base.CreateInstantLoadProperty<PdfIntOld>(new PdfPropertyDescriptor
			{
				Name = "OPM"
			});
			this.font = base.CreateInstantLoadProperty<PdfArrayOld>(new PdfPropertyDescriptor
			{
				Name = "Font"
			});
			this.strokeAlphaConstant = base.CreateInstantLoadProperty<PdfRealOld>(new PdfPropertyDescriptor
			{
				Name = "CA"
			}, Converters.PdfRealConverter);
			this.alphaConstant = base.CreateInstantLoadProperty<PdfRealOld>(new PdfPropertyDescriptor
			{
				Name = "ca"
			}, Converters.PdfRealConverter);
			this.alphaSource = base.CreateInstantLoadProperty<PdfBoolOld>(new PdfPropertyDescriptor
			{
				Name = "AIS"
			});
		}

		public PdfRealOld LineWidth
		{
			get
			{
				return this.lineWidth.GetValue();
			}
			set
			{
				this.lineWidth.SetValue(value);
			}
		}

		public PdfIntOld LineCap
		{
			get
			{
				return this.lineCap.GetValue();
			}
			set
			{
				this.lineCap.SetValue(value);
			}
		}

		public PdfIntOld LineJoin
		{
			get
			{
				return this.lineJoin.GetValue();
			}
			set
			{
				this.lineJoin.SetValue(value);
			}
		}

		public PdfRealOld MitterLimit
		{
			get
			{
				return this.mitterLimit.GetValue();
			}
			set
			{
				this.mitterLimit.SetValue(value);
			}
		}

		public PdfArrayOld DashPattern
		{
			get
			{
				return this.dashPattern.GetValue();
			}
			set
			{
				this.dashPattern.SetValue(value);
			}
		}

		public PdfNameOld RenderingIndent
		{
			get
			{
				return this.renderingIndent.GetValue();
			}
			set
			{
				this.renderingIndent.SetValue(value);
			}
		}

		public PdfBoolOld OverprintAll
		{
			get
			{
				return this.overprintAll.GetValue();
			}
			set
			{
				this.overprintAll.SetValue(value);
			}
		}

		public PdfBoolOld Overprint
		{
			get
			{
				return this.overprint.GetValue();
			}
			set
			{
				this.overprint.SetValue(value);
			}
		}

		public PdfIntOld OverprintMode
		{
			get
			{
				return this.overprintMode.GetValue();
			}
			set
			{
				this.overprintMode.SetValue(value);
			}
		}

		public PdfArrayOld Font
		{
			get
			{
				return this.font.GetValue();
			}
			set
			{
				this.font.SetValue(value);
			}
		}

		public PdfRealOld StrokeAlphaConstant
		{
			get
			{
				return this.strokeAlphaConstant.GetValue();
			}
			set
			{
				this.strokeAlphaConstant.SetValue(value);
			}
		}

		public PdfRealOld AlphaConstant
		{
			get
			{
				return this.alphaConstant.GetValue();
			}
			set
			{
				this.alphaConstant.SetValue(value);
			}
		}

		public PdfBoolOld AlphaSource
		{
			get
			{
				return this.alphaSource.GetValue();
			}
			set
			{
				this.alphaSource.SetValue(value);
			}
		}

		readonly InstantLoadProperty<PdfRealOld> lineWidth;

		readonly InstantLoadProperty<PdfIntOld> lineCap;

		readonly InstantLoadProperty<PdfIntOld> lineJoin;

		readonly InstantLoadProperty<PdfRealOld> mitterLimit;

		readonly InstantLoadProperty<PdfArrayOld> dashPattern;

		readonly InstantLoadProperty<PdfNameOld> renderingIndent;

		readonly InstantLoadProperty<PdfBoolOld> overprintAll;

		readonly InstantLoadProperty<PdfBoolOld> overprint;

		readonly InstantLoadProperty<PdfIntOld> overprintMode;

		readonly InstantLoadProperty<PdfArrayOld> font;

		readonly InstantLoadProperty<PdfRealOld> strokeAlphaConstant;

		readonly InstantLoadProperty<PdfRealOld> alphaConstant;

		readonly InstantLoadProperty<PdfBoolOld> alphaSource;
	}
}

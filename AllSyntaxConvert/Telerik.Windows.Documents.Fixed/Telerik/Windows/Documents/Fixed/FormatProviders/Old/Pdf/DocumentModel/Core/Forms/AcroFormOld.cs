using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Forms
{
	[PdfClass]
	class AcroFormOld : PdfObjectOld
	{
		public AcroFormOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.fields = base.CreateLoadOnDemandProperty<FormFieldsCollectionOld>(new PdfPropertyDescriptor
			{
				Name = "Fields",
				IsRequired = true
			}, Converters.FieldsConverter);
			this.needAppearance = base.CreateInstantLoadProperty<PdfBoolOld>(new PdfPropertyDescriptor
			{
				Name = "NeedAppearances"
			});
			this.signatureFlags = base.CreateInstantLoadProperty<PdfIntOld>(new PdfPropertyDescriptor
			{
				Name = "SigFlags"
			});
			this.calculationOrder = base.CreateLoadOnDemandProperty<PdfArrayOld>(new PdfPropertyDescriptor
			{
				Name = "CO"
			});
			this.defaultResources = base.CreateLoadOnDemandProperty<PdfResourceOld>(new PdfPropertyDescriptor
			{
				Name = "DR"
			});
			this.defaultAppearance = base.CreateInstantLoadProperty<PdfStringOld>(new PdfPropertyDescriptor
			{
				Name = "DA"
			});
			this.quadding = base.CreateInstantLoadProperty<PdfIntOld>(new PdfPropertyDescriptor
			{
				Name = "Q"
			}, new PdfIntOld(contentManager, 0));
			this.xfa = base.CreateLoadOnDemandProperty<XFAStreamOld>(new PdfPropertyDescriptor
			{
				Name = "XFA"
			}, Converters.XFAConverter);
		}

		public FormFieldsCollectionOld Fields
		{
			get
			{
				return this.fields.GetValue();
			}
			set
			{
				this.fields.SetValue(value);
			}
		}

		public PdfBoolOld NeedAppearance
		{
			get
			{
				return this.needAppearance.GetValue();
			}
			set
			{
				this.needAppearance.SetValue(value);
			}
		}

		public PdfIntOld SignatureFlags
		{
			get
			{
				return this.signatureFlags.GetValue();
			}
			set
			{
				this.signatureFlags.SetValue(value);
			}
		}

		public PdfArrayOld CalculationOrder
		{
			get
			{
				return this.calculationOrder.GetValue();
			}
			set
			{
				this.calculationOrder.SetValue(value);
			}
		}

		public PdfResourceOld DefaultResources
		{
			get
			{
				return this.defaultResources.GetValue();
			}
			set
			{
				this.defaultResources.SetValue(value);
			}
		}

		public PdfStringOld DefaultAppearance
		{
			get
			{
				return this.defaultAppearance.GetValue();
			}
			set
			{
				this.defaultAppearance.SetValue(value);
			}
		}

		public PdfIntOld Quadding
		{
			get
			{
				return this.quadding.GetValue();
			}
			set
			{
				this.quadding.SetValue(value);
			}
		}

		public XFAStreamOld XFA
		{
			get
			{
				return this.xfa.GetValue();
			}
			set
			{
				this.xfa.SetValue(value);
			}
		}

		readonly LoadOnDemandProperty<FormFieldsCollectionOld> fields;

		readonly InstantLoadProperty<PdfBoolOld> needAppearance;

		readonly InstantLoadProperty<PdfIntOld> signatureFlags;

		readonly LoadOnDemandProperty<PdfArrayOld> calculationOrder;

		readonly LoadOnDemandProperty<PdfResourceOld> defaultResources;

		readonly InstantLoadProperty<PdfStringOld> defaultAppearance;

		readonly InstantLoadProperty<PdfIntOld> quadding;

		readonly LoadOnDemandProperty<XFAStreamOld> xfa;
	}
}

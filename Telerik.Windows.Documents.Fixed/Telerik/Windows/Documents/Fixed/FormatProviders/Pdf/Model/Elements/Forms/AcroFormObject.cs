using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Forms
{
	class AcroFormObject : PdfObject, IResourceHolder
	{
		public AcroFormObject()
		{
			this.fields = base.RegisterReferenceProperty<FormFieldsTree>(new PdfPropertyDescriptor("Fields", true, true));
			this.needAppearance = base.RegisterDirectProperty<PdfBool>(new PdfPropertyDescriptor("NeedAppearances"), new PdfBool(false));
			this.sigFlags = base.RegisterDirectProperty<PdfInt>(new PdfPropertyDescriptor("SigFlags"), new PdfInt(0));
			this.calculationOrder = base.RegisterReferenceProperty<PdfArray>(new PdfPropertyDescriptor("CO"));
			this.defaultResources = base.RegisterDirectProperty<PdfResource>(new PdfPropertyDescriptor("DR"));
			this.defaultAppearance = base.RegisterReferenceProperty<PdfString>(new PdfPropertyDescriptor("DA"));
			this.quadding = base.RegisterDirectProperty<PdfInt>(new PdfPropertyDescriptor("Q"), new PdfInt(0));
			this.xfa = base.RegisterReferenceProperty<XFAStream>(new PdfPropertyDescriptor("XFA"));
		}

		public FormFieldsTree Fields
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

		public PdfBool NeedAppearance
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

		public PdfInt SigFlags
		{
			get
			{
				return this.sigFlags.GetValue();
			}
			set
			{
				this.sigFlags.SetValue(value);
			}
		}

		public PdfArray CalculationOrder
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

		public PdfResource Resources
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

		public PdfString DefaultAppearance
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

		public PdfInt Quadding
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

		public XFAStream XFA
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

		internal void SetSignatureFlags()
		{
			FlagWriter<SignatureFlags> flagWriter = new FlagWriter<SignatureFlags>();
			flagWriter.SetFlag(SignatureFlags.SignaturesExist);
			flagWriter.SetFlag(SignatureFlags.AppendOnly);
			this.SigFlags = new PdfInt(flagWriter.ResultFlags);
		}

		public const string FieldsName = "Fields";

		public const string NeedAppearancesName = "NeedAppearances";

		public const string SigFlagsName = "SigFlags";

		public const string COName = "CO";

		public const string DRName = "DR";

		public const string DAName = "DA";

		public const string QName = "Q";

		public const string XFAName = "XFA";

		readonly ReferenceProperty<FormFieldsTree> fields;

		readonly DirectProperty<PdfBool> needAppearance;

		readonly DirectProperty<PdfInt> sigFlags;

		readonly ReferenceProperty<PdfArray> calculationOrder;

		readonly DirectProperty<PdfResource> defaultResources;

		readonly ReferenceProperty<PdfString> defaultAppearance;

		readonly DirectProperty<PdfInt> quadding;

		readonly ReferenceProperty<XFAStream> xfa;
	}
}

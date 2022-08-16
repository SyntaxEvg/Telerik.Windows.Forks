using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.DigitalSignatures;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DigitalSignature
{
	class SignatureReferenceObject : PdfObject
	{
		public SignatureReferenceObject()
		{
			this.transformMethod = base.RegisterDirectProperty<PdfName>(new PdfPropertyDescriptor("TransformMethod", true, true));
			this.transformParams = base.RegisterDirectProperty<PrimitiveWrapper>(new PdfPropertyDescriptor("TransformParams"));
			this.data = base.RegisterReferenceProperty<PrimitiveWrapper>(new PdfPropertyDescriptor("Data"));
			this.digestMethod = base.RegisterDirectProperty<PdfName>(new PdfPropertyDescriptor("DigestMethod"));
			this.digestValue = base.RegisterDirectProperty<PdfString>(new PdfPropertyDescriptor("DigestValue"));
			this.digestLocation = base.RegisterDirectProperty<PdfArray>(new PdfPropertyDescriptor("DigestLocation"));
		}

		public PdfName TransformMethod
		{
			get
			{
				return this.transformMethod.GetValue();
			}
			set
			{
				this.transformMethod.SetValue(value);
			}
		}

		public PrimitiveWrapper TransformParams
		{
			get
			{
				return this.transformParams.GetValue();
			}
			set
			{
				this.transformParams.SetValue(value);
			}
		}

		public PrimitiveWrapper Data
		{
			get
			{
				return this.data.GetValue();
			}
			set
			{
				this.data.SetValue(value);
			}
		}

		public PdfName DigestMethod
		{
			get
			{
				return this.digestMethod.GetValue();
			}
			set
			{
				this.digestMethod.SetValue(value);
			}
		}

		public PdfString DigestValue
		{
			get
			{
				return this.digestValue.GetValue();
			}
			set
			{
				this.digestValue.SetValue(value);
			}
		}

		public PdfArray DigestLocation
		{
			get
			{
				return this.digestLocation.GetValue();
			}
			set
			{
				this.digestLocation.SetValue(value);
			}
		}

		internal void CopyPropertiesFrom(SignatureReference signatureReference)
		{
		}

		internal void CopyPropertiesTo(SignatureReference signatureReference, PostScriptReader reader, IRadFixedDocumentImportContext context)
		{
			Guard.ThrowExceptionIfNull<SignatureReference>(signatureReference, "signatureReference");
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IRadFixedDocumentImportContext>(context, "context");
			this.TransformMethod.CopyToProperty(signatureReference.TransformMethod, (PdfName transformMethod) => transformMethod.Value);
			if (signatureReference.TransformParameters != null)
			{
				signatureReference.TransformParameters.Value = SignatureReferenceObject.ToTransformParameters(this.TransformMethod, this.TransformParams.Primitive, reader, context);
			}
			this.DigestMethod.CopyToProperty(signatureReference.DigestMethod, (PdfName digestMethod) => digestMethod.Value);
			this.DigestValue.CopyToProperty(signatureReference.DigestValue, (PdfString digestValue) => digestValue.Value);
			this.DigestLocation.CopyToProperty(signatureReference.DigestLocation, (PdfArray digestLocation) => digestLocation.ToIntArray());
		}

		internal static TransformParameters ToTransformParameters(PdfName transformMethod, PdfPrimitive primitive, PostScriptReader reader, IRadFixedDocumentImportContext context)
		{
			TransformParameters transformParameters = null;
			string value;
			if ((value = transformMethod.Value) != null)
			{
				TransformParametersElementBase transformParametersElementBase;
				if (!(value == "DocMDP"))
				{
					if (!(value == "UR"))
					{
						if (!(value == "FieldMDP"))
						{
							if (!(value == "Identity"))
							{
								goto IL_72;
							}
							transformParametersElementBase = null;
						}
						else
						{
							transformParametersElementBase = new FieldMdpTransformParametersElement();
							transformParameters = new FieldMdpTransformParameters();
						}
					}
					else
					{
						transformParametersElementBase = new UrTransformParametersElement();
						transformParameters = new UrTransformParameters();
					}
				}
				else
				{
					transformParametersElementBase = new DocMdpTransformParametersElement();
					transformParameters = new DocMdpTransformParameters();
				}
				if (transformParametersElementBase == null)
				{
					return null;
				}
				transformParametersElementBase.CopyPropertiesTo(transformParameters);
				return transformParameters;
			}
			IL_72:
			throw new NotSupportedException(string.Format("Not supported transform method: {0}", transformMethod.Value));
		}

		readonly DirectProperty<PdfName> transformMethod;

		readonly DirectProperty<PrimitiveWrapper> transformParams;

		readonly ReferenceProperty<PrimitiveWrapper> data;

		readonly DirectProperty<PdfArray> digestLocation;

		readonly DirectProperty<PdfName> digestMethod;

		readonly DirectProperty<PdfString> digestValue;
	}
}

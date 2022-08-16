using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.InteractiveForms;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Forms
{
	class FormFieldNode : PdfObject, ITreeNode<FormFieldNode>, IVariableTextPropertiesObject
	{
		public FormFieldNode()
		{
			this.fieldType = base.RegisterDirectProperty<PdfName>(new PdfPropertyDescriptor("FT", false, PdfPropertyRestrictions.MustBeDirectObject));
			this.parent = base.RegisterReferenceProperty<FormFieldNode>(new PdfPropertyDescriptor("Parent", false, PdfPropertyRestrictions.MustBeIndirectReference));
			this.kids = base.RegisterReferenceProperty<PdfArray>(new PdfPropertyDescriptor("Kids"));
			this.partialName = base.RegisterDirectProperty<PdfString>(new PdfPropertyDescriptor("T"));
			this.userInterfaceName = base.RegisterDirectProperty<PdfString>(new PdfPropertyDescriptor("TU"));
			this.mappingName = base.RegisterDirectProperty<PdfString>(new PdfPropertyDescriptor("TM"));
			this.fieldFlags = base.RegisterDirectProperty<PdfInt>(new PdfPropertyDescriptor("Ff"));
			this.maxLengthOfInputCharacters = base.RegisterDirectProperty<PdfInt>(new PdfPropertyDescriptor("MaxLen"));
			this.fieldValue = base.RegisterReferenceProperty<PrimitiveWrapper>(new PdfPropertyDescriptor("V", false));
			this.defaultFieldValue = base.RegisterReferenceProperty<PrimitiveWrapper>(new PdfPropertyDescriptor("DV"));
			this.additionalActions = base.RegisterReferenceProperty<PdfDictionary>(new PdfPropertyDescriptor("AA"));
			this.defaultAppearance = base.RegisterReferenceProperty<PdfString>(new PdfPropertyDescriptor("DA"));
			this.quadding = base.RegisterDirectProperty<PdfInt>(new PdfPropertyDescriptor("Q"));
			this.options = base.RegisterReferenceProperty<PdfArray>(new PdfPropertyDescriptor("Opt"));
			this.topIndex = base.RegisterDirectProperty<PdfInt>(new PdfPropertyDescriptor("TI"), new PdfInt(0));
			this.sortedIndices = base.RegisterReferenceProperty<PdfArray>(new PdfPropertyDescriptor("I"));
			this.signatureFieldLock = base.RegisterReferenceProperty<SignatureFieldLock>(new PdfPropertyDescriptor("Lock", false, PdfPropertyRestrictions.MustBeIndirectReference));
			this.seedValue = base.RegisterReferenceProperty<SignatureFieldSeed>(new PdfPropertyDescriptor("SV", false, PdfPropertyRestrictions.MustBeIndirectReference));
		}

		public PdfName FieldType
		{
			get
			{
				return this.GetInheritableProperty((FormFieldNode formFieldNode) => formFieldNode.fieldType.GetValue(), null);
			}
			set
			{
				this.fieldType.SetValue(value);
			}
		}

		public FormFieldNode Parent
		{
			get
			{
				return this.parent.GetValue();
			}
			set
			{
				this.parent.SetValue(value);
			}
		}

		public PdfArray Kids
		{
			get
			{
				return this.kids.GetValue();
			}
			set
			{
				this.kids.SetValue(value);
			}
		}

		public PdfString PartialName
		{
			get
			{
				return this.partialName.GetValue();
			}
			set
			{
				this.partialName.SetValue(value);
			}
		}

		public PdfString UserInterfaceName
		{
			get
			{
				return this.userInterfaceName.GetValue();
			}
			set
			{
				this.userInterfaceName.SetValue(value);
			}
		}

		public PdfString MappingName
		{
			get
			{
				return this.mappingName.GetValue();
			}
			set
			{
				this.mappingName.SetValue(value);
			}
		}

		public PdfInt FieldFlags
		{
			get
			{
				return this.GetInheritableProperty((FormFieldNode formFieldNode) => formFieldNode.fieldFlags.GetValue(), new PdfInt(0));
			}
			set
			{
				this.fieldFlags.SetValue(value);
			}
		}

		public PdfInt MaxLengthOfInputCharacters
		{
			get
			{
				return this.GetInheritableProperty((FormFieldNode formFieldNode) => formFieldNode.maxLengthOfInputCharacters.GetValue(), null);
			}
			set
			{
				this.maxLengthOfInputCharacters.SetValue(value);
			}
		}

		public PrimitiveWrapper FieldValue
		{
			get
			{
				return this.GetInheritableProperty((FormFieldNode formFieldNode) => formFieldNode.fieldValue.GetValue(), null);
			}
			set
			{
				this.fieldValue.SetValue(value);
			}
		}

		public PrimitiveWrapper DefaultFieldValue
		{
			get
			{
				return this.GetInheritableProperty((FormFieldNode formFieldNode) => formFieldNode.defaultFieldValue.GetValue(), null);
			}
			set
			{
				this.defaultFieldValue.SetValue(value);
			}
		}

		public PdfDictionary AdditionalActions
		{
			get
			{
				return this.additionalActions.GetValue();
			}
			set
			{
				this.additionalActions.SetValue(value);
			}
		}

		public PdfArray Options
		{
			get
			{
				return this.GetInheritableProperty((FormFieldNode formFieldNode) => formFieldNode.options.GetValue(), null);
			}
			set
			{
				this.options.SetValue(value);
			}
		}

		public PdfInt TopIndex
		{
			get
			{
				return this.topIndex.GetValue();
			}
			set
			{
				this.topIndex.SetValue(value);
			}
		}

		public PdfArray SortedIndices
		{
			get
			{
				return this.sortedIndices.GetValue();
			}
			set
			{
				this.sortedIndices.SetValue(value);
			}
		}

		public SignatureFieldLock SignаtureFieldLock
		{
			get
			{
				return this.signatureFieldLock.GetValue();
			}
			set
			{
				this.signatureFieldLock.SetValue(value);
			}
		}

		public SignatureFieldSeed SeedValue
		{
			get
			{
				return this.seedValue.GetValue();
			}
			set
			{
				this.seedValue.SetValue(value);
			}
		}

		public string FullName
		{
			get
			{
				string text = null;
				for (FormFieldNode formFieldNode = this; formFieldNode != null; formFieldNode = formFieldNode.Parent)
				{
					if (formFieldNode.PartialName != null)
					{
						if (text == null)
						{
							text = formFieldNode.PartialName.ToString();
						}
						else
						{
							text = string.Format("{0}.{1}", formFieldNode.PartialName.ToString(), text);
						}
					}
				}
				return text;
			}
		}

		FormFieldNode ITreeNode<FormFieldNode>.NodeValue
		{
			get
			{
				return this;
			}
		}

		ITreeNode<FormFieldNode> ITreeNode<FormFieldNode>.ParentNode
		{
			get
			{
				return this.Parent;
			}
		}

		public PdfString GetDefaultAppearance(AcroFormObject form)
		{
			return this.GetInheritableProperty((FormFieldNode formFieldNode) => formFieldNode.defaultAppearance.GetValue(), form.DefaultAppearance);
		}

		public void SetDefaultAppearance(PdfString value)
		{
			this.defaultAppearance.SetValue(value);
		}

		public PdfInt GetQuadding(AcroFormObject form)
		{
			return this.GetInheritableProperty((FormFieldNode formFieldNode) => formFieldNode.quadding.GetValue(), form.Quadding);
		}

		public void SetQuadding(PdfInt value)
		{
			this.quadding.SetValue(value);
		}

		internal void CopyFormField(FormField field, IPdfExportContext context)
		{
			Guard.ThrowExceptionIfNull<FormField>(field, "field");
			FieldPropertiesExporter fieldPropertiesExporter = new FieldPropertiesExporter(this, context);
			fieldPropertiesExporter.ExportFieldProperties(field);
		}

		internal FormField ToFormField(PostScriptReader reader, IRadFixedDocumentImportContext context, bool importFieldProperties)
		{
			FormField formField;
			if (!context.TryGetField(this, out formField))
			{
				FieldCreationContext creationContext = new FieldCreationContext(context, this);
				formField = FieldsFactory.CreateField(creationContext);
				context.MapFields(this, formField);
				if (importFieldProperties)
				{
					FieldPropertiesImporter fieldPropertiesImporter = new FieldPropertiesImporter(this, reader, context);
					fieldPropertiesImporter.ImportFieldProperties(formField);
				}
			}
			return formField;
		}

		public const string TName = "T";

		public const string FieldValueName = "V";

		public const string KidsName = "Kids";

		public const string ParentName = "Parent";

		public const string OptionsName = "Opt";

		public const string SortedIndicesName = "I";

		readonly DirectProperty<PdfName> fieldType;

		readonly ReferenceProperty<FormFieldNode> parent;

		readonly ReferenceProperty<PdfArray> kids;

		readonly DirectProperty<PdfString> partialName;

		readonly DirectProperty<PdfString> userInterfaceName;

		readonly DirectProperty<PdfString> mappingName;

		readonly DirectProperty<PdfInt> fieldFlags;

		readonly DirectProperty<PdfInt> maxLengthOfInputCharacters;

		readonly ReferenceProperty<PrimitiveWrapper> fieldValue;

		readonly ReferenceProperty<PrimitiveWrapper> defaultFieldValue;

		readonly ReferenceProperty<PdfDictionary> additionalActions;

		readonly ReferenceProperty<PdfString> defaultAppearance;

		readonly DirectProperty<PdfInt> quadding;

		readonly ReferenceProperty<PdfArray> options;

		readonly DirectProperty<PdfInt> topIndex;

		readonly ReferenceProperty<PdfArray> sortedIndices;

		readonly ReferenceProperty<SignatureFieldLock> signatureFieldLock;

		readonly ReferenceProperty<SignatureFieldSeed> seedValue;
	}
}

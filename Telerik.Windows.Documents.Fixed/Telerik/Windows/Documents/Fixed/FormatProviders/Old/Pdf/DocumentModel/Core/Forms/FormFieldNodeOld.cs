using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Forms
{
	[PdfClass]
	class FormFieldNodeOld : PdfObjectOld, ITreeNode<FormFieldNodeOld>
	{
		public FormFieldNodeOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.fieldType = base.CreateInstantLoadProperty<PdfNameOld>(new PdfPropertyDescriptor
			{
				Name = "FT",
				IsInheritable = true
			});
			this.parent = base.CreateLoadOnDemandProperty<FormFieldNodeOld>(new PdfPropertyDescriptor
			{
				Name = "Parent"
			});
			this.kids = base.CreateLoadOnDemandProperty<PdfArrayOld>(new PdfPropertyDescriptor
			{
				Name = "Kids"
			});
			this.partialName = base.CreateInstantLoadProperty<PdfStringOld>(new PdfPropertyDescriptor
			{
				Name = "T"
			});
			this.userInterfaceName = base.CreateInstantLoadProperty<PdfStringOld>(new PdfPropertyDescriptor
			{
				Name = "TU"
			});
			this.mappingName = base.CreateInstantLoadProperty<PdfStringOld>(new PdfPropertyDescriptor
			{
				Name = "TM"
			});
			this.fieldFlags = base.CreateInstantLoadProperty<PdfIntOld>(new PdfPropertyDescriptor
			{
				Name = "Ff",
				IsInheritable = true
			});
			this.maxLengthOfInputCharacters = base.CreateInstantLoadProperty<PdfIntOld>(new PdfPropertyDescriptor
			{
				Name = "MaxLen",
				IsInheritable = true
			});
			this.fieldValue = base.CreateLoadOnDemandProperty<PdfObjectOld>(new PdfPropertyDescriptor
			{
				Name = "V",
				IsInheritable = true
			});
			this.defaultFieldValue = base.CreateLoadOnDemandProperty<PdfObjectOld>(new PdfPropertyDescriptor
			{
				Name = "DV",
				IsInheritable = true
			});
			this.additionalActions = base.CreateLoadOnDemandProperty<PdfDictionaryOld>(new PdfPropertyDescriptor
			{
				Name = "AA"
			});
			this.defaultAppearance = base.CreateInstantLoadProperty<PdfStringOld>(new PdfPropertyDescriptor
			{
				Name = "DA",
				IsInheritable = true
			});
			this.quadding = base.CreateInstantLoadProperty<PdfIntOld>(new PdfPropertyDescriptor
			{
				Name = "Q",
				IsInheritable = true
			});
			this.options = base.CreateLoadOnDemandProperty<PdfArrayOld>(new PdfPropertyDescriptor
			{
				Name = "Opt",
				IsInheritable = true
			});
			this.topIndex = base.CreateInstantLoadProperty<PdfIntOld>(new PdfPropertyDescriptor
			{
				Name = "TI"
			}, new PdfIntOld(contentManager, 0));
			this.sortedIndices = base.CreateLoadOnDemandProperty<PdfArrayOld>(new PdfPropertyDescriptor
			{
				Name = "I"
			});
			this.signatureFieldLock = base.CreateLoadOnDemandProperty<SignatureFieldLockOld>(new PdfPropertyDescriptor
			{
				Name = "Lock"
			}, Converters.PdfDictionaryToPdfObjectConverter);
			this.seedValue = base.CreateLoadOnDemandProperty<SignatureFieldSeedOld>(new PdfPropertyDescriptor
			{
				Name = "SV"
			}, Converters.PdfDictionaryToPdfObjectConverter);
		}

		public PdfNameOld FieldType
		{
			get
			{
				return this.GetInheritableProperty((FormFieldNodeOld formFieldNode) => formFieldNode.fieldType.GetValue(), null);
			}
			set
			{
				this.fieldType.SetValue(value);
			}
		}

		public FormFieldNodeOld Parent
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

		public PdfArrayOld Kids
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

		public PdfStringOld PartialName
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

		public PdfStringOld UserInterfaceName
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

		public PdfStringOld MappingName
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

		public PdfIntOld MaxLengthOfInputCharacters
		{
			get
			{
				return this.GetInheritableProperty((FormFieldNodeOld formFieldNode) => formFieldNode.maxLengthOfInputCharacters.GetValue(), null);
			}
			set
			{
				this.maxLengthOfInputCharacters.SetValue(value);
			}
		}

		public PdfObjectOld FieldValue
		{
			get
			{
				return this.GetInheritableProperty((FormFieldNodeOld formFieldNode) => formFieldNode.fieldValue.GetValue(), null);
			}
			set
			{
				this.fieldValue.SetValue(value);
			}
		}

		public PdfObjectOld DefaultFieldValue
		{
			get
			{
				return this.GetInheritableProperty((FormFieldNodeOld formFieldNode) => formFieldNode.defaultFieldValue.GetValue(), null);
			}
			set
			{
				this.defaultFieldValue.SetValue(value);
			}
		}

		public PdfDictionaryOld AdditionalActions
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

		public PdfStringOld DefaultAppearance
		{
			get
			{
				return this.GetInheritableProperty((FormFieldNodeOld formFieldNode) => formFieldNode.defaultAppearance.GetValue(), base.ContentManager.DocumentCatalog.AcroForm.DefaultAppearance);
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
				return this.GetInheritableProperty((FormFieldNodeOld formFieldNode) => formFieldNode.quadding.GetValue(), base.ContentManager.DocumentCatalog.AcroForm.Quadding);
			}
			set
			{
				this.quadding.SetValue(value);
			}
		}

		public PdfArrayOld Options
		{
			get
			{
				return this.GetInheritableProperty((FormFieldNodeOld formFieldNode) => formFieldNode.options.GetValue(), null);
			}
			set
			{
				this.options.SetValue(value);
			}
		}

		public PdfIntOld TopIndex
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

		public PdfArrayOld SortedIndices
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

		public SignatureFieldLockOld SignatureFieldLock
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

		public SignatureFieldSeedOld SeedValue
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
				string text = this.PartialName.ToString();
				for (FormFieldNodeOld formFieldNodeOld = this.Parent; formFieldNodeOld != null; formFieldNodeOld = formFieldNodeOld.Parent)
				{
					if (formFieldNodeOld.PartialName != null)
					{
						text = string.Format("{0}.{1}", formFieldNodeOld.PartialName.ToString(), text);
					}
				}
				return text;
			}
		}

		FormFieldNodeOld ITreeNode<FormFieldNodeOld>.NodeValue
		{
			get
			{
				return this;
			}
		}

		ITreeNode<FormFieldNodeOld> ITreeNode<FormFieldNodeOld>.ParentNode
		{
			get
			{
				return this.Parent;
			}
		}

		readonly InstantLoadProperty<PdfNameOld> fieldType;

		readonly LoadOnDemandProperty<FormFieldNodeOld> parent;

		readonly LoadOnDemandProperty<PdfArrayOld> kids;

		readonly InstantLoadProperty<PdfStringOld> partialName;

		readonly InstantLoadProperty<PdfStringOld> userInterfaceName;

		readonly InstantLoadProperty<PdfStringOld> mappingName;

		readonly InstantLoadProperty<PdfIntOld> fieldFlags;

		readonly InstantLoadProperty<PdfIntOld> maxLengthOfInputCharacters;

		readonly LoadOnDemandProperty<PdfObjectOld> fieldValue;

		readonly LoadOnDemandProperty<PdfObjectOld> defaultFieldValue;

		readonly LoadOnDemandProperty<PdfDictionaryOld> additionalActions;

		readonly InstantLoadProperty<PdfStringOld> defaultAppearance;

		readonly InstantLoadProperty<PdfIntOld> quadding;

		readonly LoadOnDemandProperty<PdfArrayOld> options;

		readonly InstantLoadProperty<PdfIntOld> topIndex;

		readonly LoadOnDemandProperty<PdfArrayOld> sortedIndices;

		readonly LoadOnDemandProperty<SignatureFieldLockOld> signatureFieldLock;

		readonly LoadOnDemandProperty<SignatureFieldSeedOld> seedValue;
	}
}

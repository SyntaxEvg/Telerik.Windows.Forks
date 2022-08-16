using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Types;
using Telerik.Windows.Documents.Flow.Model.Fields;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	class FieldCharacterElement : DocumentElementBase
	{
		public FieldCharacterElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.filedCharTypeAttribute = new MappedOpenXmlAttribute<FieldCharacterType>("fldCharType", OpenXmlNamespaces.WordprocessingMLNamespace, TypeMappers.FileldCharTypeMapper, true);
			base.RegisterAttribute<MappedOpenXmlAttribute<FieldCharacterType>>(this.filedCharTypeAttribute);
			this.isLocked = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("fldLock", OpenXmlNamespaces.WordprocessingMLNamespace, false));
			this.isDirty = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("dirty", OpenXmlNamespaces.WordprocessingMLNamespace, false));
		}

		public override string ElementName
		{
			get
			{
				return "fldChar";
			}
		}

		public FieldCharacterType FieldCharacterType
		{
			get
			{
				return this.filedCharTypeAttribute.Value;
			}
			set
			{
				this.filedCharTypeAttribute.Value = value;
			}
		}

		public bool IsLocked
		{
			get
			{
				return this.isLocked.Value;
			}
			set
			{
				this.isLocked.Value = value;
			}
		}

		public bool IsDirty
		{
			get
			{
				return this.isDirty.Value;
			}
			set
			{
				this.isDirty.Value = value;
			}
		}

		readonly MappedOpenXmlAttribute<FieldCharacterType> filedCharTypeAttribute;

		readonly BoolOpenXmlAttribute isLocked;

		readonly BoolOpenXmlAttribute isDirty;
	}
}

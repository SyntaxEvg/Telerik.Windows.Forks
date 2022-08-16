using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Common;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Types;
using Telerik.Windows.Documents.Flow.Model.Protection;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	class PermissionStartElement : AnnotationStartEndElementBase
	{
		public PermissionStartElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.editorCredentialsAttribute = base.RegisterAttribute<string>("ed", OpenXmlNamespaces.WordprocessingMLNamespace, false);
			this.editingGroupAttribute = base.RegisterAttribute<MappedOpenXmlAttribute<EditingGroup?>>(new MappedOpenXmlAttribute<EditingGroup?>("edGrp", OpenXmlNamespaces.WordprocessingMLNamespace, TypeMappers.EditingGroupTypeMapper, false));
			this.colFirstAttribute = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("colFirst", OpenXmlNamespaces.WordprocessingMLNamespace, false));
			this.colLastAttribute = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("colLast", OpenXmlNamespaces.WordprocessingMLNamespace, false));
		}

		public override string ElementName
		{
			get
			{
				return "permStart";
			}
		}

		public string Editor
		{
			get
			{
				return this.editorCredentialsAttribute.Value;
			}
			set
			{
				this.editorCredentialsAttribute.Value = value;
			}
		}

		public EditingGroup? EditingGroup
		{
			get
			{
				return this.editingGroupAttribute.Value;
			}
			set
			{
				this.editingGroupAttribute.Value = value;
			}
		}

		public int ColFirst
		{
			get
			{
				return this.colFirstAttribute.Value;
			}
			set
			{
				this.colFirstAttribute.Value = value;
			}
		}

		public int ColLast
		{
			get
			{
				return this.colLastAttribute.Value;
			}
			set
			{
				this.colLastAttribute.Value = value;
			}
		}

		internal void CopyPropertiesFrom(PermissionRange permission)
		{
			base.Id = AnnotationIdGenerator.GetNext();
			if (permission.Credentials.EditingGroup != null)
			{
				this.EditingGroup = new EditingGroup?(permission.Credentials.EditingGroup.Value);
			}
			if (!string.IsNullOrEmpty(permission.Credentials.Editor))
			{
				this.Editor = permission.Credentials.Editor;
			}
			if (permission.FromColumn != permission.ToColumn)
			{
				this.ColFirst = permission.FromColumn.Value;
				this.ColLast = permission.ToColumn.Value;
			}
		}

		readonly OpenXmlAttribute<string> editorCredentialsAttribute;

		readonly MappedOpenXmlAttribute<EditingGroup?> editingGroupAttribute;

		readonly IntOpenXmlAttribute colFirstAttribute;

		readonly IntOpenXmlAttribute colLastAttribute;
	}
}

using System;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Attributes;
using Telerik.Documents.SpreadsheetStreaming.Model.Relations;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements
{
	class SheetElement : DirectElementBase<Relationship>
	{
		public SheetElement()
		{
			this.sheetId = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("sheetId", true));
			this.name = base.RegisterAttribute<string>("name", true);
			this.relationId = base.RegisterAttribute<OpenXmlAttribute<string>>(new OpenXmlAttribute<string>("id", OpenXmlNamespaces.OfficeDocumentRelationshipsNamespace, true));
		}

		public override string ElementName
		{
			get
			{
				return "sheet";
			}
		}

		int SheetId
		{
			get
			{
				return this.sheetId.Value;
			}
			set
			{
				this.sheetId.Value = value;
			}
		}

		string Name
		{
			get
			{
				return this.name.Value;
			}
			set
			{
				this.name.Value = value;
			}
		}

		string RelationId
		{
			get
			{
				return this.relationId.Value;
			}
			set
			{
				this.relationId.Value = value;
			}
		}

		protected override void InitializeAttributesOverride(Relationship value)
		{
			this.Name = value.Name;
			this.SheetId = value.Id;
			this.RelationId = value.RelationshipId;
		}

		protected override void WriteChildElementsOverride(Relationship value)
		{
		}

		protected override void CopyAttributesOverride(ref Relationship value)
		{
			value = new Relationship(this.Name, this.SheetId, this.RelationId, null);
		}

		protected override void ReadChildElementOverride(ElementBase element, ref Relationship value)
		{
		}

		readonly IntOpenXmlAttribute sheetId;

		readonly OpenXmlAttribute<string> name;

		readonly OpenXmlAttribute<string> relationId;
	}
}

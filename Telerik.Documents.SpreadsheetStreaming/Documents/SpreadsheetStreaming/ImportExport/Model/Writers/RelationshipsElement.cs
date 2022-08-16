using System;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.Model.Relations;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Writers
{
	class RelationshipsElement : DirectElementBase<Relationships>
	{
		public override string ElementName
		{
			get
			{
				return "Relationships";
			}
		}

		public override OpenXmlNamespace Namespace
		{
			get
			{
				return OpenXmlNamespaces.RelationshipsNamespace;
			}
		}

		protected override void InitializeAttributesOverride(Relationships value)
		{
		}

		protected override void WriteChildElementsOverride(Relationships value)
		{
			foreach (Relationship value2 in value)
			{
				RelationshipElement relationshipElement = base.CreateChildElement<RelationshipElement>();
				relationshipElement.Write(value2);
			}
		}

		protected override void CopyAttributesOverride(ref Relationships value)
		{
		}

		protected override void ReadChildElementOverride(ElementBase element, ref Relationships value)
		{
			RelationshipElement relationshipElement = element as RelationshipElement;
			Relationship relationship = null;
			relationshipElement.Read(ref relationship);
			value.AddRelationship(relationship);
		}
	}
}

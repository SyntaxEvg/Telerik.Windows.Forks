using System;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Attributes;
using Telerik.Documents.SpreadsheetStreaming.Model.Relations;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Writers
{
	class RelationshipElement : DirectElementBase<Relationship>
	{
		public RelationshipElement()
		{
			this.idAttribute = base.RegisterAttribute<string>("Id", true);
			this.typeAttribute = base.RegisterAttribute<string>("Type", true);
			this.targetAttribute = base.RegisterAttribute<string>("Target", true);
			this.targetModeAttribute = base.RegisterAttribute<string>("TargetMode", string.Empty, false);
		}

		public override string ElementName
		{
			get
			{
				return "Relationship";
			}
		}

		string Target
		{
			get
			{
				return this.targetAttribute.Value;
			}
			set
			{
				this.targetAttribute.Value = value;
			}
		}

		string Type
		{
			get
			{
				return this.typeAttribute.Value;
			}
			set
			{
				this.typeAttribute.Value = value;
			}
		}

		string Id
		{
			get
			{
				return this.idAttribute.Value;
			}
			set
			{
				this.idAttribute.Value = value;
			}
		}

		string TargetMode
		{
			set
			{
				this.targetModeAttribute.Value = value;
			}
		}

		protected override void InitializeAttributesOverride(Relationship value)
		{
			this.Id = value.RelationshipId;
			this.Type = value.Type;
			this.Target = value.Target;
		}

		protected override void CopyAttributesOverride(ref Relationship value)
		{
			value = new Relationship(this.Id, this.Type, this.Target);
		}

		protected override void WriteChildElementsOverride(Relationship value)
		{
		}

		protected override void ReadChildElementOverride(ElementBase element, ref Relationship value)
		{
		}

		readonly OpenXmlAttribute<string> targetAttribute;

		readonly OpenXmlAttribute<string> typeAttribute;

		readonly OpenXmlAttribute<string> idAttribute;

		readonly OpenXmlAttribute<string> targetModeAttribute;
	}
}
